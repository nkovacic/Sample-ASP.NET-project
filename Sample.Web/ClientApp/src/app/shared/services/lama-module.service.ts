import { Injectable } from '@angular/core';
import { Helpers } from '@shared/utilities/helpers';
import { BehaviorSubject } from 'rxjs';

interface ILamaModule {
    key: string;
    value: string;
}

@Injectable()
export class LamaModuleService {
    private modelMap: { [key: string]: string };

    constructor(private Helpers: Helpers) {
        this.modelMap = {
            'category': 'categories',
            'contact': 'contact',
            'company': 'companies',
            'listing': 'listings',
            'page': 'pages',
            'poi': 'poi',
            'region': 'regions',
            'statistic': 'statistics'
        };
    }

    public getApiPathWithoutPrefix(moduleName: string) {
        if (this.modelMap[moduleName]) {
            return this.modelMap[moduleName];
        }
        else {
            console.warn(`Module with name ${moduleName} was not found!`);
        }

        return '';
    }
}