import { IS_PROD } from '../Shared/env.config'
import { TENANTS } from '../Shared/tenants.config'
import { stats } from '../Shared/stats.config'
import { devServer } from '../Shared/devServer.config'
import { resolve } from '../Shared/resolve.config'

// From Client/
import { config, getEntryFiles } from './entries.config';
import { plugins } from './plugins.config';
import { modules } from './loaders.config'

module.exports = () => {

    const moduleExportArr = [];

    // run through list of tenants
    TENANTS.forEach(tenant => {

        // The order here matters or the CommonChunkPlugin
        const entries = getEntryFiles(tenant);
        const pageEntries = Object.keys(getEntryFiles(tenant));

        // That is why these entries are added after
        entries[`csn.common--${tenant}`] = ['./Features/Shared/Assets/csn.common.js'];
        entries[`csn.mediaMotive--${tenant}`] = ['./Features/Shared/Assets/Js/Modules/MediaMotive/mediaMotive.js'];
        
        moduleExportArr.push({
            target: 'web',
            name: tenant,
            entry: entries,
            output: {
                path: config.outputPath,
                publicPath: config.publicPath,
                filename: IS_PROD ? '[name]-[chunkhash].js' : '[name].js'
            },
            module: modules(tenant),
            resolve,
            plugins: plugins(tenant, pageEntries),
            stats,
            devtool: IS_PROD ? 'none' : 'eval',
            devServer: devServer(tenant),
            externals: {
                'react' : 'React',
                'react-dom' : 'ReactDOM',
                'redux' : 'Redux',
                'react-redux' : 'ReactRedux',
                'immutable' : 'Immutable',
                'ScrollMagic': 'ScrollMagic',
                'swiper': 'Swiper',
                //TODO: add redux saga
            }
        });
    });
    return moduleExportArr;
};