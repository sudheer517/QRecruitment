import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { ChallengeService } from './challenge.service'
@Injectable()
export class TestGuard implements CanActivate {

  constructor(private router: Router, private challengeService: ChallengeService, private activatedRoute: ActivatedRoute) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
        return this.challengeService.IsTestAssigned().map(isAssigned => {
            if (isAssigned) {
                return true;
            }
            else{
                this.router.navigate(["/candidate/unassignedTest"]);
                return false;
            }
        }).catch(() => {
            this.router.navigate(["/candidate/instructions"]);
            return Observable.of(false);
        });
  }
}
