import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Book } from '../models/book';
import { API_URL } from '../../../app.config';

@Injectable({
  providedIn: 'root'
})
export class BooksService {

  constructor(private http: HttpClient) { }

  getBookImage(imageName: string): Observable<Blob> {
    return this.http.get(`${API_URL}/get-book-image/${imageName}`, { responseType: 'blob' });
  }

  getAllBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(`${API_URL}/Book/get-all-books`);
  }

  addBook(bookData: any): Observable<any> {
    return this.http.post(`${API_URL}/add-book`, bookData);
  }
}
