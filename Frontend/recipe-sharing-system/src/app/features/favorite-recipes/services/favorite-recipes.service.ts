import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Recipe } from '../../recipe/models/recipe.model';
import { environment } from 'src/environments/environment.development';
import { RecipeStatus } from '../models/recipe-status.model';

@Injectable({
  providedIn: 'root'
})
export class FavoriteRecipesService {

  constructor(private http: HttpClient) { }

  getFavoriteRecipesForUser(userId: string): Observable<Recipe[]> {
    return this.http.get<Recipe[]>(`${environment.apiBaseUrl}/api/users/${userId}/favorites`);
  }

  isFavorite(userId: string, recipeId: string): Observable<RecipeStatus> {
    return this.http.get<RecipeStatus>(`${environment.apiBaseUrl}/api/users/${userId}/favorites/recipes/${recipeId}/status`);
  }


  addToFavorites(userId: string, recipeId: string): Observable<void> {
    return this.http.post<void>(
      `${environment.apiBaseUrl}/api/users/${userId}/favorites/recipes/${recipeId}`,
      {}
    );
  }

  removeFromFavorites(userId: string, recipeId: string): Observable<void> {
    return this.http.delete<void>(
      `${environment.apiBaseUrl}/api/users/${userId}/favorites/recipes/${recipeId}`,
      {}
    );
  }
}
