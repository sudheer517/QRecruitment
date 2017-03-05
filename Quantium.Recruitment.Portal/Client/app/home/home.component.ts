import { Component, ViewEncapsulation } from '@angular/core';
// import { routerTransition, hostStyle } from '../router.animations';
import {NgbDropdownConfig} from '@ng-bootstrap/ng-bootstrap';

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
  public isNavbarCollapsed = true;
  constructor(config: NgbDropdownConfig) {
    config.autoClose = true;
  }
 }
