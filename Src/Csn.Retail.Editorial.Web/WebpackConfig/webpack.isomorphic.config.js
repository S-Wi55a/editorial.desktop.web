import clientConfig from './Client/webpack.client.config';
import serverConfig from './Server/webpack.server.config';
import vendorConfig from './Vendor/webpack.vendor.config';


module.exports = [...clientConfig(), ...serverConfig(), ...vendorConfig];