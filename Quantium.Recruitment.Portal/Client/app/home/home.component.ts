import { Component, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
// import { routerTransition, hostStyle } from '../router.animations';

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
  constructor(private router: Router) {
  }

  ngOnInit(){
    this.router.navigate(['admin']);
  }
 }
