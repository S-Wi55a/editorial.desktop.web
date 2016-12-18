var wpStrip = require('strip-loader'),
    devConfig = require('./webpack.config.js'),
    S3Plugin = require('webpack-s3-sync-plugin');


var awsAccessKey = '',
    awsSecret = '';

var stripLoader = {
    test: [/\.js$/,/\.es6$/],
    exclude: /node_modules/,
    loader: wpStrip.loader('console.log')
}


//TODO: Setup prod loaders

var prodLoaders = [
{
    test: [/\.css$/],
    exclude: /node_modules/,
    loaders: ExtractTextPlugin.extract('style-loader', 'css-loader?sourceMap', 'clean-css', 'autoprefixer-loader')
},
{
test: /\.scss$/,
exclude: /node_modules/,
loaders: ExtractTextPlugin.extract('style-loader', 'css-loader?sourceMap', 'clean-css', 'autoprefixer-loader', 'sass-loader?sourceMap')
}]

var s3 = new S3Plugin({
    // s3Options are required
    s3Options: {
        accessKeyId: awsAccessKey,
        secretAccessKey: awsSecret,
        region: 'ap-southeast-2'
    },
    s3UploadOptions: {
        Bucket: 'carsales-web'
    }
});

devConfig.module.loaders.push(stripLoader);

if (awsAccessKey !== '') {
    devConfig.plugins.push(s3);
}

module.exports = devConfig;