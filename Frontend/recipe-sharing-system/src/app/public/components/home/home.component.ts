import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Recipe } from 'src/app/features/recipe/models/recipe.model';
import { RecipeService } from 'src/app/features/recipe/services/recipe.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  recipes$?: Observable<Recipe[]>;

  categories = [
    { name: 'Breakfasts', featuredImageUrl: '/assets/images/breakfast-category.jpg', slug: 'breakfast-category' },
    { name: 'Drinks', featuredImageUrl: '/assets/images/drinks-category.jpg', slug: 'drinks-category' },
    { name: 'Salads', featuredImageUrl: '/assets/images/salads-category.jpg', slug: 'salads-category' },
    { name: 'Bread', featuredImageUrl: '/assets/images/bread-category.jpg', slug: 'bread-category' },
    { name: 'Lunches', featuredImageUrl: '/assets/images/lunch-category.jpg', slug: 'lunch-category' },
  ];

  constructor(private recipeService: RecipeService) {

  }

  ngOnInit(): void {
    this.recipes$ = this.recipeService.getRandomRecipes(5);
  }
}
