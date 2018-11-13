import { Ng2StateDeclaration, Transition } from '@uirouter/angular';

import { AppComponent } from './app.component';

export const APP_STATES = <Array<Ng2StateDeclaration>>[
    {
        abstract: true,
        name: 'app',
        component: AppComponent
    }   
];