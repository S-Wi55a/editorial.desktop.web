﻿//---------------------------------------------------------------------------------------------------------
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
