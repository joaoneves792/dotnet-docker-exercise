using dotnet_etcd;
using Google.Protobuf.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace dotnet_exercise
{

    public class EtcdSource : JsonConfigurationSource
    {
        private readonly string _filename;

        public EtcdSource(string filename)
        {
            _filename = filename;
        }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            base.Build(builder);
            base.Path = _filename;
            base.Optional = false;

            return new EtcdProvider(this);
        }
    }


    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddEtcdProvider(
            this IConfigurationBuilder configuration, string filename)
        {
            configuration.Add(new EtcdSource(filename));
            return configuration;
        }
    }



    public class EtcdProvider : JsonConfigurationProvider
    {
        private readonly string tokenPattern = @"\${([a-zA-Z0-9-_]+)}";

        public EtcdProvider(Microsoft.Extensions.Configuration.Json.JsonConfigurationSource source) : base(source)
        {

        }

        private string retrieveValue(string key, string etcdEndpoint, int etcdPort)
        {
            EtcdClient client = new EtcdClient(etcdEndpoint, etcdPort);
            Etcdserverpb.RangeResponse response;
            response = client.Get(key);

            RepeatedField<Mvccpb.KeyValue> kvs = response.Kvs;
            IEnumerator<Mvccpb.KeyValue> enumerator = kvs.GetEnumerator();
            enumerator.MoveNext();
            string value = enumerator.Current.Value.ToStringUtf8();

            return value;
        }

        public override void Load()
        {
            base.Load();
            foreach (KeyValuePair<string, string> entry in Data)
            {
                MatchCollection matches = Regex.Matches(entry.Value, tokenPattern);
                if (matches.Count == 0)
                    continue;

                foreach (Match match in matches)
                {
                    string etcdKey = match.Groups[1].Value;
                    string etcdValue = null;
                    while (etcdValue == null)
                    {
                        try
                        {
                            etcdValue = retrieveValue(etcdKey, "etcd", 2379);
                        }
                        catch (Exception e) //Catch'em all
                        {
                            Console.WriteLine("etcd token resolution error: " + e.Message);
                            Thread.Sleep(1000);
                        }
                    }

                    Console.WriteLine(etcdKey + " : " + etcdValue);
                }
                
            }
        }
    }
}
