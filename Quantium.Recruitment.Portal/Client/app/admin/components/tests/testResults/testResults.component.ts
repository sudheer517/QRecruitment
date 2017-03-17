import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { TestService } from '../../../services/test.service';
import { TestResultDto } from '../../../../RemoteServicesProxy';

@Component({
    selector: 'appc-test-results',
    templateUrl: './testResults.component.html',
    providers: [DatePipe]
})
export class TestResultsComponent implements OnInit {
  public rows:Array<any> = [];
  public columns:Array<any> = [
      { title: 'Candidate', name: 'Candidate', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By Candidate' } },
      { title: 'Email', name: 'Email', sort: false, filtering: { filterString: '', placeholder: 'Filter By Email' } },
      { title: 'Job Applied', name: 'JobApplied', sort: false, filtering: { filterString: '', placeholder: 'Filter By Job' } },
      { title: 'Finished Date', name: 'FinishedDate', sort: false },
      { title: 'Test Result', name: 'IsTestPassed', sort: false },
      { title: 'College', name: 'College', sort: false, filtering: { filterString: '', placeholder: 'Filter By College' } },
      { title: 'CGPA', name: 'CGPA', sort: false },
      { title: 'Correct Answers', name: 'TotalRightAnswers', sort: false }
  ];
  public page:number = 1;
  public itemsPerPage:number = 10;
  public maxSize:number = 5;
  public numPages:number = 1;
  public length: number = 0;
  testResults: TestResultDto[];

  public config:any = {
    paging: true,
    sorting: {columns: this.columns},
    filtering: {filterString: ''},
    className: ['table-striped', 'table-bordered']
  };

  private data:Array<any>;

  public constructor(private testService: TestService, private datePipe: DatePipe) {
  }

  public ngOnInit(): void {
      this.testService.GetAllTestResults().subscribe(
          testResultsDto => {
              this.testResults = testResultsDto;

              for (let testResult of testResultsDto) {
                  testResult.IsTestPassed = testResult.IsTestPassed ? "Passed" : "Failed";
                  this.datePipe.transform(testResult.FinishedDate, 'medium');
              }

              this.data = testResultsDto;
              this.length = this.data.length;
              this.numPages = Math.ceil(this.length / this.itemsPerPage);
              this.maxSize = this.numPages;
              this.onChangeTable(this.config);
          },
          error => console.log(error)
      )      
  }

  public changePage(page:any, data:Array<any> = this.data):Array<any> {
    let start = (page.page - 1) * page.itemsPerPage;
    let end = page.itemsPerPage > -1 ? (start + page.itemsPerPage) : data.length;
    return data.slice(start, end);
  }

  public changeSort(data:any, config:any):any {
    if (!config.sorting) {
      return data;
    }

    let columns = this.config.sorting.columns || [];
    let columnName:string = void 0;
    let sort:string = void 0;

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
    return data.sort((previous:any, current:any) => {
      if (previous[columnName] > current[columnName]) {
        return sort === 'desc' ? -1 : 1;
      } else if (previous[columnName] < current[columnName]) {
        return sort === 'asc' ? -1 : 1;
      }
      return 0;
    });
  }

  public changeFilter(data:any, config:any):any {
    let filteredData:Array<any> = data;
    this.columns.forEach((column:any) => {
      if (column.filtering) {
        filteredData = filteredData.filter((item:any) => {
          return item[column.name].match(column.filtering.filterString);
        });
      }
    });

    if (!config.filtering) {
      return filteredData;
    }

    if (config.filtering.columnName) {
      return filteredData.filter((item:any) =>
        item[config.filtering.columnName].match(this.config.filtering.filterString));
    }

    let tempArray:Array<any> = [];
    filteredData.forEach((item:any) => {
      let flag = false;
      this.columns.forEach((column:any) => {
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

  public onChangeTable(config:any, page:any = {page: this.page, itemsPerPage: this.itemsPerPage}):any {
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