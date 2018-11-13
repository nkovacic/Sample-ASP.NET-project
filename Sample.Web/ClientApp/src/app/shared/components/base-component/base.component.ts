import { Component, Injectable, Input, OnDestroy, OnInit } from '@angular/core';
import { Helpers } from '@shared/utilities/helpers';
import { Subject } from 'rxjs';

@Injectable()
export class BaseComponent implements OnDestroy, OnInit {
    private viewModel: any;

    protected ngUnsubscribe: Subject<any> = new Subject();

    constructor() { }

    public ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    public ngOnInit() {
        
    }
}