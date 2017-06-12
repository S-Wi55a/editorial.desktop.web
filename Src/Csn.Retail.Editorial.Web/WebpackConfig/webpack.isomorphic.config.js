import clientConfig from './Client/webpack.client.config.js'
import serverConfig from './Server/webpack.server.config.js'

module.exports = [...serverConfig(),...clientConfig()]