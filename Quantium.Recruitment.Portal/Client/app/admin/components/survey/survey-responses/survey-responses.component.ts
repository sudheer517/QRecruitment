import { Component, OnInit, ViewChild } from '@angular/core';
import { DatePipe } from '@angular/common';
import { SurveyResponseDto, CandidateDto, SurveyAdminCommentsDto } from '../../../../RemoteServicesProxy';
import { SurveyService } from '../../../services/survey.service';
import { ModalDirective } from 'ng2-bootstrap/modal';

@Component({
  selector: 'appc-survey-responses',
  templateUrl: './survey-responses.component.html',
  styleUrls: ['./survey-responses.component.scss']
})
export class SurveyResponsesComponent implements OnInit {

    @ViewChild('responsesPreview') responsesPreviewModal: ModalDirective;
    @ViewChild('progress') progressModal: ModalDirective;
    isRequestProcessing = true;
    modalResponse: string;

    responses: SurveyResponseDto[];
    candidates: CandidateDto[];
    comments: SurveyAdminCommentsDto[] =[];
    public rows: Array<any> = [];
    public columns: Array<any> = [
        { title: 'First Name', name: 'FirstName', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By First Name' }, className: 'table-header-cursor' },
        { title: 'Last Name', name: 'LastName', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By Last Name' }, className: 'table-header-cursor' },
        { title: 'Email', name: 'Email', sort: 'asc', filtering: { filterString: '', placeholder: 'Filter By Email' }, className: 'table-header-cursor' },      
        { title: 'Date Added', name: 'CreatedUtc', sort: 'asc', className: 'table-header-cursor' },       
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

    private hideCheckbox = true;
    private surveyGrid = true;    
    private commentEditedResponses = [];
    private responseIds = [];
   
    constructor(private surveyService: SurveyService, private datePipe: DatePipe) { }

  ngOnInit() {
      this.getSurveyedCandidates();

  }
  getSurveyedCandidates(modalOpened = false) {     
      this.surveyService.GetSurveydCandidates().subscribe(
          candidateList => {
             
              this.candidates = candidateList;
              if (this.candidates.length > 0) {
                  for (let candidate of candidateList) {
                      candidate.CreatedUtc = this.datePipe.transform(candidate.CreatedUtc, 'medium');                    
                  }
                  this.data = candidateList;
                  this.length = this.data.length;
                  this.numPages = Math.ceil(this.length / this.itemsPerPage);
                  this.maxSize = this.numPages;
                  this.onChangeTable(this.config);
              }
              else {
                  this.data = null;
                  this.candidates = [];
                  this.length = 0;
                  this.numPages = 1;
                  this.maxSize = 5;
                  this.rows = [];

              }
              if (modalOpened) {                 
                  this.isRequestProcessing = false;
                  this.progressModal.hide();
              }
          },
          error => console.log(error)
      );
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

  public changePage(page: any, data: Array<any> = this.data): Array<any> {
      let start = (page.page - 1) * page.itemsPerPage;
      let end = page.itemsPerPage > -1 ? (start + page.itemsPerPage) : data.length;
      return data.slice(start, end);
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

    //Reponse model events 
  public onviewResponseClicked(event)
  {     
      this.surveyService.GetSurveyResponses(event.Id).subscribe(
          candidateResponse => {
              this.responses = candidateResponse;
              this.responseIds = [];
              this.commentEditedResponses = [];
              for (let response of candidateResponse)
              {
                  this.responseIds.push(response.Id);
              }
              this.surveyService.GetSurveyAdminComments(this.responseIds).subscribe(
                  comments => {
                      this.comments = comments;
                      this.responsesPreviewModal.show();
                  },
                  error => console.log(error)
              );
             
              
          },
           error => console.log(error)

      );    
  }

  public addComment(response : SurveyResponseDto,element,text )
  {
      
      this.commentEditedResponses.push(response.Id);
      var newcomment = new SurveyAdminCommentsDto(response.Id, "");
      this.comments.push(newcomment);     
      
  }

  public commentEditable(response: SurveyResponseDto): Boolean {

      return this.commentEditedResponses.indexOf(response.Id) !== -1;      

  }

  public saveComment(response: SurveyResponseDto)
  {
      var comment = this.comments.find(a => a.ResponseId == response.Id && a.AdminId == 0);     
      this.surveyService.AddSurveyAdminComments(comment).subscribe(

          res => {
              var index = this.commentEditedResponses.indexOf(response.Id);
              if (index !== -1)
                  this.commentEditedResponses.splice(index, 1)
              this.surveyService.GetSurveyAdminComments(this.responseIds).subscribe(
                  comments => {
                      this.comments = comments;
                  },
                  error => console.log(error)
              );
          },
          error => console.log(error)
      );
      
  }

}
