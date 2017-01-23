var devConfig = require('./webpack.config.js'),
    ExtractTextPlugin = require('extract-text-webpack-plugin'),
    S3Plugin = require('webpack-s3-sync-plugin');

var argv = require('yargs').argv;


var awsAccessKey = (argv.awsAccessKey || ''),
    awsSecret = (argv.awsSecret || '');

var prodLoaders = [
           {
               test: /\.ts$/,
               exclude: /node_modules/,
               loaders: ['ts-loader']
           },
           {
               test: [/\.js$/, /\.es6$/],
               exclude: /node_modules/,
               loader: 'babel-loader',
               query: {
                   presets: ['es2015']
               }
           },
            {
                test: [/\.css$/],
                exclude: /node_modules/,
                loader: ExtractTextPlugin.extract('style-loader', 'css-loader!clean-css!autoprefixer-loader')
            },
            {
                test: /\.scss$/,
                exclude: [/(node_modules|bower_components|unitTest)/, /fonts\.scss/],
                loader: ExtractTextPlugin.extract('style-loader', 'css-loader!clean-css!autoprefixer-loader!resolve-url-loader!sass-loader?sourceMap')
            },
           {
               test: /fonts\.scss$/,
               exclude: /node_modules/,
               loader: ExtractTextPlugin.extract('style-loader', 'css-loader?sourceMap!autoprefixer-loader!resolve-url-loader!sass-loader?sourceMap')
           },
           {
               test: /.*\.(gif|png|jpe?g|svg)$/i,
               exclude: [/(node_modules|bower_components|unitTest)/, /fonts/],
               loaders: [
                   'file?hash=sha512&digest=hex&name=images/[name]-[hash].[ext]',
                   'image-webpack'
               ]
           },
           {
               test: /\.(eot|svg|ttf|woff|woff2)$/,
               exclude: /(images|img)/,
               loader: 'file?name=fonts/[name]-[hash].[ext]'
           },
           {
               test: require.resolve('jquery'),
               loader: 'expose?jQuery!expose?$'
           }

]



var s3 = new S3Plugin({
    // s3Options are required
    s3Options: {
        accessKeyId: awsAccessKey,
        secretAccessKey: awsSecret,
        region: 'us-east-1'
    },
    s3UploadOptions: {
        Bucket: 'carsales-test-editorial'
    },
    basePath: (argv.basePath || 'dist') //match with directory in bucket
});

if (awsAccessKey !== '') {
    devConfig.plugins.push(s3);
}

// Replace dev laoders with pro loaders
devConfig.module.loaders = prodLoaders


module.exports = devConfig;