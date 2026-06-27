import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { environment } from '../../../environments/environment'

@Injectable({
    providedIn: 'root',
})
export class UserService {
    private readonly apiUrl = `${environment.apiUrl}/users`

    constructor(private http: HttpClient) {}

    getUsers(request: any = {}) {
        return this.http.get(this.apiUrl, {
            params: request,
        })
    }

    getFields() {
        return this.http.get(`${this.apiUrl}/fields`)
    }
}
