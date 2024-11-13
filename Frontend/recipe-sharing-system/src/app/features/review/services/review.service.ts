import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddReviewRequest } from '../models/add-review-request.model';
import { Observable } from 'rxjs';
import { Review } from '../models/review.model';
import { environment } from 'src/environments/environment.development';
import { UpdateReviewRequest } from '../models/update-review-request.model';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {

  constructor(private http: HttpClient) { }

  addReview(model: AddReviewRequest): Observable<Review> {
    return this.http.post<Review>(`${environment.apiBaseUrl}/api/reviews`, model);
  }

  getUserRecipeReview(recipeId: string, userId: string): Observable<Review> {
    return this.http.get<Review>(`${environment.apiBaseUrl}/api/reviews/recipes/${recipeId}/users/${userId}`);
  }

  updateReview(id: string, model: UpdateReviewRequest): Observable<Review> {
    return this.http.put<Review>(`${environment.apiBaseUrl}/api/reviews/${id}`, model);
  }

  getRecipeReviews(recipeId: string): Observable<Review[]> {
    return this.http.get<Review[]>(`${environment.apiBaseUrl}/api/reviews/recipe/${recipeId}`);
  }
}
