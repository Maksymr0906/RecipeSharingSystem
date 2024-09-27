import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddRecipeRequest } from '../models/add-recipe-request.model';
import { Observable } from 'rxjs';
import { Recipe } from '../models/recipe.model';
import { EditRecipeRequest } from '../models/edit-recipe-request.model';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  constructor(private http: HttpClient) { }

  addRecipe(model: AddRecipeRequest): Observable<Recipe> {
    return this.http.post<Recipe>(`${environment.apiBaseUrl}/api/recipes`, model);
  }

  getAllRecipes(): Observable<Recipe[]> {
    return this.http.get<Recipe[]>(`${environment.apiBaseUrl}/api/recipes`);
  }

  getRecipeById(id: string, includeDetails: boolean = false): Observable<Recipe> {
    const url = `${environment.apiBaseUrl}/api/recipes/${id}?includeDetails=${includeDetails}`;
    return this.http.get<Recipe>(url);
  }

  updateRecipe(id: string, model: EditRecipeRequest): Observable<Recipe> {
    return this.http.put<Recipe>(`${environment.apiBaseUrl}/api/recipes/${id}`, model);
  }
}
