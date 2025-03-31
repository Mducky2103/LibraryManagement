import { HttpClient, HttpParams } from '@angular/common/http';
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
    return this.http.get(`${API_URL}/Book/get-book-image/${imageName}`, { responseType: 'blob' });
  }

  getAllBooks(page: number = 1, pageSize: number = 5): Observable<Book[]> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<Book[]>(`${API_URL}/Book/get-all-books`, { params });
  }

  addBook(bookData: any): Observable<any> {
    return this.http.post(`${API_URL}/Book/add-book`, bookData);
  }

  deleteBook(id: number): Observable<any> {
    return this.http.delete(`${API_URL}/Book/delete-book/${id}`);
  }

  getBookById(id: number): Observable<Book> {
    return this.http.get<Book>(`${API_URL}/Book/get-book-by-id/${id}`);
  }

  updateBook(id: number, formData: FormData): Observable<any> {
    return this.http.put<any>(`${API_URL}/Book/update-book/${id}`, formData);
  }

  searchBooks(searchTerm: string): Observable<Book[]> {
    return this.http.get<Book[]>(`${API_URL}/Book/search?searchTerm=${searchTerm}`);
  }

  getBooksByCategory(categoryId: number): Observable<Book[]> {
    return this.http.get<Book[]>(`${API_URL}/Book/by-category/${categoryId}`);
  }

  getBooksByAuthor(authorId: number): Observable<Book[]> {
    return this.http.get<Book[]>(`${API_URL}/Book/by-author/${authorId}`);
  }
}
