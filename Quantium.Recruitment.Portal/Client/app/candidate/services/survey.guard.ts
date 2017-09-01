import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { SurveyService } from './survey.service'
@Injectable()
export class SurveyGuard implements CanActivate {

    constructor(private router: Router, private surveyService: SurveyService, private activatedRoute: ActivatedRoute) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {    

      return this.surveyService.IsSurveyFinished().map(
          isFinished => {
              if (isFinished) {
                  this.router.navigate(["../challenges"]);
                  return false;
              }
              else
                  return true;              
          },
          error => { console.log(error); }
      );

      
     
  }
}
