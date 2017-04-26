import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { DatePipe } from '@angular/common';
import { TestService } from '../../../services/test.service';
import { TestResultDto } from '../../../../RemoteServicesProxy';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDirective } from 'ng2-bootstrap/modal';

class SelectedRows{
    [key: number]: boolean;
}

@Component({
    selector: 'appc-archived-tests',
    templateUrl: './archivedTests.component.html',
    styleUrls: ['./archivedTests.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ArchivedTestsComponent implements OnInit {
@ViewChild('progress') progressModal: ModalDirective;
isRequestProcessing = true;
modalResponse: string;

  public rows:Array<any> = [];
  public columns:Array<any> = [
      { title: 'Candidate', name: 'Candidate', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By Candidate' }, className: 'table-header-cursor' },
      { title: 'Email', name: 'Email', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By Email' }, className: 'table-header-cursor'  },
      { title: 'Job Applied', name: 'JobApplied', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By Job' }, className: 'table-header-cursor'  },
      { title: 'Finished Date', name: 'FinishedDate', sort: 'asc', className: 'table-header-cursor'  },
      { title: 'Test Result', name:  'Result', sort: 'asc', className: 'table-header-cursor'  },
      { title: 'College', name: 'College', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By College' }, className: 'table-header-cursor'  },
      { title: 'CGPA', name: 'CGPA', sort: 'asc', className: 'table-header-cursor'  },
      { title: 'Correct Answers', name: 'TotalRightAnswers', sort: 'asc', className: 'table-header-cursor'  }
  ];
  public page:number = 1;
  public itemsPerPage:number = 10;
  public maxSize:number = 5;
  public numPages:number = 1;
  public length: number = 0;
  testResults: TestResultDto[];
  
  atleastOneRowSelected = false;
  selectedRows: any;

  public config:any = {
    paging: true,
    sorting: {columns: this.columns},
    filtering: {filterString: ''},
    className: ['table-striped', 'table-bordered']
  };

  private data:Array<any>;

  public constructor(private testService: TestService, private datePipe: DatePipe, private router: Router, private activatedRoute: ActivatedRoute) {
  }

  public ngOnInit(): void {
      this.getArchivedTests();
  }

  public getArchivedTests(modalOpened = false){
      this.testService.GetArchivedTestResults().subscribe(
          testResultsDto => {
              this.testResults = testResultsDto;
              if (testResultsDto.length > 0)
              {
                  for (let testResult of testResultsDto) {
                      //testResult.IsTestPassed = testResult.IsTestPassed ? "Passed" : "Failed";
                      let candidateFirstName = "";

                      if (testResult.Candidate.trim()) {
                          let splitNames = testResult.Candidate.split(" ");

                          if (splitNames.length > 0) {
                              candidateFirstName = splitNames[0];
                          }
                      }

                      testResult.RecruiterBoxUrl = `https://thequantiumgroup.recruiterbox.com/app/#candidates/list/type:search/search:${candidateFirstName}/`;
                      if(testResult.IsFinished){
                        testResult.FinishedDate = this.datePipe.transform(testResult.FinishedDate, 'medium');
                      }
                      else{
                          testResult.College = '';
                          testResult.FinishedDate = "";
                      }
                  }

                  this.data = testResultsDto;
                  this.length = this.data.length;
                  this.numPages = Math.ceil(this.length / this.itemsPerPage);
                  this.maxSize = this.numPages;
                  this.onChangeTable(this.config);
                  
              }
              else{
                    this.data = null;
                    this.testResults = [];
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

  public changePage(page: any, data: Array<any> = this.data): Array<any> {
      let start = (page.page - 1) * page.itemsPerPage;
      let end = page.itemsPerPage > -1 ? (start + page.itemsPerPage) : data.length;
      return data.slice(start, end);
  }

  public changeSort(data: any, config: any): any {
      if (!config.sorting) {
          return data;
      }

      //hack for first time page load only.refactor later
      if (this.columns.filter(col => col.sort.length === 0).length === 0) {
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
      //console.log(config);

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

      let hasFilterString = this.columns.filter(col => col.filtering && col.filtering.filterString.length > 0).length > 0;
      let filteredData = this.data;
      let sortedData = this.data;

      if(hasFilterString){
          filteredData = this.changeFilter(this.data, this.config);
      }
      
      sortedData = this.changeSort(filteredData, this.config);

      this.rows = page && config.paging ? this.changePage(page, sortedData) : sortedData;
      this.length = sortedData.length;
  }

  public onCellClick(data: any): any {
      console.log('clicked');
      let clickedTestResult = data.row as TestResultDto;
      if(clickedTestResult.IsFinished){
          //this.router.navigate(['testDetail', clickedTestResult.Id]);
          this.router.navigate(['../../testDetail', clickedTestResult.Id], { relativeTo: this.activatedRoute});
      }
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

  closeProgressModal(){
      this.progressModal.hide();
  }

  archiveSelectedTests(){
    let selectedTestIds = [];

    Object.keys(this.selectedRows).forEach((key, index) =>{
        if(this.selectedRows[key] === true){
            selectedTestIds.push(key);
        }
      });
    this.isRequestProcessing = true;
    this.modalResponse = "Archiving tests";
    this.progressModal.show();
    this.testService.ArchiveTests(selectedTestIds).subscribe(
        result => {
            this.modalResponse = "Refreshing tests";
            this.getArchivedTests(true);
        }, 
        error => console.log(error)
    );
  }

  public exportToExcel(){
      this.testService.GetExcelFileForArchivedTests().subscribe(
          data => {
                var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                if (navigator.msSaveOrOpenBlob) { // for IE and Edge
                    navigator.msSaveOrOpenBlob( blob, "Archived Test Results.xlsx" );
                }
                else {
                    var link=document.createElement('a');
                    document.body.appendChild(link);
                    link.href=window.URL.createObjectURL(blob);
                    link.download="Archived Test Results.xlsx";
                    link.click();
                }
          },
          error => console.log(error)
      )
  }

}