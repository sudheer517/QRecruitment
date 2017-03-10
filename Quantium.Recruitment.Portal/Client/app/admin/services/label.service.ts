import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { LabelDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class LabelService{
    private labelApiUrl: string = '/Label/';

    constructor(private http: Http) { }

    public GetAllLabels(): Observable<LabelDto[]> {
        return this.http.get(`${this.labelApiUrl}GetAll`).map(
            (response: Response) => {
                return response.json();
            });
    }
}