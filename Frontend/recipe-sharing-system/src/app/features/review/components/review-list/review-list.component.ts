import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Review } from '../../models/review.model';
import { ReviewService } from '../../services/review.service';

@Component({
  selector: 'app-review-list',
  templateUrl: './review-list.component.html',
  styleUrls: ['./review-list.component.css']
})
export class ReviewListComponent implements OnInit {
  @Input() recipeId?: string;

  reviews$?: Observable<Review[]>

  constructor(private reviewService: ReviewService) {

  }

  ngOnInit(): void {
    if (this.recipeId) {
      this.reviews$ = this.reviewService.getRecipeReviews(this.recipeId);
    }
  }
}
