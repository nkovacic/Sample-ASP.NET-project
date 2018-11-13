import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    public isOpened: boolean = false;

    constructor() {
        
    }

    private prepareWatchers() {
        
    }

    public ngOnInit() {
        this.prepareWatchers();
    }
}
