import { Component, ViewEncapsulation } from '@angular/core';
// import { routerTransition, hostStyle } from '../router.animations';
//import {NgbDropdownConfig} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'appc-admin',
  styleUrls: ['./admin.component.scss'],
  templateUrl: './admin.component.html',
  // animations: [routerTransition()],
  // encapsulation: ViewEncapsulation.Native,
  // tslint:disable-next-line:use-host-property-decorator
  // host: hostStyle()
})
export class AdminComponent {
  public isNavbarCollapsed = true;
  constructor() {
  }
 }
