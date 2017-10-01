﻿import path from 'path'
import { IS_PROD } from '../Shared/env.config.js'
import { listOfPaths } from '../Shared/paths.config.js'
import ExtractTextPlugin from 'extract-text-webpack-plugin'

// Error with sourcemaps b/c of css-loader. So inline URL to resolve issue (for development only)
const URL_LIMIT = IS_PROD ? 1 : null;

const loaders = (tenant) => ([{
        loader: 'cache-loader',
        options: {
            cacheDirectory: path.resolve('.cache')
        }
    },
    {
        loader: 'css-loader',
        options: {
            sourceMap: IS_PROD ? false : true,
            minimize: IS_PROD ? true : false,
            //modules: true
        }
    },
    'postcss-loader?sourceMap',
    {
        loader: 'resolve-url-loader',
        options: {
            sourceMap: IS_PROD ? false : true,
            keepQuery: true
        }
    },
    {
        loader: 'sass-loader',
        options: {
            includePaths: listOfPaths,
            sourceMap: true,
            data: '@import "Css/Tenants/' + tenant.charAt(0).toUpperCase() + tenant.slice(1) + '/' + tenant + '.scss";'
        }
    }
])

export const prodLoaderCSSExtract = (tenant) => (ExtractTextPlugin.extract({
    fallback: 'style-loader',
    use: loaders(tenant)
}))

export const devLoaderCSSExtract = (tenant) => (['style-loader'].concat(loaders(tenant)))

export const modules = (tenant) => {

    let CSSLoader = IS_PROD ? prodLoaderCSSExtract(tenant) : devLoaderCSSExtract(tenant)

    return {
        rules: [{
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
                            transpileOnly: IS_PROD ? false : true, // Performance reasons - https://github.com/TypeStrong/ts-loader
                            visualStudioErrorFormat: true,
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
                test: /\.(gif|png|jpe?g|svg)$/i,
                exclude: [/(node_modules|bower_components|unitTest)/, /Fonts/],
                use: [{
                        loader: 'url-loader',
                        options: {
                            limit: URL_LIMIT,
                            name: IS_PROD ? 'images/[name]-[hash].[ext]' : 'images/[name].[ext]'
                        }
                    },
                    {
                        loader: 'image-webpack-loader',
                        options: {
                            gifsicle: {
                                interlaced: false,
                            },
                            optipng: {
                                optimizationLevel: 7,
                            },
                            pngquant: {
                                quality: '65-90',
                                speed: 4
                            },
                            mozjpeg: {
                                progressive: true,
                                quality: 65
                            },
                            // Specifying webp here will create a WEBP version of your JPG/PNG images
                            webp: {
                                quality: 75
                            }
                        }
                    }
                ]
            },
            {
                test: /\.(eot|ttf|woff|woff2|svg)$/,
                exclude: /Images/,
                use: [{
                    loader: 'url-loader',
                    options: {
                        limit: URL_LIMIT,
                        name: IS_PROD ? 'fonts/[name]-[hash].[ext]' : 'fonts/[name].[ext]'
                    }
                }]
            }
        ]
    }
}