import { Component } from '@angular/core';
import { map, finalize } from 'rxjs/operators';
import { LamaDataService, Method } from '@shared/services/lama-data.service';
import { AlertService } from '@shared/services/alert.service';

interface ITextUploadResultViewModel {
    reversedLines: Array<string>;
}

@Component({
    selector: 'home-page',
    templateUrl: './home-page.component.html',
    styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent {
    public files: Array<File> = [];
    public results: Array<string> = [];
    public showLoader: boolean;

    constructor(private AlertService: AlertService, private LamaDataService: LamaDataService) { }

    private prepareData() {
        
    }

    public ngOnInit() {
        this.prepareData();
    }

    public onFilesChange(files: Array<File>) {
        this.files = files;
    }

    public uploadFile() {
        if (this.files && this.files.length) {
            this.showLoader = true;

            this.LamaDataService
                .repository('textUpload')
                .actionFile({
                    action: '',
                    file: this.files[0],
                    fileFormDataName: 'file',
                    method: Method.post
                })
                .pipe(
                    map((res) => {
                        return res as ITextUploadResultViewModel;
                    }),
                    finalize(() => {
                        this.showLoader = false
                    })
                )
                .subscribe((result: ITextUploadResultViewModel) => {
                    this.results = result.reversedLines;
                })
        }
        else {
            this.AlertService.addWarningAlert('Prvo izberi datoteko!');
        }
    }
}