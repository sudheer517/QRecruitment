import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { TestService } from '../../../services/test.service';
import { TestDto } from '../../../../RemoteServicesProxy';

@Component({
    selector: 'appc-view-tests',
    templateUrl: './viewTests.component.html',
    providers: [DatePipe]
})
export class ViewTestsComponent implements OnInit{
   tests: TestDto[];
   public rows: Array<any> = [];
   public columns: Array<any> = [
       { title: 'Job Title', name: 'Title', sort: false, filtering: { filterString: '', placeholder: 'Filter By Job' } },
       { title: 'Candidate', name: 'CandidateName', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By Candidate' } },       
       { title: 'Email', name: 'Email', sort: false, filtering: { filterString: '', placeholder: 'Filter By Email' } },
       { title: 'Test Status', name: 'Status', sort: false },
       { title: 'Created Date', name: 'CreatedUtc', sort: false },
       { title: 'Finished Date', name: 'FinishedDate', sort: false },
       
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

   constructor(private testService: TestService, private datePipe : DatePipe) {
   }

   ngOnInit(){
     this.testService.GetAll().subscribe(
       tests => {
           this.tests = tests;
           for (let test of tests)
           {
               test.CandidateName = test.Candidate.FirstName + " " + test.Candidate.LastName;
               test.Email = test.Candidate.Email;
               test.Title = test.Job.Title;
               test.FinishedDate = test.IsFinished ? (this.datePipe.transform(test.FinishedDate, 'medium')) : "N/A";
               test.CreatedUtc = this.datePipe.transform(test.CreatedUtc, 'medium');
               test.Status = test.IsFinished ? "Finished" : "Pending";
               
           }
           this.data = tests;
           this.length = this.data.length;
           this.numPages = Math.ceil(this.length / this.itemsPerPage);
           this.maxSize = this.numPages;
           this.onChangeTable(this.config);
         console.log(tests);
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
       console.log(data);
   }
}