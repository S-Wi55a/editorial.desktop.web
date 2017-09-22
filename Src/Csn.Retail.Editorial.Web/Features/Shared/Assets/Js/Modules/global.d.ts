declare var require: {
    <T>(path: string): T;
    (paths: string[], callback: (...modules: any[]) => void): void;
    ensure: (paths: string[], callback: (require: <T>(path: string) => T) => void) => void;
};

declare module 'Endpoints/endpoints'
declare module 'ReactReduxUI'
declare module 'iNav/Components/iNavMenuHeader'
declare module 'global-object'

declare var SERVER: boolean