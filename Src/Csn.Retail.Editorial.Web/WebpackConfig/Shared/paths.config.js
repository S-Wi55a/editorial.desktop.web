import path from 'path';

export const rootRelativePath = path.resolve('');

//---------------------------------------------------------------------------------------------
// list of path to search for files
export const listOfPaths = [
    path.resolve('./'),
    'node_modules',
    'bower_components',
    'Features/Shared/Assets',
    'Features/Details/Assets',
    'Features/Listings/Assets',
    'Features/SiteNav/Assets',
    'Features/Errors/Assets',
    'Features'    
];

// list of path to search for files
export const s3path = 'dist/retail/editorial/'; // transfer to s3 is handles with gulp