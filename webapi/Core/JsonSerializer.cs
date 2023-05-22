using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApi.Core
{
    public interface ISerializer
    {
        string Serialize(object input, bool indented = false);
        T Deserialize<T>(string input);
        object Deserialize(string input, Type objType);
        dynamic DeserializeDynamic(string input);
        bool TryDeserialize<T>(string input, out T t);
    }

    public class JsonSerializer : ISerializer
    {
        #region ISerializer Members

        public string Serialize(object input, bool indented = false)
        {
            return JsonConvert.SerializeObject(input, indented ? Formatting.Indented : Formatting.None);
        }

        public T Deserialize<T>(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(input);
        }

        public object Deserialize(string input, Type objType)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            return JsonConvert.DeserializeObject(input, objType);
        }

        public dynamic DeserializeDynamic(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            return JObject.Parse(input);
        }

        public bool TryDeserialize<T>(string input, out T t)
        {
            try
            {
                t = JsonConvert.DeserializeObject<T>(input);
                return true;
            }
            catch
            {
                t = default(T);
                return false;
            }
        }
        #endregion
    }
}
