import path from 'path'
import webpack from 'webpack'
import AssetsPlugin from 'assets-webpack-plugin'
import {IS_PROD} from '../Shared/env.config.js'


const assetsPluginInstance = new AssetsPlugin({
    filename: 'webpack.assets.json',
    path: path.resolve('./'),
    prettyPrint: true,
    fullPath: false,
});

export const plugins = () => {
    
        let pluginsArr = [
            assetsPluginInstance,              
            new webpack.NamedModulesPlugin(),
            new webpack.DefinePlugin({
                'process.env.NODE_ENV': IS_PROD ? '"production"': '"development"', //TODO add to shared / Correct the logic
            }),
        ]
    
        if (IS_PROD) {
            pluginsArr.push(
                new webpack.optimize.UglifyJsPlugin({
                    compress: {
                      warnings: false,
                      // Disabled because of an issue with Uglify breaking seemingly valid code:
                      // https://github.com/facebookincubator/create-react-app/issues/2376
                      // Pending further investigation:
                      // https://github.com/mishoo/UglifyJS2/issues/2011
                      comparisons: false,
                    },
                    output: {
                      comments: false,
                      // Turned on because emoji and regex is not minified properly using default
                      // https://github.com/facebookincubator/create-react-app/issues/2488
                      ascii_only: true,
                    },
                    sourceMap: false,
                  })
            )
        }   
        return pluginsArr
    }