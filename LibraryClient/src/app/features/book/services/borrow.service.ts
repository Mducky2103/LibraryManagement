import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_URL } from '../../../app.config';
import { Borrow } from '../models/db.model';

@Injectable({
  providedIn: 'root'
})
export class BorrowService {

  constructor(private http: HttpClient) {
  }
  borrow(formData: any): Observable<any> {
    return this.http.post(`${API_URL}/Borrow/request-borrow-book`, formData);
  }
  getBorrowHistoryByIdUser(id: number): Observable<Borrow[]> {
    return this.http.get<Borrow[]>(`${API_URL}/Borrow/borrow-history/${id}`);
  }
  getBorrowingingByIdUser(id: number): Observable<Borrow[]> {
    return this.http.get<Borrow[]>(`${API_URL}/Borrow/borrowed-books/${id}`);
  }
  returnBook(receiptDetailId: number): Observable<any> {
    return this.http.put(`${API_URL}/Borrow/return-book/${receiptDetailId}`, {});
  }
  dueDateborrow(id: any, note: any): Observable<any> {
    return this.http.post(`${API_URL}/Borrow/request-extend-due-date/${id}?notes=${note}`, id);
  }
}
