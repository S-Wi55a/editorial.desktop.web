import os from 'os'
import path from 'path'
import webpack from 'webpack'
import HappyPack from 'happypack'
import AssetsPlugin from 'assets-webpack-plugin'

const assetsPluginInstance = new AssetsPlugin({
    filename: 'webpack.assets.json',
    path: path.resolve('./'),
    prettyPrint: true,
    fullPath: false,
});

export const plugins = [
    assetsPluginInstance,  
    new webpack.NamedModulesPlugin(),
    // new HappyPack({
    //     // loaders is the only required parameter:
    //     id: 'babel',
    //     loaders: ['babel-loader?cacheDirectory=true'],
    //     threadPool: HappyPack.ThreadPool({ size: os.cpus().length >= 4 ? 3 : os.cpus().length - 1})        
    // })
]
