import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Helpers } from '@shared/utilities/helpers';

@Component({
    selector: 'file-dropzone',
    templateUrl: './file-dropzone.component.html',
    styleUrls: ['./file-dropzone.component.scss']
})
export class FileDropzoneComponent implements OnInit {
    @Output() filesChanged: EventEmitter<Array<File>> = new EventEmitter<Array<File>>();

    public validComboDrag: boolean;
    public invalidComboDrag: boolean;
    public files: Array<File> = [];

    constructor(private Helpers: Helpers) { }

    private prepareWatchers() {
        
    }

    public onFilesChanged() {
        if (this.files.length > 1) {
            this.files.splice(0, this.files.length - 1);
        }
        
        this.filesChanged.emit(this.files);
    }

    public ngOnInit() {
        this.prepareWatchers();
    }
}