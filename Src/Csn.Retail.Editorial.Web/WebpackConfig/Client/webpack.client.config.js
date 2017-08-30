import rimraf from 'rimraf';

import { isProd } from '../Shared/env.config';
import { TENANTS } from '../Shared/tenants.config';
import { stats } from '../Shared/stats.config';
import { devServer } from '../Shared/devServer.config';
import { resolve } from '../Shared/resolve.config';

import os from 'os'
console.log('Cores: ' + os.cpus().length)

// From Client/
import { config, getEntryFiles } from './entries.config';
import { plugins } from './plugins.config';
import { modules } from './loaders.config';
import { externals } from './externals.config';

// Remove dist folder
rimraf('./dist', err => { if (err) { throw err; } });

module.exports = () => {

    const moduleExportArr = [];

    // run through list of tenants
    TENANTS.forEach(tenant => {

        // The order here matters or the CommonChunkPlugin
        const entries = getEntryFiles(tenant);
        const pageEntries = Object.keys(getEntryFiles(tenant));

        // That is why these entries are added after
        entries[`csn.vendor--${tenant}`] = ['./Features/Shared/Assets/Js/csn.vendor.js'];
        entries[`csn.common--${tenant}`] = ['./Features/Shared/Assets/csn.common.js'];

        moduleExportArr.push({
            target: 'web',
            name: tenant,
            entry: entries,
            output: {
                path: config.outputPath,
                publicPath: config.publicPath,
                filename: isProd ? '[name]-[chunkhash].js' : '[name].js'
            },
            module: modules(tenant),
            resolve,
            externals,
            plugins: plugins(tenant, pageEntries),
            stats,
            devtool: isProd ? 'cheap-source-map' : 'eval',
            devServer: devServer(tenant)
        });
    });
    return moduleExportArr;
};