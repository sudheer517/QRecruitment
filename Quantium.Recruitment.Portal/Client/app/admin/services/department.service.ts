import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { DepartmentDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class DepartmentService{
    public departmentApiUrl: string = '/Department/';

    constructor(private http: Http) { }

    public GetAll(): Observable<DepartmentDto[]> {
        return this.http.get(`${this.departmentApiUrl}GetAll`).map(
            (response: Response) => {
                return response.json();
            });
    }
}