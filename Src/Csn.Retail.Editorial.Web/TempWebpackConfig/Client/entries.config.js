'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.config = undefined;
exports.getEntryFiles = getEntryFiles;

var _pathsConfig = require('../Shared/paths.config.js');

var _envConfig = require('../Shared/env.config.js');

var _glob = require('glob');

var _glob2 = _interopRequireDefault(_glob);

var _path = require('path');

var _path2 = _interopRequireDefault(_path);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var config = exports.config = {
    entryPointMatch: './Features/**/*-page.js', // anything ends with -page.js
    outputPath: _path2.default.resolve(_pathsConfig.s3path),
    publicPath: _envConfig.isProd ? './' : _pathsConfig.s3path
};

function getEntryFiles(tenant) {
    if (!tenant) {
        tenant = '';
    }
    var entries = {};

    var matchedFiles = _glob2.default.sync(config.entryPointMatch);

    var length = matchedFiles.length;

    for (var i = 0; i < length; i++) {
        var filePath = matchedFiles[i];
        var ext = _path2.default.extname(filePath);
        var filename = _path2.default.basename(filePath, ext);
        entries[filename + '--' + tenant] = filePath;
    }
    return entries;
}

//entries['vendor' + '--' + tenant] = ['./Features/Shared/Assets/Js/vendor.js'];
//entries['csn.base' + '--' + tenant] = ['./Features/Shared/Assets/csn.base.js'];
//entries['csn.mm' + '--' + tenant] = ['./Features/Shared/Assets/Js/Modules/MediaMotive/mm.js'];

//entries['react-server-components' + '--' + tenant] = ['Features/ReactServerRender/Assets/Js/react-server-components.js'];