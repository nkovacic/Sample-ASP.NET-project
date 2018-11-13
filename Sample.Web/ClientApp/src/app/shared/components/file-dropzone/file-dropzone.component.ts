import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Helpers } from '@shared/utilities/helpers';

@Component({
    selector: 'file-dropzone',
    templateUrl: './file-dropzone.component.html',
    styleUrls: ['./file-dropzone.component.scss']
})
export class FileDropzoneComponent implements OnInit {
    @Output() filesChanged: EventEmitter<Array<File>> = new EventEmitter<Array<File>>();

    public files: Array<File> = [];

    constructor(private Helpers: Helpers) { }

    private prepareWatchers() {
        
    }

    public onFilesChanged() {
        this.filesChanged.emit(this.files);
    }

    public ngOnInit() {
        this.prepareWatchers();
    }
}