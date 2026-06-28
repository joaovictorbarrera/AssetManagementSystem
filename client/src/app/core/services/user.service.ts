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

    create(request: any) {
        return this.http.post(this.apiUrl, request)
    }

    updateRole(id: string, role: string) {
        return this.http.patch(`${this.apiUrl}/${id}/role`, { role })
    }

    updateActive(id: string, isActive: boolean) {
        return this.http.patch(`${this.apiUrl}/${id}/active`, { isActive })
    }
}
