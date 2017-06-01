import path from 'path'
import {listOfPaths} from '../Shared/paths.config.js'

 
export const resolve = {
    extensions: ['.js','.scss'],
        alias: {
        modernizr$: path.resolve('./.modernizrrc.js'),
            'debug.addIndicators': path.resolve('node_modules', 'scrollmagic/scrollmagic/uncompressed/plugins/debug.addIndicators.js'),
    },
    //aliasFields: ["browser"],
    descriptionFiles: ['package.json', 'bower.json'],
    modules: listOfPaths
}