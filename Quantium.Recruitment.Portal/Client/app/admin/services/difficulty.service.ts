import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { DifficultyDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class DifficultyService{
    private difficultyApiUrl: string = '/Difficulty/';

    constructor(private http: Http) { }

    public GetAllDifficulties(): Observable<DifficultyDto[]> {
        return this.http.get(`${this.difficultyApiUrl}GetAll`).map(
            (response: Response) => {
                return response.json();
            });
    }
}