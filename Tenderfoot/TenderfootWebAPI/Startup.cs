using Tenderfoot.Mvc;
using Microsoft.Extensions.Configuration;

namespace TenderfootWebAPI
{
    public class Startup : TfStartup
    {
        public Startup(IConfiguration configuration) { }
    }
}
