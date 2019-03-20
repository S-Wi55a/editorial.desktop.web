import { IS_PROD } from '../Shared/env.config'
import { TenantConfig } from '../Shared/tenants.config'
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
    TenantConfig.Tenants.forEach(tenant => {

        // The order here matters or the CommonChunkPlugin
        const entries = getEntryFiles(tenant);
        const pageEntries = Object.keys(getEntryFiles(tenant));

        // That is why these entries are added after
        entries[`csn.common--${tenant}`] = './Features/Shared/Assets/csn.common.js';

        // Use a config to switch ad source when needed
        if (TenantConfig.isAuTenant(tenant)) {
            entries[`csn.displayAds--${tenant}`] = './Features/Shared/Assets/Js/Modules/MediaMotive/mediaMotive.js';
        } else {
            entries[`csn.displayAds--${tenant}`] = './Features/Shared/Assets/Js/Modules/GoogleAd/googleAd.js';
        }

        moduleExportArr.push({
            target: 'web',
            name: tenant,
            mode: IS_PROD ? 'production' : 'development',
            entry: entries,
            output: {
                path: config.outputPath,
                publicPath: config.publicPath,
                filename: IS_PROD ? '[name]-[chunkhash].js' : '[name].js'
            },
            module: modules(tenant),
            plugins: plugins(tenant, pageEntries),
            optimization: {
                splitChunks: {
                    chunks: 'async',
                    minSize: 3000,
                    minChunks: 1,
                    maxAsyncRequests: 5,
                    maxInitialRequests: 3,
                    automaticNameDelimiter: '~',
                    name: true,
                    cacheGroups: {
                        vendors: {
                            test: /[\\/]node_modules[\\/]/,
                            name: 'vendor',
                            chunks: 'async',
                            reuseExistingChunk: true
                        }
                    }
                },
                runtimeChunk: {
                    name: 'csn.manifest' + '--' + tenant,
                },
            },
            resolve,
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