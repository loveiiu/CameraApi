using Newtonsoft.Json;
using System.IO;
using WebApi.Utilities;

namespace WebApi.Core
{
    public interface ISystemConfig
    {
        int Port { get; set; }
        string ConnectionString { get; set; }
        bool LaunchBrowser { get; set; }
        AuthObject Authorization { get; set; }
    }
    public class AuthObject
    {
        public string Key { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class SystemConfig : ISystemConfig
    {
        public int Port { get; set; }
        public string ConnectionString { get; set; }
        public bool LaunchBrowser { get; set; }

        public AuthObject Authorization { get; set; }
        public static SystemConfig Reload()
        {
            string configFile = Helper.MapPath(string.Format("config.{0}.json", System.Environment.MachineName));
            if (File.Exists(configFile) == false)
            {
                configFile = Helper.MapPath("config.json");
            }
            string json = File.ReadAllText(configFile);
            return JsonConvert.DeserializeObject<SystemConfig>(json);
        }
    }
}
