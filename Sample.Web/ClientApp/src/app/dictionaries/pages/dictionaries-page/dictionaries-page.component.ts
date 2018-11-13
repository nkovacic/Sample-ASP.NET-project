import { Component } from '@angular/core';
import { Subject, of } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, map, finalize } from 'rxjs/operators';
import { LamaDataService, IPaginationViewModel } from '@shared/services/lama-data.service';
import { AlertService } from '@shared/services/alert.service';

interface IDictionaryValue {
    value: string;
}

interface IDictionary extends IDictionaryValue {
    key: string;
}

@Component({
    selector: 'dictionaries-page',
    templateUrl: './dictionaries-page.component.html',
    styleUrls: ['./dictionaries-page.component.scss']
})
export class DictionariesPageComponent {
    public debounceNgModelChange: Subject<string> = new Subject<string>();
    public dictionaries: Array<IDictionaryValue> = []; 
    public searchQuery: string;
    public showLoader: boolean;

    constructor(private AlertService: AlertService, private LamaDataService: LamaDataService) {
        this.prepareDebounce();
    }

    private prepareDebounce() {
        this.debounceNgModelChange
            .pipe(
                map(() => this.searchQuery),
                debounceTime(400),
                distinctUntilChanged()
            )
            .subscribe(() => {
                this.onSearchQueryChange();
            })
            
    }

    public ngOnInit() {
        this.fetchAllDictionaries();
    }

    public fetchValueFromKey() {
        this.showLoader = true;

        this.LamaDataService
            .repository('dictionary')
            .action({
                action: `${this.searchQuery}/value`
            })
            .pipe(
                catchError(() => {
                    this.dictionaries = [];

                    return of({});
                }),
                finalize(() => {
                    this.showLoader = false
                })
            )
            .subscribe((result: IDictionaryValue) => {
                this.dictionaries = [result];
            })
    }

    public fetchAllDictionaries() {
        this.showLoader = true;

        this.LamaDataService
            .repository('dictionary')
            .action({
                action: 'keys'
            })
            .pipe(
                finalize(() => {
                    this.showLoader = false
                })
            )
            .subscribe((paginationViewModel: IPaginationViewModel<string>) => {
                if (paginationViewModel.items && paginationViewModel.items.length) {
                    this.dictionaries = paginationViewModel.items.map((key) => ({ key: key } as IDictionary));
                }
                else {
                    this.dictionaries = [];
                }
            });
    }

    public onSearchQueryChange() {
        if (this.searchQuery) {
            this.fetchValueFromKey();
        }
        else {
            this.fetchAllDictionaries();
        }
    }
}