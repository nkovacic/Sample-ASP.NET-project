import { Component } from '@angular/core';
import * as getYear from 'date-fns/getYear';

@Component({
    selector: 'layout-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
    public currentYear: number = getYear(new Date());
}