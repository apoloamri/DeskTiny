using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DeskTinyFront
{
    public class BaseModel : PageModel
    {
        public static IConfigurationRoot Configuration()
        {
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return configurationBuilder.Build();
        }

        public string ApiUrl
        {
            get
            {
                return Configuration().GetSection("Api").GetSection("Url").Value;
            }
        }
    }
}
