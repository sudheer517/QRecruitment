import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Observable } from 'rxjs/Observable';
import { RouterModule } from '@angular/router';


/*
 * App Component
 * Top Level Component
 */

@Component({
  selector: 'appc-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {


  constructor(public titleService: Title) {
    // this language will be used as a fallback when a translation isn't found in the current language

    // the lang to use, if the lang isn't available, it will use the current loader to get them
  }

  public ngOnInit() {

  }

  public setTitle(newTitle: string) {
    this.titleService.setTitle(newTitle);
  }
}
