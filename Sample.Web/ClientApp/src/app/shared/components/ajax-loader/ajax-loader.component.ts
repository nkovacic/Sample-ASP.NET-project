import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Helpers } from '@shared/utilities/helpers';
import { AjaxLoaderSize, IAjaxLoaderOptions } from './ajax-loader.model';

@Component({
    selector: 'ajax-loader',
    templateUrl: './ajax-loader.component.html',
    styleUrls: ['./ajax-loader.component.scss']
})
export class AjaxLoaderComponent implements OnInit, OnChanges {
    @Input() options: IAjaxLoaderOptions;
    @Input() showLoader = false;

    public ajaxLoaderOptions: IAjaxLoaderOptions;
    public cssClasses: string;

    constructor(private Helpers: Helpers) { }

    private prepareOptions() {
        let defaultAjaxLoaderOptions: IAjaxLoaderOptions = {
            inline: true,
            fixed: false,
            transparent: true,
            size: AjaxLoaderSize.large
        };

        this.ajaxLoaderOptions = this.Helpers.extend({}, defaultAjaxLoaderOptions, this.options);
    }

    public prepareCssClasses() {
        if (this.ajaxLoaderOptions) {
            let cssClasses = <string[]>[];

            if (this.ajaxLoaderOptions.fixed) {
                cssClasses.push('fixed-loader');
            }
            else if (this.ajaxLoaderOptions.inline) {
                cssClasses.push('inline-loader');
            }

            if (this.ajaxLoaderOptions.transparent) {
                cssClasses.push('transparent-loader');
            }

            cssClasses.push('loader-' + this.ajaxLoaderOptions.size);

            this.cssClasses = cssClasses.join(' ');
        }
    }

    public ngOnInit() {
        this.prepareOptions();
        this.prepareCssClasses();
    }

    public ngOnChanges(changes: SimpleChanges) {
        if (changes.options && changes.options.previousValue) {
            this.prepareOptions();
            this.prepareCssClasses();
        }
    }
}