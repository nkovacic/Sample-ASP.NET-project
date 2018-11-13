import { Component } from '@angular/core';

@Component({
    selector: 'home-page',
    templateUrl: './home-page.component.html',
    styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent {
    public showLoader: boolean;

    constructor() {

    }

    private prepareData() {
        
    }

    public ngOnInit() {
        this.prepareData();
    }
}