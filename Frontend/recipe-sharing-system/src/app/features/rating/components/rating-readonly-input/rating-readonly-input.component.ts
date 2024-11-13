import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-rating-readonly-input',
  templateUrl: './rating-readonly-input.component.html',
  styleUrls: ['./rating-readonly-input.component.css']
})
export class RatingReadonlyInputComponent {
  readonly totalStars = 5;
  @Input() rating = 5;
}
