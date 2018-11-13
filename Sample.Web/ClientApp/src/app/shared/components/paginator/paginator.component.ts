import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { IPaginationStateViewModel } from '@shared/services/lama-data.service';

@Component({
    selector: 'paginator',
    templateUrl: './paginator.component.html',
    styleUrls: ['./paginator.component.scss']
})
export class PaginatorComponent implements OnChanges {
    @Input() maxSize: number = 6;
    @Input() paginationState: IPaginationStateViewModel;
    @Output() onPageChange = new EventEmitter<number>();

    public count: number;
    public limit: number;
    public page: number;

    public ngOnChanges(changes: SimpleChanges): void {
        if (changes.paginationState && changes.paginationState.currentValue) {
            const state = <IPaginationStateViewModel>changes.paginationState.currentValue;

            this.count = state.count;
            this.limit = state.limit;
            this.page = state.page;
        }
    }

    public onPaginationChange(currentPage: number) {
        this.onPageChange.emit(currentPage);
    }
}