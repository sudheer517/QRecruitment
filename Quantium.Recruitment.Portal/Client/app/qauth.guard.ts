import { Injectable } from '@angular/core';
import { CanLoad, Router, RouterModule, Route } from '@angular/router';
import { QAuthService } from './qauth.service';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable()
export class QAuthGuard implements CanLoad { 
    constructor(private qauthService: QAuthService, private router: Router){
    }

    canLoad(route: Route) {
      return this.qauthService.isAdmin();
    }

}

