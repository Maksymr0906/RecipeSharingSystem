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

  getRandomRecipes(count: number): Observable<Recipe[]> {
    return this.http.get<Recipe[]>(`${environment.apiBaseUrl}/api/recipes/random?count=${count}`);
  }

  getRecipeById(id: string): Observable<Recipe> {
    const url = `${environment.apiBaseUrl}/api/recipes/${id}`;
    return this.http.get<Recipe>(url);
  }

  updateRecipe(id: string, model: EditRecipeRequest): Observable<Recipe> {
    return this.http.put<Recipe>(`${environment.apiBaseUrl}/api/recipes/${id}`, model);
  }

  deleteRecipe(id: string): Observable<Recipe> {
    return this.http.delete<Recipe>(`${environment.apiBaseUrl}/api/recipes/${id}`);
  }

  getRecipesByCategoryId(categoryId: string): Observable<Recipe[]> {
    return this.http.get<Recipe[]>(`${environment.apiBaseUrl}/api/recipes/category/${categoryId}`);
  }

  getRecipeBySlug(slug: string): Observable<Recipe> {
    return this.http.get<Recipe>(`${environment.apiBaseUrl}/api/recipes/slug/${slug}`);
  }

  getSearchRecipes(query: string): Observable<Recipe[]> {
    return this.http.get<Recipe[]>(`${environment.apiBaseUrl}/api/recipes/search?query=${query}`);
  }

  getAuthoredRecipes(authorId: string): Observable<Recipe[]> {
    return this.http.get<Recipe[]>(`${environment.apiBaseUrl}/api/recipes/author/${authorId}`);
  }
}
