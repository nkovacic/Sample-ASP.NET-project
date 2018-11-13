import { Injectable } from '@angular/core';
import { environment } from '@environment';
//import { Headers, Http, Response, URLSearchParams } from '@angular/http';
import { HttpClient, HttpHeaders, HttpResponse, HttpRequest  } from '@angular/common/http';
import { Observable, pipe, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { LamaModuleService } from '@shared/services/lama-module.service';
import { SortDirection } from '@shared/models/base.model';

export enum Method {
    del = <any>'DELETE',
    get = <any>'GET',
    post = <any>'POST',
    put = <any>'PUT'
}

export interface IApiResponseError {
    message: string;
    messageKey: string;
    status: number;
}

export interface IFacet {
    name: string;
    values: Array<IFacetValue>;
}

export interface IFacetValue {
    count: number;
    value: string;
}

export interface IGetParameters {
    fields?: string;
    limit?: number;
    page?: number;
    pagination?: boolean;
    order?: string | SortDirection;
    orderBy?: string;
    query?: string | Array<string>;
    queryBy?: string | Array<string>;
}

export interface IParameters {
    data?: any;
    parameters?: IGetParameters
}

export interface IActionFileParameters extends IActionParameters {
    file: File | Array<File>;
    fileFormDataName: string | Array<string>;
}

export interface IActionParameters extends IParameters {
    action: string;
    method?: Method;
}

export interface IFindOneParameters extends IParameters {
    id: string;
    association?: string;
    parameters?: IGetParameters;
}

export interface IMethodOptions {
    handleServerErrors: boolean;
}

export interface IPaginationViewModel<T> {
    items: Array<T>;
    pagination: IPaginationStateViewModel;
}

export interface IPaginationStateViewModel {
    count: number;
    limit: number;
    page: number;
}

interface ILamaDataService {
    repository(model: string): IRepository;
}

interface IRepository {
    action(options: IActionParameters): Observable<any>;
    association(id: string, model: string): IRepository;
    find(parameters?: IGetParameters, methodOptions?: IMethodOptions): Observable<any>;
    findOne(options: IFindOneParameters, methodOptions?: IMethodOptions): Observable<any>;
    create(model): Observable<any>;
    update(model: any, methodOptions?: IMethodOptions): Observable<any>;
    url(url: string): IRepository;
    destroy(id): Observable<any>;
    version(version: string): IRepository;
}

class Repository implements IRepository {
    private _url: string;

    constructor(private http: HttpClient, private apiVersion: string, modelOrUrl: string) {
        this._url = modelOrUrl;
    }

    private formatErrors() {

    }
    private getFileUploadHeaders() {
        
        const headersConfig = {
            //'Content-Type': 'multipart/form-data',
            'Accept': 'application/json'
        };

        return new HttpHeaders(headersConfig);
    }
    private getHeaders() {
        const headersConfig = {
            'Content-Type': 'application/json; charset=UTF-8',
            'Accept': 'application/json'
        };

        return new HttpHeaders(headersConfig);
    }
    private getUrl() {
        let urlComponents = [environment.apiRoot, environment.apiPrefix, this.apiVersion, this._url],
            url = '';

        urlComponents.forEach((urlComponent) => {
            url += urlComponent;

            if (!url.endsWith('/')) {
                url += '/';
            }
        });

        return url.slice(0, -1);
    }

    public action(options: IActionParameters) {
        if (options.action) {
            this.url(options.action);
        }

        options.method = options.method || Method.get;

        return this.http
            .request(options.method.toString(), this.getUrl(), {
                body: options.data,
                headers: this.getHeaders(),
                params: <any>options.parameters
            });
    }

    public actionFile(options: IActionFileParameters) {
        if (options.action) {
            this.url(options.action);
        }

        const formData: FormData = new FormData();

        formData.append(options.fileFormDataName as string, options.file as File);

        return this.http
            .request(options.method.toString(), this.getUrl(), {
                body: formData,
                headers: this.getFileUploadHeaders(),
                params: <any>options.parameters
            });
    }

    public association(id: string, model: string): IRepository {
        throw new Error("Method not implemented.");
    }
    public find(parameters?: IGetParameters, methodOptions?: IMethodOptions): Observable<any> {
        return this.action({
            action: '',
            parameters: parameters
        })
            .pipe(
                catchError((err) => {
                    console.log(err);

                    return throwError('');
                })
            );
    }
    public findOne(options: IFindOneParameters, methodOptions?: IMethodOptions) {
        if (!options || (options && !options.id)) {
            throw new Error('Id is required.');
        }

        let action = options.id;

        if (options.association) {
            action += `/${options.association}`;
        }

        return this.action({
            action: action,
            parameters: options.parameters
        })
            .pipe(catchError((err) => {
                console.log(err);

                return throwError('');
            }))
    }
    public create(model: any): Observable<any> {
        throw new Error("Method not implemented.");
    }
    public update(model: any, methodOptions?: IMethodOptions): Observable<any> {
        throw new Error("Method not implemented.");
    }
    public url(url: string): IRepository {
        if (url) {
            this._url += url.startsWith('/') ? url : `/${url}`;
        }

        return this;
    }
    public destroy(id: any): Observable<any> {
        throw new Error("Method not implemented.");
    }
    public version(version: string): IRepository {
        throw new Error("Method not implemented.");
    }
}

@Injectable()
export class LamaDataService implements ILamaDataService {
    private apiPrefix: string;
    private apiRoot: string;
    private apiVersion: string;

    constructor(private http: HttpClient, private LamaModuleService: LamaModuleService) {
        this.apiVersion = 'v1';
    }

    public repository(modelOrUrl: string, apiVersion?: string) {
        let url = this.LamaModuleService.getApiPathWithoutPrefix(modelOrUrl);

        return new Repository(this.http, apiVersion || this.apiVersion, url);
    }
}

