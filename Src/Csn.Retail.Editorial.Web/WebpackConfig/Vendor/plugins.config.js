import path from 'path'
import webpack from 'webpack'
import AssetsPlugin from 'assets-webpack-plugin'
import {IS_PROD} from '../Shared/env.config.js'

export const plugins = () => {
    let pluginsArr = [
        new AssetsPlugin({
          filename: 'webpack.assets.json',
          path: path.resolve('./'),
          prettyPrint: true,
          fullPath: false,
          update:true
      })
    ] 
    return pluginsArr
}