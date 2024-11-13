import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-rating-display',
  templateUrl: './rating-display.component.html',
  styleUrls: ['./rating-display.component.css']
})
export class RatingDisplayComponent { 
  @Input() rating = 0;  
}
