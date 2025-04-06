import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { API_URL } from "../../../app.config";
import { Penalty } from "../model/penalty";

@Injectable({
    providedIn: 'root'
})

export class PenaltyService {
    constructor(private http: HttpClient) { }

    getAllPenalties(): Observable<Penalty[]> {
        return this.http.get<Penalty[]>(`${API_URL}/Penalty/all-penalties`);
    }
    getPenaltyById(id: string): Observable<Penalty[]> {
        return this.http.get<Penalty[]>(`${API_URL}/Penalty/user-penalties/${id}`);
    }
}