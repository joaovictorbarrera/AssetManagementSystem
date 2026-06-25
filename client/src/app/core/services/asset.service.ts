import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { environment } from '../../../environments/environment'

@Injectable({
    providedIn: 'root',
})
export class AssetService {
    private readonly apiUrl = `${environment.apiUrl}/assets`

    constructor(private http: HttpClient) {}

    getAssets(request: any = {}) {
        return this.http.get(this.apiUrl, {
            params: request,
        })
    }

    getFields() {
        return this.http.get(`${this.apiUrl}/fields`)
    }

    updateStatus(id: string, status: string) {
        return this.http.patch(`${this.apiUrl}/${id}/status`, { status })
    }

    updateCondition(id: string, condition: string) {
        return this.http.patch(`${this.apiUrl}/${id}/condition`, { condition })
    }
}
