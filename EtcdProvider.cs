using dotnet_etcd;
using Google.Protobuf.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;


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

        public EtcdProvider(Microsoft.Extensions.Configuration.Json.JsonConfigurationSource source) : base(source)
        {

        }

        public override void Load()
        {
            base.Load();
            foreach (KeyValuePair<string, string> entry in Data)
            {
                Console.WriteLine(entry.Value);
            }
        }
    }
}
