import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpModule, JsonpModule } from '@angular/http';

import { PageHeadingComponent } from './directives/page-heading.directive';
import { DynamicFormComponent } from './forms/dynamic-form.component';
import { DynamicFormControlComponent } from './forms/dynamic-form-control.component';
import { ErrorMessageComponent } from './forms/error-message.component';
import { ErrorSummaryComponent } from './forms/error-summary.component';
import { FormControlService } from './forms/form-control.service';

import { HeaderComponent } from './layout/header.component';
import { FooterComponent } from './layout/footer.component';

import { UppercasePipe } from './pipes/uppercase.pipe';

// Services
import { DataService } from './services/data.service';
// import { AuthService } from './services/auth.service';
import { ContentService } from './services/content.service';
import { UtilityService } from './services/utility.service';

import { ChartModule } from 'angular2-highcharts';

// //import { ReactiveFormsModule } from '@angular/forms';
import { ModalModule, ButtonsModule } from 'ng2-bootstrap';
import { SelectModule } from 'ng2-select';

import { PaginationModule } from 'ng2-bootstrap/pagination';
import { Ng2TableModule } from 'ng2-table/ng2-table';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    // No need to export as these modules don't expose any components/directive etc'
    HttpModule,
    JsonpModule,
    ChartModule.forRoot(require('highcharts')),
    //Ng2FileInputModule.forRoot(),
    ReactiveFormsModule,
    ModalModule.forRoot(),
    ButtonsModule.forRoot(),
    SelectModule, 
    PaginationModule.forRoot(),   
    Ng2TableModule,
  ],
  declarations: [
    DynamicFormComponent,
    DynamicFormControlComponent,
    ErrorMessageComponent,
    ErrorSummaryComponent,
    FooterComponent,
    HeaderComponent,
    PageHeadingComponent,
    UppercasePipe
  ],
  exports: [
    // Modules
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    
    ChartModule,
    //Ng2FileInputModule,
    ModalModule,
    ButtonsModule,
    SelectModule,
    PaginationModule,
    Ng2TableModule,
    // Providers, Components, directive, pipes
    DynamicFormComponent,
    DynamicFormControlComponent,
    ErrorSummaryComponent,
    ErrorMessageComponent,
    FooterComponent,
    HeaderComponent,
    PageHeadingComponent,
    UppercasePipe
  ]

})
export class SharedModule {
  public static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [
        FormControlService,
        DataService,
        // AuthService,
        DataService,
        ContentService,
        UtilityService
      ]
    };
  }
}
