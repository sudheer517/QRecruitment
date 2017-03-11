import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { CanLoad, Route } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable()
export class QAuthService{ 
    // static UNKNOWN_USER = new QAuthInfo(null);
    // authInfo$: BehaviorSubject<QAuthInfo> = new BehaviorSubject<QAuthInfo>(QAuthService.UNKNOWN_USER);
    private authUrl = "/Account/GetUserRole";
    public isLoggedIn = false;
    constructor(private http: Http){
    }
    
    isAdmin() {
        return this.http.get(this.authUrl).map(
            auth => {
                let result = auth;
                 if(auth.json() === "Admin")
                    return true;
                 else 
                    return false;
            });
    }
}

  