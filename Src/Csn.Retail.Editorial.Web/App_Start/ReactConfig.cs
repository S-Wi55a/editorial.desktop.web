using React;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Csn.Retail.Editorial.Web.ReactConfig), "Configure")]

namespace Csn.Retail.Editorial.Web
{
	public static class ReactConfig
	{
		public static void Configure()
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

		    ReactSiteConfiguration.Configuration
		        .SetStartEngines(1)
		        .SetMaxEngines(1)
                .SetUseDebugReact(true)
		        .SetReuseJavaScriptEngines(false) //TODO: remove in Prod
                .SetLoadBabel(false)
                .SetLoadReact(false)
                .AddScriptWithoutTransform("/dist--server/react-server-components.js")
                //.DisableServerSideRendering()
                ;


		}
    }
}