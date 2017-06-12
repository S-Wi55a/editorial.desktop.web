'use strict';

var _webpackClientConfig = require('./Client/webpack.client.config.js');

var _webpackClientConfig2 = _interopRequireDefault(_webpackClientConfig);

var _webpackServerConfig = require('./Server/webpack.server.config.js');

var _webpackServerConfig2 = _interopRequireDefault(_webpackServerConfig);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _toConsumableArray(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } else { return Array.from(arr); } }

module.exports = [].concat(_toConsumableArray((0, _webpackServerConfig2.default)()), _toConsumableArray((0, _webpackClientConfig2.default)()));