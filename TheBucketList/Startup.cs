using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheBucketList.Startup))]
namespace TheBucketList
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
