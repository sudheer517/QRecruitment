module Recruitment.Services {

    interface IConnectionService {
        getOdataConnection(): string;
    }

    export class ConnectionService implements IConnectionService {
        constructor() {
        }

        public getOdataConnection(): string {
            return "http://localhost:60606";
        }
    }
}
