import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ReviewService } from '../../services/review.service';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { Subscription } from 'rxjs';
import { Review } from '../../models/review.model';
import { AddReviewRequest } from '../../models/add-review-request.model';
import { UpdateReviewRequest } from '../../models/update-review-request.model';

@Component({
  selector: 'app-add-review',
  templateUrl: './add-review.component.html',
  styleUrls: ['./add-review.component.css']
})
export class AddReviewComponent implements OnInit, OnDestroy {
  @Input() recipeId: string = '';
  @Input() recipeTitle: string = '';

  isEditing: boolean = true;
  
  rating: number = 0;
  content: string = '';

  user = this.authService.getUser();

  reviewModel: Review = {
    id: '',
    rating: 0,
    dateCreated: new Date(),
    content: '',
    userName: '',
    userId: '',
    recipeId: ''
  }

  addReviewSubscription?: Subscription;
  updateReviewSubscription?: Subscription;

  constructor(
    private reviewService: ReviewService,
    private authService: AuthService
  ) { }
  
  ngOnInit(): void {
    if (this.user) {
      this.reviewService.getUserRecipeReview(this.recipeId, this.user.userId).subscribe(review => {
        this.reviewModel = review;
        this.rating = review.rating;

        if (review.content) {
          this.content = review.content;
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.addReviewSubscription?.unsubscribe();
    this.updateReviewSubscription?.unsubscribe();
  }
  
  onCancel() {
    this.rating = 0;
    this.content = '';
  }

  onFormSubmit() {
    if (this.user) {
      if (this.reviewModel.rating === 0) {
        const model: AddReviewRequest = {
          rating: this.rating,
          content: this.content,
          recipeId: this.recipeId,
          userId: this.user.userId
        };

        this.addReviewSubscription = this.reviewService.addReview(model).subscribe();
      }
      else {
        const model: UpdateReviewRequest = {
          rating: this.rating,
          content: this.content
        };

        this.updateReviewSubscription = this.reviewService.updateReview(this.reviewModel.id, model).subscribe();
      }
    }
  }
}
