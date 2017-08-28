import {isProd} from '../Shared/env.config.js'
import {listOfPaths} from '../Shared/paths.config.js'
import path from 'path'
import ExtractTextPlugin from 'extract-text-webpack-plugin'

// Error with sourcemaps b/c of css-loader. So inline URL to resolve issue (for development only)
const URL_LIMIT = isProd ? 1 : null;

const loaders = (tenant) => ([
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

    return {
        noParse: isProd ? /\A(?!x)x/ : /jquery|swiper|ScrollMagic|modernizr|TinyAnimate|circles/,
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
            loaders: ['happypack/loader?id=babel']
        },
        {
            test: /\.modernizrrc.js$/,
            exclude: /(node_modules|bower_components|unitTest)/,
            loader: "modernizr-loader"
        },
        {
            test: /\.tsx?$/,
            exclude: /(node_modules|bower_components|unitTest)/,
            loaders: ['happypack/loader?id=babelTypeScript']
        },
        {
            test: /\.css$/,
            exclude: /(node_modules|bower_components|unitTest)/,
            use: isProd ? prodLoaderCSSExtract(tenant) : devLoaderCSSExtract(tenant)

        },
        {
            test: /\.scss$/,
            exclude: [/(node_modules|bower_components|unitTest)/],
            use: isProd ? prodLoaderCSSExtract(tenant) : devLoaderCSSExtract(tenant)
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