import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Helpers } from '@shared/utilities/helpers';

import { AlertService, AlertType, IAlertMessage } from '@shared/services/alert.service';

@Component({
    selector: 'alert-container',
    templateUrl: './alert-container.component.html',
    styleUrls: ['./alert-container.component.scss']
})
export class AlertContainerComponent implements OnInit {
    public alerts: Array<IAlertMessage>;

    constructor(private Helpers: Helpers, private AlertService: AlertService) { }

    private prepareWatchers() {
        this.AlertService
            .getAlerts()
            .subscribe((alerts) => {
                this.alerts = alerts;
            });
    }

    public ngOnInit() {
        this.prepareWatchers();
    }

    public getAlertCssClasses(alert: IAlertMessage) {
        let cssClass = 'alert-';

        switch (alert.alertType) {
            case AlertType.success:
                return cssClass + 'success';
            case AlertType.error:
                return cssClass + 'error';
            case AlertType.info:
                return cssClass + 'info';
            case AlertType.warning:
                return cssClass + 'warning';
        }
    }

    public getAlertIcon(alert: IAlertMessage) {
        let iconCssClass = 'sl sl-icon-';

        switch (alert.alertType) {
            case AlertType.success:
                return iconCssClass + 'check';
            case AlertType.error:
                return iconCssClass + 'exclamation';
            case AlertType.info:
                return iconCssClass + 'info';
            case AlertType.warning:
                return iconCssClass + 'exclamation';
        }
    }

    public removeAlert(alert: IAlertMessage) {
        this.AlertService.removeAlert(alert.id);
    }

    public trackByAlert(index: number, item: IAlertMessage) {
        return item.id;
    }
}