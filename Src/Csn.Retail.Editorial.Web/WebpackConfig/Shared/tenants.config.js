//---------------------------------------------------------------------------------------------------------
// List of Tenants
// Make sure associated '_settings--tenant.scss' file is added to Features\Shared\Assets\Css\Settings\

export const listofTenants = [
    'carsales',
    'constructionsales',
    'bikesales',
    'boatsales',
    'trucksales',
    'caravancampingsales',
    'farmmachinerysales',
    'redbook',
    //'soloautos' // Comment out soloautos till its ready
];

export const AUTenants = [
    'carsales',
    'constructionsales',
    'bikesales',
    'boatsales',
    'trucksales',
    'caravancampingsales',
    'farmmachinerysales',
    'redbook'
];

export const LATAMTenants = [
    'soloautos',
    'demotores'
];

const getTenants = (tenant) => {
    switch (tenant) {
        case "au":
            return AUTenants;
        case "latam":
            return LATAMTenants;

        default:
            return [tenant];
    }
}

export const TENANTS = process.env.TENANT ? getTenants(process.env.TENANT.trim().toLowerCase()) : listofTenants;

export const tenantSetting = {
    carsales: {
        adSource: ['./Features/Shared/Assets/Js/Modules/MediaMotive/mediaMotive.js']
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
}
