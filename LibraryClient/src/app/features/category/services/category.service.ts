import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../models/category';
import { API_URL } from '../../../app.config';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }

  getAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${API_URL}/Category/get-all-categories`);
  }

  addCategory(Categorydata: any): Observable<any> {
    return this.http.post(`${API_URL}/Category/add-category`, Categorydata);
  }

  deleteCategory(id: number): Observable<any> {
    return this.http.delete(`${API_URL}/Category/delete-category/${id}`);
  }
}
