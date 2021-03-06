﻿import {s3path} from '../Shared/paths.config.js'
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