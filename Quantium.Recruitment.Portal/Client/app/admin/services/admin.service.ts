import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { AdminDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AdminService {

    public adminApiUrl: string = '/Admin/';

    constructor(private http: Http) { }

    public IsAdmin(): Observable<AdminDto> {
        return this.http.get(this.adminApiUrl + "/IsAdmin?email=Rakesh.Aitipamula@quantium.co.in").map(
            (response: Response) => {
                return response.json();
            });
    }

    public AddAdmin(adminDto: AdminDto): Observable<AdminDto> {
        return this.http.post(this.adminApiUrl + "AddAdminAsync", adminDto).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetAdminByEmail(email: string): Observable<AdminDto> {
        return this.http.get(`${this.adminApiUrl}GetAdminByEmail?email=${email}`).map(
            (response: Response) => {
                return response.json();
            });
    }
}
