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

    create(request: any) {
        return this.http.post(this.apiUrl, request)
    }
}
