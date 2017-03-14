import { Component, ViewEncapsulation, trigger, state, style, transition, animate, keyframes } from '@angular/core';
// import { routerTransition, hostStyle } from '../router.animations';
//import {NgbDropdownConfig} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'appc-admin',
  styleUrls: ['./admin.component.scss'],
  templateUrl: './admin.component.html',
  animations: [
      trigger("visibility",[
          state("shown", style({
            left: '0px'
          })),
          state("hidden", style({
            left: '-200px',
          })),
          transition("* <=> *", animate("200ms"))
          // transition("* <=> *", [
          //   style({opacity: '0'}),
          //   animate("500ms")
          // ])
      ])
  ],
  // encapsulation: ViewEncapsulation.Native,
  // tslint:disable-next-line:use-host-property-decorator
  // host: hostStyle()
})
export class AdminComponent {
  visibility: string = "hidden";
  setDisplayNoneForSideNav = true;
  isNavbarCollapsed: boolean;

  constructor() {
  }

  toggleNavButton(){
    this.setDisplayNoneForSideNav = false;
    this.isNavbarCollapsed = !this.isNavbarCollapsed;
    this.visibility = (this.visibility === "hidden" ? "shown" : "hidden");
  }
 }
