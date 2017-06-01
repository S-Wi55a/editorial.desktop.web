﻿import {s3path} from '../Shared/paths.config.js'
import {isProd} from '../Shared/env.config.js'

var glob = require('glob'),
    path = require('path');


export const config = {
    entryPointMatch: './Features/**/*-page.js', // anything ends with -page.js
    outputPath: path.resolve(s3path),
    publicPath: isProd ? './' : s3path
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



//entries['vendor' + '--' + tenant] = ['./Features/Shared/Assets/Js/vendor.js'];
//entries['csn.base' + '--' + tenant] = ['./Features/Shared/Assets/csn.base.js'];
//entries['csn.mm' + '--' + tenant] = ['./Features/Shared/Assets/Js/Modules/MediaMotive/mm.js'];

//entries['react-server-components' + '--' + tenant] = ['Features/ReactServerRender/Assets/Js/react-server-components.js'];