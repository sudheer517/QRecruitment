import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { CandidateService } from './candidate.service'
@Injectable()
export class InstructionsGuard implements CanActivate {

  constructor(private router: Router, private candidateService: CandidateService, private activatedRoute: ActivatedRoute) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
    
    return this.candidateService.IsInformationFilled().do(
      isFilled => {
        if(isFilled === true){
          console.log("navigatiing to instruc");
          return true;
        }
        else{
          console.log("navigatiing to cand details");
          this.router.navigate(["/candidate/candidateDetails"]);
          return false;
        }
      });
  }
}
