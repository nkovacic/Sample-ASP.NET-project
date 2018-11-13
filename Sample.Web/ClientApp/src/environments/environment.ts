// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
    apiPrefix: 'api',
    apiRoot: 'http://localhost:53215',
    envName: 'local',
    google: {
        apiKey: 'AIzaSyAd-oVR3A_7V9eCO5G1rfzCyUE70DmK9bU'
    },
    production: false,
    storageContainer: 'pictures',
    storageName: 'realestatelocal'
};
