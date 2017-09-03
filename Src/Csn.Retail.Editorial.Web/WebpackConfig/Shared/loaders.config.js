import path from 'path'
import {IS_PROD} from '../Shared/env.config.js'
import {listOfPaths} from '../Shared/paths.config.js'
import ExtractTextPlugin from 'extract-text-webpack-plugin'

// Error with sourcemaps b/c of css-loader. So inline URL to resolve issue (for development only)
const URL_LIMIT = IS_PROD ? 1 : null;

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

    let CSSLoader = IS_PROD ? prodLoaderCSSExtract(tenant) : devLoaderCSSExtract(tenant)

    return {
        noParse: IS_PROD ? /\A(?!x)x/ : /jquery|swiper|ScrollMagic|modernizr|TinyAnimate|circles/,
        rules: [
        // {
        //     enforce: 'pre',
        //     test: /\.jsx?$/,
        //     loader: "source-map-loader"
        // },
        // {
        //     enforce: 'pre',
        //     test: /\.tsx?$/,
        //     use: "source-map-loader"
        // },
        {
            test: [/\.jsx?$/, /\.es6$/],
            exclude: /(node_modules|bower_components|unitTest)/,
            use: [          
                {
                    loader: 'cache-loader',
                    options: {
                        cacheDirectory: path.resolve('.cache')
                    }
                },
                'happypack/loader?id=babel'
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
                {
                    loader: 'cache-loader',
                    options: {
                        cacheDirectory: path.resolve('.cache')
                    }
                },
                'happypack/loader?id=babelTypeScript'
            ]
        },
        {
            test: /\.css$/,
            exclude: /(node_modules|bower_components|unitTest)/,
            loaders: [...CSSLoader]
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
                        name: IS_PROD ? 'images/[name]-[hash].[ext]' : 'images/[name].[ext]'
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
                        name: IS_PROD ? 'fonts/[name]-[hash].[ext]' : 'fonts/[name].[ext]'
                    }
                }
            ]
        }
        ]
    }
}