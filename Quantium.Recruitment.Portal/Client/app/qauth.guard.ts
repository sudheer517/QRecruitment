import { Injectable } from '@angular/core';
import { CanLoad, Router, RouterModule, Route } from '@angular/router';
import { QAuthService } from './qauth.service';

import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { Store } from '@ngrx/store';
import { AppState } from './app-store';
import { LoggedInActions } from './auth/logged-in.actions';

@Injectable()
export class QAuthGuard implements CanLoad { 
    constructor(private qauthService: QAuthService, private router: Router, private store: Store<AppState>, private loggedInActions: LoggedInActions){
    }

    canLoad(route: Route) {
      //return this.store.select(authState => authState.auth.loggedIn);
      return this.qauthService.isAdmin();
    }

}

