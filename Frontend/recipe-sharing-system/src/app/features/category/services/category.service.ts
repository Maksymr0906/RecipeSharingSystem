import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../../models/category.model';
import { environment } from 'src/environments/environment.development';
import { AddCategoryRequest } from '../../models/add-category-request.model';
import { EditCategoryRequest } from '../../models/edit-category-request.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  constructor(private http: HttpClient) { }

  addCategory(model: AddCategoryRequest): Observable<Category> {
    return this.http.post<Category>(`${environment.apiBaseUrl}/api/categories`, model);
  }

  getAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${environment.apiBaseUrl}/api/categories`);
  }

  getCategoryById(id: string): Observable<Category> {
    return this.http.get<Category>(`${environment.apiBaseUrl}/api/categories/${id}`);
  }

  updateCategory(id: string, model: EditCategoryRequest): Observable<Category> {
    return this.http.put<Category>(`${environment.apiBaseUrl}/api/categories/${id}`, model);
  }
}
