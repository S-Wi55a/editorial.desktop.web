'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.s3path = exports.listOfPaths = exports.rootRelativePath = undefined;

var _path = require('path');

var _path2 = _interopRequireDefault(_path);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var rootRelativePath = exports.rootRelativePath = _path2.default.resolve('');

//---------------------------------------------------------------------------------------------------------
// list of path to search for files
var listOfPaths = exports.listOfPaths = [_path2.default.resolve(__dirname, './'), 'node_modules', 'bower_components', 'Features/Shared/Assets', 'Features/Details/Assets', 'Features/SiteNav/Assets', 'Features/Errors/Assets', 'Features'];

// list of path to search for files
var s3path = exports.s3path = 'dist/retail/editorial/'; //transfer to s3 is handles with gulp