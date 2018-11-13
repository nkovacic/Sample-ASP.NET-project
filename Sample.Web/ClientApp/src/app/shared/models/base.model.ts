export enum SortDirection {
    ascending = 'asc',
    descending = 'desc'
}

export interface IKeyValueCheckbox<T, E> extends IKeyValue<T, E> {
    selected?: boolean;
}

export interface ITuple<T, E, F> extends IKeyValue<T, E> {
    title: F 
}

export interface IKeyValue<T, E> {
    key: T;
    value: E;
}

export interface ISimpleEntity {
    id: string;
    title: string;
}

export interface ISize {
    height?: number;
    width?: number;
}