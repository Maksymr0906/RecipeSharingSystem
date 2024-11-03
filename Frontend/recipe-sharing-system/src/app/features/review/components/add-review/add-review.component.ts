import { Component, Input, OnInit } from '@angular/core';
import { RatingService } from 'src/app/features/rating/services/rating.service';
import { ReviewService } from '../../services/review.service';
import { AuthService } from 'src/app/features/auth/services/auth.service';

@Component({
  selector: 'app-add-review',
  templateUrl: './add-review.component.html',
  styleUrls: ['./add-review.component.css']
})
export class AddReviewComponent implements OnInit {
  @Input() recipeId: string = '';
  @Input() recipeTitle: string = '';
  rating: number = 0;
  content: string = '';
  user = this.authService.getUser();
  
  constructor(
    private ratingService: RatingService,
    private reviewService: ReviewService,
    private authService: AuthService
  ) { }
  
  ngOnInit(): void {
    
  }

  onCancel() {
    this.rating = 0;
    this.content = '';
  }

  onFormSubmit() {

  }

  submitRating() {

  }

  submitReview() {

  }
}
