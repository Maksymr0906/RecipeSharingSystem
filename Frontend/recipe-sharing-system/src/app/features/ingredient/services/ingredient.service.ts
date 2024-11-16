import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Ingredient } from '../models/ingredient.model';
import { environment } from 'src/environments/environment.development';
import { AddIngredientRequest } from '../models/add-ingredient-request.model';
import { EditIngredientRequest } from '../models/edit-ingredient-request.model';

@Injectable({
  providedIn: 'root'
})
export class IngredientService {

  constructor(private http: HttpClient) { }

  getAllIngredients(): Observable<Ingredient[]> {
    return this.http.get<Ingredient[]>(`${environment.apiBaseUrl}/api/ingredients`);
  }

  addIngredient(model: AddIngredientRequest): Observable<Ingredient> {
    return this.http.post<Ingredient>(`${environment.apiBaseUrl}/api/ingredients`, model);
  }

  getIngredientById(id: string): Observable<Ingredient> {
    return this.http.get<Ingredient>(`${environment.apiBaseUrl}/api/ingredients/${id}`);
  }

  updateIngredient(id: string, model: EditIngredientRequest): Observable<Ingredient> {
    return this.http.put<Ingredient>(`${environment.apiBaseUrl}/api/ingredients/${id}`, model);
  }

  deleteIngredient(id: string): Observable<Ingredient> {
    return this.http.delete<Ingredient>(`${environment.apiBaseUrl}/api/ingredients/${id}`);
  }

  getIngredientBySlug(slug: string): Observable<Ingredient> {
    return this.http.get<Ingredient>(`${environment.apiBaseUrl}/api/ingredients/slug/${slug}`);
  }
}
