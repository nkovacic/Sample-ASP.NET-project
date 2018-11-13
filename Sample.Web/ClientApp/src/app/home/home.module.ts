import { NgModule } from '@angular/core';
import { UIRouterModule } from '@uirouter/angular';

import { SharedModule } from '@shared/shared.module';

import { HomePageComponent } from './pages/home-page/home-page.component';

import { HOME_STATES } from './home.states';

@NgModule({
    imports: [
        UIRouterModule.forChild({ states: HOME_STATES }),
        SharedModule
    ],
    declarations: [
        HomePageComponent
    ]
})
export class HomeModule { }
