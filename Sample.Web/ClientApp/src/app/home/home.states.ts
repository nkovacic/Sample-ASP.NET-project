import { Ng2StateDeclaration } from '@uirouter/angular';

import { HomePageComponent } from './pages/home-page/home-page.component';

export const HOME_STATES = <Array<Ng2StateDeclaration>>[
    {
        parent: 'app',
        name: 'home',
        url: '/',
        component: HomePageComponent
    }
];