import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { CanLoad, Route } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { Store } from '@ngrx/store';
import { AppState, GlobalState } from './app-store';
import { LoggedInActions } from './auth/logged-in.actions';

@Injectable()
export class QAuthService{ 
    private authUrl = "/Account/GetUserRole";
    constructor(private http: Http, private store: Store<AppState>, private loggedInActions: LoggedInActions, private globalState: GlobalState){
    }
    
    isAdmin(): Observable<boolean> | boolean{
        if(this.globalState.isAdmin === null){
         return this.http.get(this.authUrl).map(
            auth => {
                 if(auth.json() === "Admin"){
                    this.globalState.isAdmin = true;
                    this.store.dispatch(this.loggedInActions.loggedIn());
                    return true;
                 }
                 else {
                     this.globalState.isAdmin = false;
                    this.store.dispatch(this.loggedInActions.notLoggedIn());
                    return false;
                 }
            });
        }
        else{
            return this.globalState.isAdmin;
        }
    }
}

  