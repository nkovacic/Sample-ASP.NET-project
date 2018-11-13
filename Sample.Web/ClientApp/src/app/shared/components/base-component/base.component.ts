import { Component, Injectable, Input, OnDestroy, OnInit } from '@angular/core';
import { TransferDataService } from '@shared/services/transfer-data.service';
import { Helpers } from '@shared/utilities/helpers';
import { inject } from '@angular/core/testing';
import { Subject } from 'rxjs';

@Injectable()
export class BaseComponent implements OnDestroy, OnInit {
    @Input('td-prop') private viewModelProperty: string;

    private viewModel: any;

    protected ngUnsubscribe: Subject<any> = new Subject();

    constructor(private TransferDataService: TransferDataService) { }

    protected getViewModel<T>() {
        return <T>this.viewModel;
    }

    protected hasViewModel() {
        return this.TransferDataService.hasViewModel(this.viewModelProperty);
    }

    protected isViewModelEmpty() {
        return this.TransferDataService.isViewModelEmpty(this.viewModelProperty);
    }

    public ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    public ngOnInit() {
        this.viewModel = this.TransferDataService.getViewModel(this.viewModelProperty);
    }
}