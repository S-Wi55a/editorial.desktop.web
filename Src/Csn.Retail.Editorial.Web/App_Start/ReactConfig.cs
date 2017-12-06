using Autofac;
using Csn.Retail.Editorial.Web.Infrastructure.Settings;
using React;

namespace Csn.Retail.Editorial.Web
{
	public static class ReactConfig
	{
		public static void Configure(IContainer container)
		{
            // If you want to use server-side rendering of React components, 
            // add all the necessary JavaScript files here. This includes 
            // your components as well as all of their dependencies.
            // See http://reactjs.net/ for more information. Example:

            // If you use an external build too (for example, Babel, Webpack,
            // Browserify or Gulp), you can improve performance by disabling 
            // ReactJS.NET's version of Babel and loading the pre-transpiled 
            // scripts. Example:
            //ReactSiteConfiguration.Configuration
            //	.SetLoadBabel(false)
            //	.AddScriptWithoutTransform("~/Scripts/bundle.server.js")

		    var settings = container.Resolve<ReactNetSettings>();

		    ReactSiteConfiguration.Configuration
		        .SetStartEngines(settings.StartEngines)
		        .SetMaxEngines(settings.MaxEngines)
                .SetUseDebugReact(settings.UseDebugReact)
		        .SetReuseJavaScriptEngines(settings.ReuseJavaScriptEngines)
                .SetLoadBabel(false)
                .SetLoadReact(false)
                .AddScriptWithoutTransform("/dist--server/react-server-components.js")
                //.DisableServerSideRendering()
                ;
		}
    }
}