//---------------------------------------------------------------------------------------------------------
// List of Tenants
// Make sure associated '_settings--tenant.scss' file is added to Features\Shared\Assets\Css\Settings\

const listofTenants = [
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

const AUTenants = [
    'carsales',
    'constructionsales',
    'bikesales',
    'boatsales',
    'trucksales',
    'caravancampingsales',
    'farmmachinerysales',
    'redbook'
];

const LATAMTenants = [
    'soloautos',
    'demotores'
];

const isAuTenant = (tenant) => {
    return AUTenants.indexOf(tenant) > -1;
}

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

export const TenantConfig = {
    Tenants: process.env.TENANT ? getTenants(process.env.TENANT.trim().toLowerCase()) : listofTenants,
    isAuTenant: isAuTenant
 }
