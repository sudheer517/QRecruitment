import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { CandidateService } from '../../../services/candidate.service';
import { CandidateDto } from '../../../../RemoteServicesProxy';
import { FilterCandidatesPipe } from '../../../pipes/filterCandidates.pipe';


@Component({
    selector: 'appc-view-candidates',
    templateUrl: './viewCandidates.component.html',
    providers: [DatePipe]
})

export class ViewCandidatesComponent implements OnInit{

    candidates: CandidateDto[];
    public rows: Array<any> = [];
    public columns: Array<any> = [
        { title: 'First Name', name: 'FirstName', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By First Name' } },
        { title: 'Last Name', name: 'LastName', sort: false, filtering: { filterString: '', placeholder: 'Filter By Last Name' } },
        { title: 'Email', name: 'Email', sort: false, filtering: { filterString: '', placeholder: 'Filter By Email' } },
        { title: 'Status', name: 'IsActive', sort: false },
        { title: 'Account Status', name: 'PasswordSent', sort: false },
        { title: 'Test Email', name: 'TestMailSent', sort: false },
        { title: 'Date Added', name: 'CreatedUtc', sort: false}        
    ];
    public page: number = 1;
    public itemsPerPage: number = 10;
    public maxSize: number = 5;
    public numPages: number = 1;
    public length: number = 0;

    public config: any = {
        paging: true,
        sorting: { columns: this.columns },
        filtering: { filterString: '' },
        className: ['table-striped', 'table-bordered']
    };
    private data: Array<any>;
    public constructor(private candidateService: CandidateService, private datePipe: DatePipe) {
    }

    ngOnInit() {
        this.candidateService.GetAllCandidates().subscribe(
            candidateList => {
                this.candidates = candidateList;
                for (let candidate of candidateList) {
                    candidate.IsActive = candidate.IsActive ? "Active" : "Archived";
                    candidate.PasswordSent = candidate.PasswordSent ? "Created" : "Pending";
                    candidate.TestMailSent = candidate.TestMailSent ? "Test Created" : "No Test";
                    candidate.CreatedUtc = this.datePipe.transform(candidate.CreatedUtc, 'medium');
                }
                this.data = candidateList;
                this.length = this.data.length;
                this.numPages = Math.ceil(this.length / this.itemsPerPage);
                this.maxSize = this.numPages;
                this.onChangeTable(this.config);
            },
            error => console.log(error)
        )
    }
    public changePage(page: any, data: Array<any> = this.data): Array<any> {
        let start = (page.page - 1) * page.itemsPerPage;
        let end = page.itemsPerPage > -1 ? (start + page.itemsPerPage) : data.length;
        return data.slice(start, end);
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
                    return item[column.name].toLowerCase().match(column.filtering.filterString.toLowerCase());
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
        console.log(data);
    }
}