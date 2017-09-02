import {s3path} from '../Shared/paths.config.js'
import {isProd} from '../Shared/env.config.js'
import glob from 'glob'
import path from 'path'

export const config = {
    entryPointMatch: './Features/**/*.vendor.js', // anything ends with -page.js
    outputPath: path.resolve(s3path),
    publicPath: isProd ? './' : s3path
}

export function getEntryFiles() {

    let entries = {};

    let matchedFiles = glob.sync(config.entryPointMatch);

    let length = matchedFiles.length;

    for(let i = 0; i < length; i++){
        let filePath = matchedFiles[i];
        let ext = path.extname(filePath);
        let filename = path.basename(filePath, ext);
        entries[filename] = filePath;
    }
    return entries;
}

