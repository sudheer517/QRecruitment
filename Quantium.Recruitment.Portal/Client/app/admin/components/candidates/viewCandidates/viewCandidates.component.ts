import { Component, OnInit, ViewChild, ViewEncapsulation, ChangeDetectorRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { CandidateService } from '../../../services/candidate.service';
import { CandidateDto } from '../../../../RemoteServicesProxy';
import { FilterCandidatesPipe } from '../../../pipes/filterCandidates.pipe';
import { ModalDirective } from 'ng2-bootstrap/modal';

@Component({
    selector: 'appc-view-candidates',
    templateUrl: './viewCandidates.component.html',
    styleUrls: ['./viewCandidates.component.scss'],
    encapsulation: ViewEncapsulation.None
})

export class ViewCandidatesComponent implements OnInit{

    @ViewChild('progress') progressModal: ModalDirective;
    isRequestProcessing = true;
    modalResponse: string;

    atleastOneRowSelected = false;
    selectedRows: any;

    candidates: CandidateDto[];
    public rows: Array<any> = [];
    public columns: Array<any> = [
        { title: 'First Name', name: 'FirstName', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By First Name' }, className: 'table-header-cursor' },
        { title: 'Last Name', name: 'LastName', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By Last Name' }, className: 'table-header-cursor' },
        { title: 'Email', name: 'Email', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By Email' }, className: 'table-header-cursor' },
        { title: 'Password', name: 'Password', className: 'table-header-cursor' },
        // { title: 'Status', name: 'IsActive', sort: false },
        // { title: 'Account Status', name: 'PasswordSent', sort: false },
        // { title: 'Test Email', name: 'TestMailSent', sort: false },
        { title: 'Date Added', name: 'CreatedUtc', sort: 'asc', className: 'table-header-cursor'}        
    ];
    public page: number = 1;
    public itemsPerPage: number = 10;
    public maxSize: number = 5;
    public numPages: number = 0;
    
    public length: number = 0;

    public config: any = {
        paging: true,
        sorting: { columns: this.columns },
        filtering: { filterString: '' },
        className: ['table-striped', 'table-bordered']
    };
    private data: Array<any>;
    public constructor(private candidateService: CandidateService, private datePipe: DatePipe, private cdRef: ChangeDetectorRef) {
    }

    ngOnInit() {
        this.getAllCandidates();
    }

    ngAfterViewChecked() {
        this.cdRef.detectChanges();
    }

    public getAllCandidates(modalOpened = false){
        this.candidateService.GetAllCandidatesWithPassword().subscribe(
            candidateList => {
                this.candidates = candidateList;
                if(this.candidates.length > 0){
                    for (let candidate of candidateList) {
                        candidate.CreatedUtc = this.datePipe.transform(candidate.CreatedUtc, 'medium');
                    }
                    this.data = candidateList;
                    this.length = this.data.length;
                    //this.numPages = Math.ceil(this.length / this.itemsPerPage);
                    //this.maxSize = this.numPages;
                    this.onChangeTable(this.config);
                }
                else{
                    this.data = null;
                    this.candidates = [];
                    this.length = 0;
                    this.numPages = 1;
                    this.maxSize = 5;
                    this.rows = [];
                    
              }
              if(modalOpened){
                this.atleastOneRowSelected = false;
                this.isRequestProcessing = false;
                this.progressModal.hide();
              }
            },
            error => console.log(error)
        );
    }

    public archiveSelectedCandidates(){
        let selectedCandidateIds = [];

        Object.keys(this.selectedRows).forEach((key, index) =>{
            if(this.selectedRows[key] === true){
                selectedCandidateIds.push(key);
            }
        });
        this.isRequestProcessing = true;
        this.modalResponse = "Archiving candidates";
        this.progressModal.show();
        this.candidateService.ArchiveCandidates(selectedCandidateIds).subscribe(
            result => {
                this.modalResponse = "Refreshing candidates";
                this.getAllCandidates(true);
            }, 
            error => console.log(error)
        );
    }
    public changePage(page: any, data: Array<any> = this.data): Array<any> {
        let start = (page.page - 1) * page.itemsPerPage;
        let end = page.itemsPerPage > -1 ? (start + page.itemsPerPage) : data.length;
        return data.slice(start, end);
    }

    public onRowCheck(data: any): any{
      this.selectedRows = data.selectedRows;
      Object.keys(data.selectedRows).forEach((key, index) =>{
        if(data.selectedRows[key] === true){
            this.atleastOneRowSelected = true;
            return;
        }
        this.atleastOneRowSelected = false;
      });
    }

    public changeSort(data: any, config: any): any {
        if (!config.sorting) {
            return data;
        }

        let columns = this.config.sorting.columns || [];
        let columnName: string = void 0;
        let sort: string = void 0;

        for (let i = 0; i < columns.length; i++) {
            if (columns[i].sort !== '' && columns[i].sort !== false) {
                columnName = columns[i].name;
                sort = columns[i].sort;
            }
        }

        if (!columnName) {
            return data;
        }

        // simple sorting
        return data.sort((previous: any, current: any) => {
            if (previous[columnName] > current[columnName]) {
                return sort === 'desc' ? -1 : 1;
            } else if (previous[columnName] < current[columnName]) {
                return sort === 'asc' ? -1 : 1;
            }
            return 0;
        });
    }

    public changeFilter(data: any, config: any): any {

        let filteredData: Array<any> = data;
        this.columns.forEach((column: any) => {
            if (column.filtering) {
                filteredData = filteredData.filter((item: any) => {
                    if (item[column.name] != null)
                        return item[column.name].toLowerCase().match(column.filtering.filterString.toLowerCase());
                    else
                        return null;
                });
            }
        });

        if (!config.filtering) {
            return filteredData;
        }

        if (config.filtering.columnName) {
            return filteredData.filter((item: any) =>
                item[config.filtering.columnName].toLowerCase().match(this.config.filtering.filterString.toLowerCase()));
        }

        let tempArray: Array<any> = [];
        filteredData.forEach((item: any) => {
            let flag = false;
            this.columns.forEach((column: any) => {
                if (item[column.name].toString().match(this.config.filtering.filterString)) {
                    flag = true;
                }
            });
            if (flag) {
                tempArray.push(item);
            }
        });
        filteredData = tempArray;

        return filteredData;
    }

    public onChangeTable(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
        
        if (config.filtering) {
            Object.assign(this.config.filtering, config.filtering);
        }

        if (config.sorting) {
            Object.assign(this.config.sorting, config.sorting);
        }

        let filteredData = this.changeFilter(this.data, this.config);
        let sortedData = this.changeSort(filteredData, this.config);
        this.rows = page && config.paging ? this.changePage(page, sortedData) : sortedData;
        this.length = sortedData.length;
    }

    public onCellClick(data: any): any {
        //console.log(data);
    }
}