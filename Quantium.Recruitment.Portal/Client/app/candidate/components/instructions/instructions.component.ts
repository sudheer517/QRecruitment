import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: '[appc-instructions]',
  styleUrls: ['./instructions.component.scss'],
  templateUrl: './instructions.component.html',
})
export class InstructionsComponent {
  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
  }

  takeTest(){
    this.router.navigate(["challenge"], { relativeTo : this.activatedRoute});
  }
 }
