import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Observable } from 'rxjs/Observable';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { QAuthService } from './qauth.service';
import { Store } from '@ngrx/store';
import { AppState } from './app-store';
import { LoggedInActions } from './auth/logged-in.actions';
/*
 * App Component
 * Top Level Component
 */

@Component({
  selector: 'appc-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  isRoleReceived = false;
  constructor(private router: Router, public titleService: Title, private store: Store<AppState>, private loggedInActions: LoggedInActions, private qAuthService: QAuthService) {
  }

  public ngOnInit() {
    // this.qAuthService.isAdmin().subscribe(
    //   result => console.log(result)
    // );
    
    this.store.select(state => state.auth).subscribe(
      authState => {
        this.isRoleReceived = true;
    });
  }

  public setTitle(newTitle: string) {
    this.titleService.setTitle(newTitle);
  }
}
