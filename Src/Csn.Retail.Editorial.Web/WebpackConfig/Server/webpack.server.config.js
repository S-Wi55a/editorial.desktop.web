import {isProd} from '../Shared/env.config.js'
import {listofTenants, TENANTS} from '../Shared/tenants.config.js'
import {resolve} from '../Shared/resolve.config.js'
import {stats} from '../Shared/stats.config.js'
import {devServer} from '../Shared/devServer.config.js'
import rimraf from 'rimraf'

//From Server/
import {config} from './entries.config.js'
import {plugins} from './plugins.config.js'
import {modules} from './loaders.config.js'

// Remove dist folder
rimraf('./dist--server', function (err) { if (err) { throw err; } });

module.exports = () => {

    let moduleExportArr = [];

    //run through list of tenants
    TENANTS.forEach((tenant) => {

        const entries = {}
            
        entries['react-server-components'] = ['./Features/React/Assets/Js/react-server-components.js'];

        moduleExportArr.push(
            {
                //target:'node',
                name: tenant,
                entry: entries,
                output: {
                    path: config.outputPath,
                    publicPath: config.publicPath,
                    filename: '[name].js'
                },
                module: modules(tenant),
                resolve: resolve,
                plugins: plugins(tenant),
                stats: stats,
                devtool: isProd ? "cheap-source-map" : "eval",
                devServer: devServer(tenant)
            })
    });
    return moduleExportArr

}
