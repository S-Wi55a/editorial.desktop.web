import clientConfig from './Client/webpack.client.config';
import serverConfig from './Server/webpack.server.config';


module.exports = [...clientConfig(), ...serverConfig()];