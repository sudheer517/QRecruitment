import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { ChallengeService } from './challenge.service';

@Injectable()
export class InstructionsGuard implements CanActivate {

  constructor(private router: Router, private challengeService: ChallengeService, private activatedRoute: ActivatedRoute) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
        return this.challengeService.IsTestFinished().map(isFinished => {
            if (isFinished) {
                this.router.navigate(["/candidate/testFinished"]);
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
