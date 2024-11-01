import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-rating-input',
  templateUrl: './rating-input.component.html',
  styleUrls: ['./rating-input.component.css']
})
export class RatingInputComponent {
  readonly totalStars = 5;
  @Input() rating = 5;
  @Output() ratingChange = new EventEmitter<number>();

  hoveredStar = 0;

  onStarHover(starPosition: number) {
    this.hoveredStar = starPosition + 1;
  }

  onStarLeave() {
    this.hoveredStar = 0;
  }

  onStarClick(starPosition: number) {
    this.rating = starPosition + 1;
    this.ratingChange.emit(this.rating);
  }
}
