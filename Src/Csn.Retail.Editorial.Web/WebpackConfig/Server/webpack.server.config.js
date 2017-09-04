import {IS_PROD} from '../Shared/env.config.js'
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

    const entries = {}
        
    entries['react-server-components'] = ['./Features/React/Assets/Js/react-server-entry.js'];

    return [{
        target:'node',
        name: 'server',
        entry: entries,
        output: {
            path: config.outputPath,
            publicPath: config.publicPath,
            filename: '[name].js'
        },
        module: modules(),
        resolve: resolve,
        plugins: plugins(),
        stats: stats,
        devtool: IS_PROD ? "cheap-source-map" : "eval",
        devServer: devServer('carsales') //TODO: fix
    }]
}
