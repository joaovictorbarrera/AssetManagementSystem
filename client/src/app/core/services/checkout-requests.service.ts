import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { environment } from '../../../environments/environment'

@Injectable({
    providedIn: 'root',
})
export class CheckoutRequestService {
    private readonly apiUrl = `${environment.apiUrl}/checkout-requests`

    constructor(private http: HttpClient) {}

    getCheckoutRequests(request: any = {}) {
        return this.http.get(this.apiUrl, {
            params: request,
        })
    }

    // getDetail(id: string) {
    //     return this.http.get<CheckoutRequestDetail>(`${this.apiUrl}/${id}`)
    // }

    getFields() {
        return this.http.get(`${this.apiUrl}/fields`)
    }

    create(request: any) {
        return this.http.post(this.apiUrl, request)
    }

    archive(id: string) {
        return this.http.delete<void>(`${this.apiUrl}/${id}`)
    }

    cancel(id: string) {
        return this.http.patch(`${this.apiUrl}/${id}/cancel`, {})
    }

    approve(id: string) {
        return this.http.patch<void>(`${this.apiUrl}/${id}/approve`, {})
    }

    reject(id: string) {
        return this.http.patch<void>(`${this.apiUrl}/${id}/reject`, {})
    }

    assignAsset(id: string, request: any) {
        return this.http.patch<void>(
        `${this.apiUrl}/${id}/assign-asset`,
        request
        )
    }

    return(id: string) {
        return this.http.patch<void>(`${this.apiUrl}/${id}/return`, {})
    }
}
