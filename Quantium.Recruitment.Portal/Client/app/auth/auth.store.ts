import { ActionReducer, combineReducers } from '@ngrx/store';
import { loggedInReducer } from './logged-in.reducer';

export interface AuthState {
    loggedIn: boolean;
}

const reducers = combineReducers({
    loggedIn: loggedInReducer
});

export function authReducer( state: any, action: any ): ActionReducer<AuthState> {
    return reducers(state, action);
}
