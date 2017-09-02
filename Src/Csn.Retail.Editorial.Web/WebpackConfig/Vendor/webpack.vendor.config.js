import rimraf from 'rimraf'
import path from 'path'

import { isProd } from '../Shared/env.config'
import { stats } from '../Shared/stats.config'
import { resolve } from '../Shared/resolve.config'
import { devServer } from '../Shared/devServer.config'


// From Vendor/
import { plugins } from './plugins.config'
import { modules } from './loaders.config'

import { config, getEntryFiles } from './entries.config';

module.exports = [{
        target: 'web',
        name: 'vendor',
        entry: getEntryFiles(),
        output: {
            path: config.outputPath,
            publicPath: config.publicPath,
            filename: isProd ? '[name]-[chunkhash].js' : '[name].js'
        },
        module: modules,
        resolve,
        plugins,
        stats,
        devtool: isProd ? 'cheap-source-map' : 'eval',
        devServer: devServer('carsales') //TODO: set default value
        
    }]

