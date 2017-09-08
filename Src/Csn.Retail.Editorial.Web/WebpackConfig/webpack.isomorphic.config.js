import clientConfig from './Client/webpack.client.config';
import serverConfig from './Server/webpack.server.config';
import vendorConfig from './Vendor/webpack.vendor.config';
import os from 'os'
console.log('Cores: ' + os.cpus().length)

module.exports = [...clientConfig(), ...serverConfig(), ...vendorConfig];