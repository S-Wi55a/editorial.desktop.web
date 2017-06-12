import {isProd} from '../Shared/env.config.js'
import {listofTenants, TENANTS} from '../Shared/tenants.config.js'
import {stats} from '../Shared/stats.config.js'
import {devServer} from '../Shared/devServer.config.js'
import {resolve} from '../Shared/resolve.config.js'
import rimraf from 'rimraf'

//From Client/
import {config, getEntryFiles} from './entries.config.js'
import {plugins} from './plugins.config.js'
import {modules} from './loaders.config.js'


// Remove dist folder
rimraf('./dist', function (err) { if (err) { throw err; } });

module.exports = () => {

    let moduleExportArr = [];

    //run through list of tenants
    TENANTS.forEach((tenant) => {

        // The order here matters or the CommonChunkPlugin
        const entries = getEntryFiles(tenant);
        const pageEntries = Object.keys(getEntryFiles(tenant));

        //That is why these entries are added after
        entries['vendor' + '--' + tenant] = ['./Features/Shared/Assets/Js/vendor.js'];
        entries['csn.base' + '--' + tenant] = ['./Features/Shared/Assets/csn.base.js'];
        entries['csn.mm' + '--' + tenant] = ['./Features/Shared/Assets/Js/Modules/MediaMotive/mm.js'];

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
                resolve: resolve,
                plugins: plugins(tenant, pageEntries),
                stats: stats,
                devtool: isProd ? "cheap-source-map" : "eval",
                devServer: devServer(tenant)
            })
    });
    return moduleExportArr

}
