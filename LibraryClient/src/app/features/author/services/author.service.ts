import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Author } from '../models/author';
import { API_URL } from '../../../app.config';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {

  constructor(private http: HttpClient) { }

  getAllAuthors(): Observable<Author[]> {
    return this.http.get<Author[]>(`${API_URL}/Author/get-all-authors`);
  }

  addAuthor(Authordata: any): Observable<any> {
    return this.http.post(`${API_URL}/Author/add-author`, Authordata);
  }

  deleteAuthor(id: number): Observable<any> {
    return this.http.delete(`${API_URL}/Author/delete-author/${id}`);
  }
}
