import { NgModule } from '@angular/core';
import { UIRouterModule } from '@uirouter/angular';

import { SharedModule } from '@shared/shared.module';

import { DictionariesPageComponent } from './pages/dictionaries-page/dictionaries-page.component';

import { DICTIONARIES_STATES } from './dictionaries.states';

@NgModule({
    imports: [
        UIRouterModule.forChild({ states: DICTIONARIES_STATES }),
        SharedModule
    ],
    declarations: [
        DictionariesPageComponent
    ]
})
export class DictionariesModule { }
