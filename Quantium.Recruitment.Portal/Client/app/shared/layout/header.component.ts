// tslint:disable
import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'appc-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
    public isCollapsed: boolean = true;
    public languages = [
        { locale: 'en', description: 'English' },
        { locale: 'fr', description: 'French' }
    ];
    public currentLanguage = this.languages[0];


    constructor(
    ) { }


    public ngOnInit(): void {
    }

    public setLang(lang) {
        this.currentLanguage = lang;
    }

    public ngOnDestroy(): void {
    }
}
