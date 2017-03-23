import { AuthState, authReducer } from './auth/auth.store';
import { ActionReducer, combineReducers } from '@ngrx/store';
import { Injectable } from '@angular/core';


export interface AppState {
    auth: AuthState;
}

const reducers = {
    auth: authReducer
};

const reducer: ActionReducer<AppState> = combineReducers(reducers);

export function appReducer(state: any, action: any) {
    return reducer(state, action);
}

@Injectable()
export class GlobalState{
    constructor(){
    }

    public isAdmin = null;
}