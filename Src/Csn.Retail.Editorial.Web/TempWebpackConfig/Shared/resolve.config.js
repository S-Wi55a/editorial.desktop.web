'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.resolve = undefined;

var _path = require('path');

var _path2 = _interopRequireDefault(_path);

var _pathsConfig = require('../Shared/paths.config.js');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var resolve = exports.resolve = {
    extensions: ['.js', '.scss'],
    alias: {
        modernizr$: _path2.default.resolve('./.modernizrrc.js'),
        'debug.addIndicators': _path2.default.resolve('node_modules', 'scrollmagic/scrollmagic/uncompressed/plugins/debug.addIndicators.js')
    },
    //aliasFields: ["browser"],
    descriptionFiles: ['package.json', 'bower.json'],
    modules: _pathsConfig.listOfPaths
};