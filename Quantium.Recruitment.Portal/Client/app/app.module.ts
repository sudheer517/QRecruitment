import { NgModule, NgModuleFactoryLoader } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/Router';
import { SharedModule } from './shared/shared.module';
import { HomeModule } from './home/home.module';

import { StoreModule } from '@ngrx/store';

import { routing } from './app.routes';
import { AppComponent } from './app.component';

import { QAuthGuard } from './qauth.guard';
import { QAuthService } from './qauth.service';
import { appReducer, GlobalState } from './app-store';

import { LoggedInActions } from './auth/logged-in.actions';


@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        routing,
        // FormsModule,
        HttpModule,
        // Only module that app module loads
        SharedModule.forRoot(),
        StoreModule.provideStore(appReducer),
        HomeModule
    ],
    providers: [
        QAuthGuard, QAuthService, LoggedInActions, GlobalState
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
