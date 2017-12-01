import path from 'path';

export const rootRelativePath = path.resolve('./');
//---------------------------------------------------------------------------------------------
// list of path to search for files
export const listOfPaths = [
    'node_modules',
    'bower_components',
    'Features/Shared/Assets/Js/Modules/',
    'Features/Details/Assets/Js/Modules/',
    'Features/Listings/Assets/Js/Modules/',
    'Features/Landing/Assets/Js/Modules/',
    'Features/SiteNav/Assets/Js/Modules/',
    'Features/Errors/Assets/Js/Modules/',
    'Features/Shared/Assets',
    'Features/Details/Assets',
    'Features/Listings/Assets',
    'Features/Landing/Assets',
    'Features/SiteNav/Assets',
    'Features/Errors/Assets',
    'Infrastructure/React/Assets/Js/'
];

// list of path to search for files
export const s3path = 'dist/retail/editorial/'; // transfer to s3 is handles with gulp