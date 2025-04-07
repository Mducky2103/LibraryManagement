import { HttpClient, HttpParams } from '@angular/common/http';
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
  getBorrowRequestList(): Observable<any> {
    return this.http.get(`${API_URL}/Borrow/pending-borrow-requests`);
  }
  approveRequest(id: number): Observable<any> {
    return this.http.put(`${API_URL}/Borrow/approve-borrow-book/${id}`, {});
  }
  rejectBorrowRequest(detailId: number, notes: string): Observable<any> {
    const params = new HttpParams().set('notes', notes);
    return this.http.put<any>(`${API_URL}/borrow/cancel/${detailId}`, null, { params });
  }
  getExtendRequestList(): Observable<any> {
    return this.http.get(`${API_URL}/Borrow/extend-requests`);
  }
  approveExtendDueDate(detailId: number, isApproved: boolean, notes: string): Observable<any> {
    const params = new HttpParams()
      .set('isApproved', isApproved)
      .set('notes', notes);

    return this.http.put(`${API_URL}/borrow/approve-extend-due-date/${detailId}`, null, { params });
  }
  getOverdueList(): Observable<any> {
    return this.http.get(`${API_URL}/Borrow/overdue-books-list`);
  }
  getOverdueBooksByUser(userId: string): Observable<any> {
    return this.http.get(`${API_URL}/borrow/overdue-books/${userId}`);
  }
  returnOverdueBookWithPenalty(detailId: number): Observable<any> {
    return this.http.put(`${API_URL}/borrow/return-book-and-apply-penalty/${detailId}`, {});
  }
  getAllBorrowedBooks(): Observable<any> {
    return this.http.get(`${API_URL}/borrow/all-borrowed-book`);
  }
}
