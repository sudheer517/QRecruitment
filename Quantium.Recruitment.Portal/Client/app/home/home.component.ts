import { Component, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { QAuthService } from '../qauth.service'; 
// import { routerTransition, hostStyle } from '../router.animations';
import { Store } from '@ngrx/store';
import { AppState } from '../app-store';
import { LoggedInActions } from '../auth/logged-in.actions';

@Component({
  selector: 'appc-home',
  styleUrls: ['./home.component.scss'],
  templateUrl: './home.component.html',
  // animations: [routerTransition()],
  // encapsulation: ViewEncapsulation.None,
  // tslint:disable-next-line:use-host-property-decorator
  // host: hostStyle()
})
export class HomeComponent {

  constructor(private router: Router, private store: Store<AppState>, private loggedInActions: LoggedInActions) {
  }

  ngOnInit(){
    this.router.navigate(['admin']);
    this.store.select(authState => authState.auth.loggedIn).subscribe(
      isLogged =>
      { 
        if(isLogged === true){
          this.router.navigate(['admin']);
        }
        if(isLogged === false){
          this.router.navigate(['candidate']);
        }
      });
  }

 }
