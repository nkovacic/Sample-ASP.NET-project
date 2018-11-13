import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { ngfModule } from "angular-file"
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap/pagination/pagination.module';
import { UIRouterModule } from '@uirouter/angular';

import { AjaxLoaderComponent } from './components/ajax-loader/ajax-loader.component';
import { AlertContainerComponent } from './components/alert-container/alert-container.component';
import { FileDropzoneComponent } from './components/file-dropzone/file-dropzone.component';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { NotFoundComponent } from './components/not-found-component/not-found.component';
import { PaginatorComponent } from './components/paginator/paginator.component';

import { NkFormModule } from './modules/form/form.module';

import { Helpers } from './utilities/helpers';

import { AlertService } from './services/alert.service';
import { LamaDataService } from './services/lama-data.service';
import { LamaModuleService } from './services/lama-module.service';

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        FormsModule,
        ngfModule,
        NgbPaginationModule,
        UIRouterModule.forChild(),
        NkFormModule
    ],
    declarations: [
        AjaxLoaderComponent,
        AlertContainerComponent,
        FileDropzoneComponent,
        FooterComponent,
        HeaderComponent,
        NotFoundComponent,
        PaginatorComponent   
    ],
    exports: [
        CommonModule,
        FormsModule,
        NkFormModule,
        AjaxLoaderComponent,
        AlertContainerComponent,
        FileDropzoneComponent,
        FooterComponent,
        HeaderComponent,
        NotFoundComponent,
        PaginatorComponent
    ],
    providers: [       
        Helpers,
        AlertService,
        LamaDataService,
        LamaModuleService
    ]
})
export class SharedModule { }