import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { ChallengeService } from './challenge.service'
@Injectable()
export class TestGuard implements CanActivate {

  constructor(private router: Router, private challengeService: ChallengeService, private activatedRoute: ActivatedRoute) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
    return true;
    // return this.challengeService.IsTestFinished().do(
    //   isFinished => {
    //     if(isFinished === true){
    //       console.log("isfinshed is true");
    //       this.router.navigate(["/candidate/testFinished"]);
    //       return false;
    //     }
    //     else{
    //       console.log("isfinshed is false");
    //       this.router.navigate(["/candidate/instructions"]);
    //       return true;
    //     }
    //   });
  }
}
