// tslint:disable
import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';

import { AppState } from './../../app-store';
import { AccountService } from './../../core/account/account.service';
import { AuthTokenService } from './../../core/auth-token/auth-token.service';
import { AuthState } from '../../core/auth-store/auth.store';


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

    public authState$: Observable<AuthState>;

    constructor(
        public tokens: AuthTokenService,
        public store: Store<AppState>,
        public accountService: AccountService,
    ) { }


    public ngOnInit(): void {
        this.authState$ = this.store.select(state => state.auth);
    }

    public toggleNav() {
        this.isCollapsed = !this.isCollapsed;
    }

    public setLang(lang) {
        this.currentLanguage = lang;
    }

    public ngOnDestroy(): void {
        this.tokens.unsubscribeRefresh();
    }
}
