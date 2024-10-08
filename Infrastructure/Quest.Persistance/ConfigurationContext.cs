using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Quest.Persistance
{
    public static class ConfigurationContext
    {
        public static string ConnectionString
        {
            get
            {
                ConfigurationManager configuration = new();
                configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),"../../Presentation/Quest.API"));
                configuration.AddJsonFile("appsettings.json");

                return configuration.GetConnectionString("PostgreSQL");
            }
        }

    }
}
