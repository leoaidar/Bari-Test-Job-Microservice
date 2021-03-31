using Microsoft.Extensions.Configuration;
using System.IO;

namespace Bari.Test.Job.Infra.Bus
{
    public class AppConfiguration
    {
        public readonly string _connectionString;
        public readonly IConfigurationRoot _appSettings;

        public AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            _appSettings = configurationBuilder.Build();            
        }

        public string ConnectionString(string key)
        {
            return _appSettings.GetConnectionString(key);
        }

        public IConfigurationRoot AppSettings
        {
            get => _appSettings;
        }
    }
}
