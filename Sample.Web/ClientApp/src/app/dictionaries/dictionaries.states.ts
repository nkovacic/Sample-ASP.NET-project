import { Ng2StateDeclaration } from '@uirouter/angular';

import { DictionariesPageComponent } from './pages/dictionaries-page/dictionaries-page.component';

export const DICTIONARIES_STATES = <Array<Ng2StateDeclaration>>[
    {
        parent: 'app',
        name: 'dictionaries',
        url: '/dictionaries',
        component: DictionariesPageComponent
    }
];