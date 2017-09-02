import path from 'path'
import {isProd} from '../Shared/env.config.js'
import {listOfPaths} from '../Shared/paths.config.js'
import ExtractTextPlugin from 'extract-text-webpack-plugin'

// Error with sourcemaps b/c of css-loader. So inline URL to resolve issue (for development only)
const URL_LIMIT = isProd ? 1 : null;

const loaders = (tenant) => ([
    {
        loader: 'cache-loader',
        options: {
            cacheDirectory: path.resolve('.cache')
            }
    },
    {
        loader: 'css-loader',
        options: {
            sourceMap: isProd ? false : true,
            minimize: isProd ? true : false,
            //modules: true
        }
    },
    'postcss-loader?sourceMap',
    {
        loader: 'resolve-url-loader',
        options: {
            sourceMap: isProd ? false : true,
            keepQuery: true
        }
    },
    {
        loader: 'sass-loader',
        options: {
            includePaths: listOfPaths,
            sourceMap: true,
            data: '@import "Css/Tenants/' + tenant + '/' + tenant +'.scss";'
        }
    }
])

export const prodLoaderCSSExtract = (tenant) => (ExtractTextPlugin.extract({
    fallback: 'style-loader',
    use: loaders(tenant)
}))

export const devLoaderCSSExtract = (tenant) => (['style-loader'].concat(loaders(tenant)))

export const modules = (tenant) => {

    let CSSLoader = isProd ? prodLoaderCSSExtract(tenant) : devLoaderCSSExtract(tenant)

    return {
        rules: [
        {
            test: [/\.jsx?$/, /\.es6$/],
            exclude: /(node_modules|bower_components|unitTest)/,
            use: [
                'babel-loader?cacheDirectory=true'
            ]
        },
        {
            test: /\.modernizrrc.js$/,
            exclude: /(node_modules|bower_components|unitTest)/,
            use: 'modernizr-loader'
        },
        {
            test: /\.tsx?$/,
            exclude: /(node_modules|bower_components|unitTest)/,
            use: [
                'babel-loader?cacheDirectory=true',
                {
                    loader: 'ts-loader', 
                    options: {
                    transpileOnly: isProd ? false : true,
                      //visualStudioErrorFormat: true,
                      logLevel: 'warn'
                    } 
                }
            ]
            
        },
        {
            test: /\.css$/,
            exclude: /(node_modules|bower_components|unitTest)/,
            use: [...CSSLoader]
        },
        {
            test: /\.scss$/,
            exclude: [/(node_modules|bower_components|unitTest)/],
            use: [...CSSLoader]                  
        },
        {
            test: /.*\.(gif|png|jpe?g|svg)$/i,
            exclude: [/(node_modules|bower_components|unitTest)/, /fonts/],
            use: [
                {
                    loader: 'url-loader',
                    options: {
                        limit: URL_LIMIT,
                        name: isProd ? 'images/[name]-[hash].[ext]' : 'images/[name].[ext]'
                    }
                },
                {
                    loader: 'image-webpack-loader',
                    options: {
                        progressive: true,
                        optipng: {
                            optimizationLevel: 7,
                        },
                        gifsicle: {
                            interlaced: false,
                        },
                        pngquant: {
                            quality: '65-90',
                            speed: 4
                        }
                    }
                }
            ]
        },
        {
            test: /\.(eot|svg|ttf|woff|woff2)$/,
            exclude: /(images|img)/,
            use: [
                {
                    loader: 'url-loader',
                    options: {
                        limit: URL_LIMIT,
                        name: isProd ? 'fonts/[name]-[hash].[ext]' : 'fonts/[name].[ext]'
                    }
                }
            ]
        }
        ]
    }
}