'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
//---------------------------------------------------------------------------------------------------------
// List of Tenants
// Make sure associated '_settings--tenant.scss' file is added to Features\Shared\Assets\Css\Settings\

var listofTenants = exports.listofTenants = ['carsales', 'constructionsales', 'bikesales', 'carpoint', 'boatsales', 'boatpoint', 'trucksales', 'caravancampingsales', 'farmmachinerysales'];

var TENANTS = exports.TENANTS = process.env.TENANT ? [process.env.TENANT.trim()] : listofTenants;