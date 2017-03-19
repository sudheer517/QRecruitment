import { NgModule, NgModuleFactoryLoader } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { StoreModule } from '@ngrx/store';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/Router';
import { SharedModule } from './shared/shared.module';
import { HomeModule } from './home/home.module';

import { routing } from './app.routes';
import { AppComponent } from './app.component';

import { QAuthGuard } from './qauth.guard';
import { QAuthService } from './qauth.service';

@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        routing,
        // FormsModule,
        HttpModule,
        // Only module that app module loads
        SharedModule.forRoot(),
        HomeModule
    ],
    providers: [
        QAuthGuard, QAuthService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
