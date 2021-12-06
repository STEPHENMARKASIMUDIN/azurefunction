using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace AzureFunctionTest
{
    public class Credentials
    {
        private static readonly object _InstanceLock = new object();
        private static Credentials _Credentials;
        private readonly string _APIKey = ReadJson();

        public static Credentials Instance
        {
            get
            {
                if (_Credentials == null)
                {
                    lock (_InstanceLock)
                    {
                        if (_Credentials == null)
                        {
                            _Credentials = new Credentials();
                        }
                    }
                }
                return _Credentials;
            }
        }
        public string APIKey => _APIKey;

        public static string GetFileJson(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("utf-8")))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return json;
        }
        //Read Json Value
        public static string ReadJson()
        {
            string jsonfile = "host.json";
            string jsonText = GetFileJson(jsonfile);
            JObject jsonObj = JObject.Parse(jsonText);
            string value = ((JObject)jsonObj["Credentials"])["APIKey"].ToString();
            return value;
        }        
    }
}

