import { environment } from '@environment';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { UIRouterModule, UIRouter, UIView } from '@uirouter/angular';

import { DictionariesModule } from './dictionaries/dictionaries.module';
import { HomeModule } from './home/home.module';
import { SharedModule } from './shared/shared.module';

import { AppComponent } from './app.component';

import { APP_STATES } from './app.states';

@NgModule({
    declarations: [
        AppComponent,
    ],
    imports: [
        BrowserModule,
        UIRouterModule.forRoot({
            states: APP_STATES,
            useHash: false/*,
            otherwise: { state: 'home' }*/
        }),
        SharedModule,
        DictionariesModule,
        HomeModule
    ],
    providers: [
    ],
    bootstrap: [UIView]
})
export class AppModule { }
