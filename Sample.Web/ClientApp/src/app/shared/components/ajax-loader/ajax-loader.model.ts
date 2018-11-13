export enum AjaxLoaderSize {
    small = 'sm',
    medium = 'md',
    large = 'lg'
}

export interface IAjaxLoaderOptions {
    inline?: boolean;
    fixed?: boolean;
    transparent?: boolean;
    size?: AjaxLoaderSize
}