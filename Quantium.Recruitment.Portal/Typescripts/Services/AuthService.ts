
module Recruitment.Services {

    interface IAuthService {
        clear(): void;
        go(fallback: any): void;
        getMemorizedState(): any;
        setMemorizedState(stateValue: any): void;
        isAuthorized(): boolean;
        getRole(): string;
    }

    export class AuthService implements IAuthService {
        private authorized: boolean;
        private memorizedState: any;
        private role: string;

        constructor(private $state: angular.ui.IStateService, private $http: ng.IHttpService) {
            this.$http.get("/Temp/GetUserRole").then(response => {
                this.role = _.toString(response.data);
            },
            error => {
                console.log(error);
            });
        }

        public getRole(): string {
            return this.role;
        }

        public getMemorizedState(): any {
            return this.memorizedState;
        }

        public isAuthorized(): boolean {
            return this.authorized;
        }

        public setMemorizedState(stateValue: any): void {
            this.memorizedState = stateValue;
        }
        public clear(): void {
            this.authorized = false;
            this.memorizedState = null;
        }

        public go(fallback: any): void {
            this.authorized = true;
            var targetState = this.memorizedState ? this.memorizedState : fallback;
            this.$state.go(targetState);
        }
    }
}
