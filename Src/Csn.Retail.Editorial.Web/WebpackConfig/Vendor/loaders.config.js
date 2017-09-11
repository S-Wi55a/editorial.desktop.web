export const modules = {
        rules: [
        {
            test: [/\.jsx?$/, /\.es6$/],
            exclude: /(node_modules|bower_components|unitTest)/,
            use: [
                'babel-loader?cacheDirectory=true'
            ]
        }
    ]
}