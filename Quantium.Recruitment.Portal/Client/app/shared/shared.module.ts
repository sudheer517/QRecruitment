import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule, TranslateLoader } from 'ng2-translate/ng2-translate';
import { HttpModule, JsonpModule } from '@angular/http';

import { PageHeadingComponent } from './directives/page-heading.directive';
import { DynamicFormComponent } from './forms/dynamic-form.component';
import { DynamicFormControlComponent } from './forms/dynamic-form-control.component';
import { ErrorMessageComponent } from './forms/error-message.component';
import { ErrorSummaryComponent } from './forms/error-summary.component';
import { FormControlService } from './forms/form-control.service';
import { ApiTranslationLoader } from './services/api-translation-loader.service';

import { HeaderComponent } from './layout/header.component';
import { FooterComponent } from './layout/footer.component';

import { UppercasePipe } from './pipes/uppercase.pipe';

// Services
import { DataService } from './services/data.service';
// import { AuthService } from './services/auth.service';
import { ContentService } from './services/content.service';
import { UtilityService } from './services/utility.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    TranslateModule.forRoot({ provide: TranslateLoader, useClass: ApiTranslationLoader }),
    // No need to export as these modules don't expose any components/directive etc'
    HttpModule,
    JsonpModule
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
