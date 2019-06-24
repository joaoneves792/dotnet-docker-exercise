using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http.Features;
using dotnet_etcd;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using System;
using Microsoft.Extensions.Configuration;

namespace MvcMovie.Controllers
{
    public class HelloDockerController : Controller
    {
        // 
        // GET: /HelloDocker/

        private readonly IConfiguration _configuration = null;


        public HelloDockerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index(string key)
        {
            EtcdClient client = new EtcdClient("etcd", 2379);
            Etcdserverpb.RangeResponse response;
            if (key == null)
                response = client.Get("foo");
            else
                response = client.Get(key);

            try
            {
                RepeatedField<Mvccpb.KeyValue> kvs = response.Kvs;
                IEnumerator<Mvccpb.KeyValue> enumerator = kvs.GetEnumerator();
                enumerator.MoveNext();
                string value = enumerator.Current.Value.ToStringUtf8();

                ViewData["Message"] = "endpoint is:" + _configuration["endpoint"]  + " etcd says: " + value;
            }catch(Exception e){
                ViewData["Message"] = e.ToString();
            }
            IHttpConnectionFeature feature = HttpContext.Features.Get<IHttpConnectionFeature>();
            ViewData["IP"] = feature.LocalIpAddress.ToString();
            ViewData["Config"] = _configuration["Foo"];

            client.Put("visited", feature.LocalIpAddress.ToString());

            return View();
        }
    }
}