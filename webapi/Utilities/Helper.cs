using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Net;
using System.Diagnostics;
using WebApi.Enums;
using WebApi.Core;

namespace WebApi.Utilities
{
    public static class Helper
    {
        public static string MapPath(params string[] paths)
        {
            if (paths.Length == 1)
            {
                return Path.Combine(GetRootPath(), paths[0]);
            }
            else if (paths.Length == 2)
            {
                return Path.Combine(GetRootPath(), paths[0], paths[1]);
            }
            else if (paths.Length == 3)
            {
                return Path.Combine(GetRootPath(), paths[0], paths[1], paths[2]);
            }
            else if (paths.Length > 3)
            {
                throw new Exception("GetRelativePath 目前不支援超過3個目錄參數陣列");
            }
            return GetRootPath();
        }

        private static string rootPath;

        public static void SetRootPath(string rootPath)
        {
            Helper.rootPath = rootPath;
        }

        public static string GetRootPath()
        {
            if (rootPath != null)
            {
                return rootPath;
            }
            if (rootPath == null)
            {
                if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "config.json")))
                {
                    rootPath = Directory.GetCurrentDirectory();
                }
            }
            if (rootPath == null)
            {
                rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            return rootPath;
        }

        public static string GetBasePath()
        {
            var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }

        public static string HttpWebRequest<T>(string apiUrl, T gwApiParams, out HttpStatusCode statusCode, Dictionary<string, string> headers = null, HttpWebRequestMethod tsRequestMethod = HttpWebRequestMethod.POST)
        {
            statusCode = HttpStatusCode.NotFound;
            var request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = WebRequestMethods.Http.Post;
            var jsonText = new JsonSerializer().Serialize(gwApiParams);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }


            var jsonBytes = Encoding.UTF8.GetBytes(jsonText);
            request.Method = tsRequestMethod.ToString();
            request.ContentType = "application/json";
            request.ContentLength = jsonBytes.Length;
            int secs = 100000;

            request.Timeout = secs;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            try
            {
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(jsonBytes, 0, jsonBytes.Length);
                    requestStream.Flush();
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    statusCode = response.StatusCode;
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null) return null;
                        using (var reader = new StreamReader(stream))
                        {

                            var responseResult = reader.ReadToEnd();
                            return responseResult;
                        }
                    }
                }

            }
            catch (WebException ex)
            {
                var response = (HttpWebResponse)ex.Response;
                if (response != null)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null) return null;
                        using (var reader = new StreamReader(stream))
                        {

                            var responseResult = reader.ReadToEnd();
                            return responseResult;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public static string HttpWebRequest(string apiUrl, out HttpStatusCode statusCode, Dictionary<string, string> headers = null, HttpWebRequestMethod tsRequestMethod = HttpWebRequestMethod.GET)
        {
            statusCode = HttpStatusCode.NotFound;
            var request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = WebRequestMethods.Http.Get;

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }

            int secs = 100000;

            request.Timeout = secs;

            bool _keepAlive = false;

            request.KeepAlive = _keepAlive;
            ServicePointManager.DefaultConnectionLimit = 50;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    statusCode = response.StatusCode;
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null) return null;
                        using (var reader = new StreamReader(stream))
                        {

                            var responseResult = reader.ReadToEnd();
                            return responseResult;
                        }
                    }
                }

            }
            catch (WebException ex)
            {
                var response = (HttpWebResponse)ex.Response;
                statusCode = response.StatusCode;
                using (var stream = response.GetResponseStream())
                {
                    if (stream == null) return null;
                    using (var reader = new StreamReader(stream))
                    {

                        var responseResult = reader.ReadToEnd();
                        return responseResult;
                    }
                }
            }

        }

        public static string HmacSHA256(string message, string key)
        {
            key = key ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new System.Security.Cryptography.HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        public static Dictionary<string, TValue> ToDictionary<TValue>(object obj)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, TValue>>(json);
            return dictionary;
        }
    }
}
