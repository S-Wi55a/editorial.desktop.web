//---------------------------------------------------------------------------------------------------------
// List of Tenants
// Make sure associated '_settings--tenant.scss' file is added to Features\Shared\Assets\Css\Settings\

export const listofTenants = [
    'carsales',
    'constructionsales',
    'bikesales',
    //'carpoint',
    'boatsales',
    'boatpoint',
    'trucksales',
    'caravancampingsales',
    'farmmachinerysales',
];


export const TENANTS = process.env.TENANT ? [process.env.TENANT.trim().toLowerCase()] : listofTenants;
