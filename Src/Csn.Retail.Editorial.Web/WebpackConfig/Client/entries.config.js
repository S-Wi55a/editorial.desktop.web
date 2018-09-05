import {s3path} from '../Shared/paths.config.js'
import {IS_PROD} from '../Shared/env.config.js'
import glob from 'glob'
import path from 'path'

export const config = {
    entryPointMatch: './Features/**/*-page.+(js|ts)', // anything ends with -page.js
    outputPath: path.resolve(s3path),
    publicPath: IS_PROD ? './' : s3path
}

export function getEntryFiles(tenant) {
    if (!tenant) {
        tenant = '';
    }
    let entries = {};

    let matchedFiles = glob.sync(config.entryPointMatch);

    let length = matchedFiles.length;

    for(let i = 0; i < length; i++){
        let filePath = matchedFiles[i];
        let ext = path.extname(filePath);
        let filename = path.basename(filePath, ext);
        entries[filename + '--' + tenant] = filePath;
    }
    return entries;
}

// TODO: Make this configurable.
export function getTenantEntryFiles(tenant, entries) {
    let tenantEntries = {
        carsales: {
            'csn.displayAds--' : ['./Features/Shared/Assets/Js/Modules/MediaMotive/mediaMotive.js']
        },
        constructionsales: {
            adSource: ['./Features/Shared/Assets/Js/Modules/MediaMotive/mediaMotive.js']
        },
        bikesales: {
            adSource: ['./Features/Shared/Assets/Js/Modules/MediaMotive/mediaMotive.js']
        },
        boatsales: {
            adSource: ['./Features/Shared/Assets/Js/Modules/MediaMotive/mediaMotive.js']
        },
        trucksales: {
            adSource: ['./Features/Shared/Assets/Js/Modules/MediaMotive/mediaMotive.js']
        },
        caravancampingsales: {
            adSource: ['./Features/Shared/Assets/Js/Modules/MediaMotive/mediaMotive.js']
        },
        farmmachinerysales: {
            adSource: ['./Features/Shared/Assets/Js/Modules/MediaMotive/mediaMotive.js']
        },
        redbook: {
            adSource: ['./Features/Shared/Assets/Js/Modules/MediaMotive/mediaMotive.js']
        },
        soloautos: {
            adSource: ['./Features/Shared/Assets/Js/Modules/GoogleAds/googleAds.js']
        }
    };
}