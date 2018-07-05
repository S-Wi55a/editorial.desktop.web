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
    'soloautos'
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
    'soloautos'
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
//export const TENANTS = process.env.TENANT ? [process.env.TENANT.trim().toLowerCase()] : listofTenants;
