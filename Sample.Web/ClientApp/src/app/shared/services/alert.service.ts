import { Injectable } from '@angular/core';
import { Helpers } from '@shared/utilities/helpers';
import { BehaviorSubject } from 'rxjs';

export enum AlertType {
    error = 'error',
    info = 'info',
    success = 'success',
    warning = 'warning'
}

export interface IAlertMessage {
    id?: string;
    message: string;
    title?: string;
    timeout?: number;
    alertType: AlertType;
}

@Injectable()
export class AlertService {
    private _alerts: BehaviorSubject<Array<IAlertMessage>>;
    private alerts: Array<IAlertMessage>;

    constructor(private Helpers: Helpers) {
        this.alerts = [];
        this._alerts = new BehaviorSubject([]);
    }

    public addErrorAlert(message: string) {
        return this.addAlert({
            alertType: AlertType.error,
            message: message
        });
    }

    public addSuccessAlert(message: string) {
        return this.addAlert({
            alertType: AlertType.success,
            message: message
        });
    }
    
    public addWarningAlert(message: string) {
        return this.addAlert({
            alertType: AlertType.warning,
            message: message
        });
    }

    public addAlert(message: IAlertMessage): IAlertMessage;
    public addAlert(message: string, alertType: AlertType): IAlertMessage;
    public addAlert(message: any, alertType?: AlertType): IAlertMessage {
        let alertMessage: IAlertMessage,
            defaultAlert = <IAlertMessage>{
                id: this.Helpers.createGuid(),
                timeout: 6000
            }

        if (this.Helpers.isString(message)) {
            alertMessage = this.Helpers.extend(defaultAlert, { message: message });
        }
        else {
            alertMessage = this.Helpers.extend(defaultAlert, message);
        }

        alertMessage.title = alertMessage.title || this.getTitleFromAlertType(alertMessage.alertType);

        this.alerts.push(alertMessage);
        this._alerts.next(this.Helpers.copy(this.alerts));

        setTimeout(() => {
            this.removeAlert(alertMessage.id);
        }, alertMessage.timeout);

        return alertMessage;
    }

    public addAlerts(alerts: Array<IAlertMessage>) {
        if (alerts && alerts.length) {
            alerts.forEach((alert) => {
                this.addAlert(alert);
            });
        }
    }

    public getAlerts() {
        return this._alerts.asObservable();
    }

    public removeAlert(alertId: string) {
        let indexOfAlert = this.Helpers.findIndex(this.alerts, { id: alertId });

        if (indexOfAlert != -1) {
            this.alerts.splice(indexOfAlert);
        }

        this._alerts.next(this.Helpers.copy(this.alerts));
    }

    private getTitleFromAlertType(alertType: AlertType) {
        switch (alertType) {
            case AlertType.error:
                return 'Napaka!';
            case AlertType.info:
                return 'Info!';
            case AlertType.success:
                return 'Uspeh!';
            case AlertType.warning:
                return 'Opozorilo!';
        }
    }
}