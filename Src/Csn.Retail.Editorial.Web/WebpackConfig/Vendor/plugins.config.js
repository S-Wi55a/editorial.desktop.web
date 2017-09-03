import path from 'path'
import webpack from 'webpack'
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
]
