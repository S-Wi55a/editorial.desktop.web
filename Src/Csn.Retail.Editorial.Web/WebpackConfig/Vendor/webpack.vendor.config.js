import { IS_PROD } from '../Shared/env.config'
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
            filename: IS_PROD ? '[name]-[chunkhash].js' : '[name].js'
        },
        module: modules,
        resolve,
        plugins: plugins(),
        stats,
        devtool: IS_PROD ? 'none' : 'eval',
        devServer: devServer('carsales') //TODO: set default value
        
    }]

