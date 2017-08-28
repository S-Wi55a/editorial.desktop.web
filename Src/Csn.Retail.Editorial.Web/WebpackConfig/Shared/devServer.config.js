export const devServer = (tenant) => ( {
    stats: {
        // Add asset Information
        assets: true,
            // Sort assets by a field
            assetsSort: "field",
            // Add information about cached (not built) modules
            cached: true,
            // Show cached assets (setting this to `false` only shows emitted files)
            cachedAssets: false,
            // Add children information
            children: true,
            // Add chunk information (setting this to `false` allows for a less verbose output)
            chunks: false,
            // Add built modules information to chunk information
            chunkModules: true,
            // Add the origins of chunks and chunk merging info
            chunkOrigins: false,
            // Sort the chunks by a field
            chunksSort: "field",
            // Context directory for request shortening
            //context: "../src/",
            // `webpack --colors` equivalent
            colors: true,
            // Add errors
            errors: true,
            // Add details to errors (like resolving log)
            errorDetails: true,
            // Add the hash of the compilation
            hash: false,
            // Add built modules information
            modules: false,
            // Sort the modules by a field
            modulesSort: "field",
            // Show performance hint when file size exceeds `performance.maxAssetSize`
            performance: true,
            // Add public path information
            publicPath: false,
            // Add information about the reasons why modules are included
            reasons: false,
            // Add the source code of modules
            source: false,
            // Add timing information
            timings: true,
            // Add webpack version information
            version: true,
            // Add warnings
            warnings: true
    },
    proxy: {
        '/': {
            target: 'http://' + (tenant || 'carsales').toString().toLowerCase() + '.editorial.csdev.com.au',
                changeOrigin: true,
                secure: false
        }
    }
})