import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { CandidateService } from './candidate.service'
@Injectable()
export class CandidateDetailsGuard implements CanActivate {

  constructor(private router: Router, private candidateService: CandidateService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
    if (!this.candidateService.IsInformationFilled()) {
      this.router.navigate(['candidateDetails']);
      return false;
    }
    return true;
  }
}