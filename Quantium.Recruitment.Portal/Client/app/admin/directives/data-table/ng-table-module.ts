import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NgTableComponent } from './table/ng-table.component';
import { NgTableFilteringDirective } from './table/ng-table-filtering.directive';
import { NgTablePagingDirective } from './table/ng-table-paging.directive';
import { NgTableSortingDirective } from './table/ng-table-sorting.directive';

import { SharedModule } from '../../../shared/shared.module';

@NgModule({
  imports: [CommonModule, SharedModule],
  declarations: [NgTableComponent, NgTableFilteringDirective, NgTablePagingDirective, NgTableSortingDirective],
  exports: [NgTableComponent, NgTableFilteringDirective, NgTablePagingDirective, NgTableSortingDirective]
})
export class TableModule {
}
