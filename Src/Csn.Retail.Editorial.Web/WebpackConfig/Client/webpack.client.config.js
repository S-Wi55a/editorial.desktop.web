import {isProd} from '../Shared/env.config.js'
import {listofTenants, TENANTS} from '../Shared/tenants.config.js'
import {modules} from '../Shared/loaders.config.js'
import {stats} from '../Shared/stats.config.js'
import {devServer} from '../Shared/devServer.config.js'
import {resolve} from '../Shared/resolve.config.js'
import path from 'path'
import rimraf from 'rimraf'

//From Client/
import {config, getEntryFiles} from '../Client/entries.config.js'
import {plugins} from '../Client/plugins.config.js'



// Remove dist folder
rimraf('./dist', function (err) { if (err) { throw err; } });

module.exports = () => {

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
                resolve: resolve,
                plugins: plugins(tenant, pageEntries),
                stats: stats,
                devtool: isProd ? "cheap-source-map" : "eval",
                devServer: devServer(tenant)
            })
    });
    return moduleExportArr

}
