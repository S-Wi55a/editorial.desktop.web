import {isProd} from '../Shared/env.config.js'
import {listofTenants, TENANTS} from '../Shared/tenants.config.js'
import {rootRelativePath, listOfPaths, s3path} from '../Shared/paths.config.js'
import {config, getEntryFiles} from '../Shared/entries.config.js'
import {modules} from '../Shared/loaders.config.js'
import {plugins} from '../Shared/plugins.config.js'
import {stats} from '../Shared/stats.config.js'
import {devServer} from '../Shared/devServer.config.js'
import path from 'path'
import rimraf from 'rimraf'

// Remove dist folder
rimraf('./dist', function (err) { if (err) { throw err; } });

module.exports = (env) => {

    process.env.BABEL_ENV = env;

    let moduleExportArr = [];

    //run through list of tenants
    TENANTS.forEach((tenant) => {

        const entries = getEntryFiles(tenant);

        const pageEntries = Object.keys(getEntryFiles(tenant));

        entries['vendor' + '--' + tenant] = ['./Features/Shared/Assets/Js/vendor.js'];
        entries['csn.base' + '--' + tenant] = ['./Features/Shared/Assets/csn.base.js'];
        entries['csn.mm' + '--' + tenant] = ['./Features/Shared/Assets/Js/Modules/MediaMotive/mm.js'];

        entries['react-server-components' + '--' + tenant] = ['./Features/ReactServerRender/Assets/Js/react-server-components.js'];

        moduleExportArr.push(
            {
                name: tenant,
                entry: entries,
                output: {
                    path: config.outputPath,
                    publicPath: config.publicPath,
                    filename: isProd ? '[name]-[chunkhash].js' : '[name].js'
                },
                module: modules(tenant),
                resolve: {
                    extensions: ['.js','.scss'],
                    alias: {
                        modernizr$: path.resolve('./.modernizrrc.js'),
                        'debug.addIndicators': path.resolve('node_modules', 'scrollmagic/scrollmagic/uncompressed/plugins/debug.addIndicators.js'),
                    },
                    //aliasFields: ["browser"],
                    descriptionFiles: ['package.json', 'bower.json'],
                    modules: listOfPaths
                },
                plugins: plugins(tenant, pageEntries),
                stats: stats,
                devtool: isProd ? "cheap-source-map" : "eval",
                devServer: devServer(tenant)
            })
    });
    return moduleExportArr

}
