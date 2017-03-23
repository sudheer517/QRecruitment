import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { CandidateService } from './candidate.service'

@Injectable()
export class DetailsGuard implements CanActivate {

  constructor(private router: Router, private candidateService: CandidateService, private activatedRoute: ActivatedRoute) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
        return this.candidateService.IsInformationFilled().map(isFilled => {
            if (isFilled) {
                this.router.navigate(["/candidate/instructions"]);
                return false;
            }
            else{
                return true;
            }
        }).catch(() => {
            this.router.navigate(["/candidate/instructions"]);
            return Observable.of(false);
        });
  }
}
