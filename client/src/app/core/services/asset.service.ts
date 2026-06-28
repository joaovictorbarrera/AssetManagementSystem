import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { environment } from '../../../environments/environment'
import PaginatedResponse from '../DTOs/shared/paginated.response'
import { AssetDto } from '../DTOs/asset/asset.dto'

@Injectable({
    providedIn: 'root',
})
export class AssetService {
    private readonly apiUrl = `${environment.apiUrl}/assets`

    constructor(private http: HttpClient) {}

    getAssets(request: any = {}) {
        return this.http.get<PaginatedResponse<AssetDto>>(this.apiUrl, {
            params: request,
        })
    }

    getAvailable(category: string) {
        return this.http.get(`${this.apiUrl}/available`, {
            params: { category },
        })
    }

    // getDetail(id: string) {
    //     return this.http.get<AssetDetail>(`${this.apiUrl}/${id}`)
    // }

    getFields() {
        return this.http.get(`${this.apiUrl}/fields`)
    }

    // getHistory(id: string) {
    //     return this.http.get<AssetHistory[]>(`${this.apiUrl}/${id}/history`)
    // }

    create(request: any) {
        return this.http.post(this.apiUrl, request)
    }

    update(id: string, request: any) {
        return this.http.put<AssetDto>(`${this.apiUrl}/${id}`, request)
    }

    archive(id: string) {
        return this.http.delete<void>(`${this.apiUrl}/${id}`)
    }

    updateStatus(id: string, status: string) {
        return this.http.patch(`${this.apiUrl}/${id}/status`, { status })
    }

    updateCondition(id: string, condition: string) {
        return this.http.patch(`${this.apiUrl}/${id}/condition`, { condition })
    }
}
