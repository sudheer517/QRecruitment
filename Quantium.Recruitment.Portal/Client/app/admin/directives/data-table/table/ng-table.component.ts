import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'ng2-table',
  template: `
    <table class="table dataTable table-responsive" ngClass="{{config.className || ''}}"
           style="display: table">
      <thead>
        <tr>
          <th *ngIf="!hideCheckbox">*</th>
          <th *ngFor="let column of columns" [ngTableSorting]="config" [column]="column" 
              (sortChanged)="onChangeTable($event)" ngClass="{{column.className || ''}}">
            {{column.title}}
            <i *ngIf="config && column.sort" class="pull-right fa"
              [ngClass]="{'fa-chevron-down': column.sort === 'desc', 'fa-chevron-up': column.sort === 'asc'}"></i>
          </th>
          <th style="text-align:center"><div tooltip="Recruiterbox" placement="left" ><a href="https://thequantiumgroup.recruiterbox.com/app/#candidates/overview"><i class="fa fa-lg fa-info-circle" ></i></a></div></th>
        </tr>
      </thead>
      <tbody>
      <tr *ngIf="showFilterRow">
        <td *ngIf="!hideCheckbox"></td>
        <td *ngFor="let column of columns">
          <input *ngIf="column.filtering" placeholder="{{column.filtering.placeholder}}"
                 [ngTableFiltering]="column.filtering"
                 class="form-control"
                 style="width: 100%; line-height: normal"
                 (tableChanged)="onChangeTable(config)"/>
        </td>
        <td></td>
      </tr>
        <tr *ngFor="let row of rows" style="line-height: 45px">
          <td *ngIf="!hideCheckbox" style="height: 100%">
            <div style="height: 100%; width: 100%; display: flex; justify-content: center; align-items: center">
                <input type="checkbox" #rowModel="ngModel" [(ngModel)]="selectedRows[row.Id]" name="rowCheckBox" (ngModelChange)="rowCheck(row, rowModel)" style="width: 20px;"/>
            </div>
          </td>
          
          <td style="padding: 0; height: 100%" *ngFor="let column of columns" (click)="cellClick(row, column.name)">
                <div style="height: 100%; width: 100%; display: flex; justify-content: center; align-items: center" [tooltip]="row.IsFinished ? 'Click for details' : ''" [class.cursor-pointer]="row.IsFinished">
                    {{getData(row, column.name)}}
                </div>
          </td>
          <td style="padding: 0; height: 100%">
            <div  style="height: 100%; width: 100%;display: flex; justify-content: center"> 
                <a tooltip="Candidate" placement="left" [href]="row.RecruiterBoxUrl" target="_blank"  style="color:black;margin:auto"><i class="fa fa-lg fa-user-circle"></i></a>
            </div>            
        </td>
        </tr>
      </tbody>
    </table>

  `,
  styles: [
    `
    .dataTable input{
        line-height: 20px !important;
    }
    `
  ]

})
export class NgTableComponent {
  // Table values
    @Input() public rows: Array<any> = [];
    @Input() public hideCheckbox = false;

  @Input()
  public set config(conf:any) {
    if (!conf.className) {
      conf.className = 'table-striped table-bordered';
    }
    if (conf.className instanceof Array) {
      conf.className = conf.className.join(' ');
    }
    this._config = conf;
  }

  selectedRows = new SelectedRowCheckboxes();
  // Outputs (Events)
  @Output() public tableChanged:EventEmitter<any> = new EventEmitter();
  @Output() public cellClicked:EventEmitter<any> = new EventEmitter();
  @Output() public rowChecked:EventEmitter<any> = new EventEmitter();

  public showFilterRow:Boolean = false;

  @Input()
  public set columns(values:Array<any>) {
    values.forEach((value:any) => {
      if (value.filtering) {
        this.showFilterRow = true;
      }
      if (value.className && value.className instanceof Array) {
        value.className = value.className.join(' ');
      }
      let column = this._columns.find((col:any) => col.name === value.name);
      if (column) {
        Object.assign(column, value);
      }
      if (!column) {
        this._columns.push(value);
      }
    });
  }

  private _columns:Array<any> = [];
  private _config:any = {};

  public constructor(private sanitizer:DomSanitizer) {
  }

  public sanitize(html:string):SafeHtml {
    return this.sanitizer.bypassSecurityTrustHtml(html);
  }

  public get columns():Array<any> {
    return this._columns;
  }

  public get config():any {
    return this._config;
  }

  public get configColumns():any {
    let sortColumns:Array<any> = [];

    this.columns.forEach((column:any) => {
      if (column.sort) {
        sortColumns.push(column);
      }
    });

    return {columns: sortColumns};
  }

  public onChangeTable(column:any):void {
    this._columns.forEach((col:any) => {
      if (col.name !== column.name && col.sort !== false) {
        col.sort = '';
      }
    });
    this.tableChanged.emit({sorting: this.configColumns});
  }

  public getData(row:any, propertyName:string):string {
    return propertyName.split('.').reduce((prev:any, curr:string) => prev[curr], row);
  }

  public cellClick(row:any, column:any):void {
    this.cellClicked.emit({row, column});
  }

  public rowCheck(row:any, rowModel: any):void {
    let selectedRows = this.selectedRows;
    this.rowChecked.emit({row, rowModel, selectedRows});
  }
}

export class SelectedRowCheckboxes{
  [key: number]: boolean;
}