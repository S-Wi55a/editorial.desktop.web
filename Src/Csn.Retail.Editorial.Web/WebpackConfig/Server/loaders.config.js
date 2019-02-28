import { IS_PROD } from '../Shared/env.config.js'

export const modules = () => {

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
                            logLevel: 'warn'
                        }
                    }
                ]

            }
        ]
    }
}