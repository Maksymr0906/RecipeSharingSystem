import { Component } from '@angular/core';
import { RecipeService } from '../services/recipe.service';
import { Recipe } from '../models/recipe.model';

@Component({
  selector: 'app-search-recipes',
  templateUrl: './search-recipes.component.html',
  styleUrls: ['./search-recipes.component.css']
})
export class SearchRecipesComponent {
  query: string = '';
  recipes: Recipe[] = [];

  constructor(private recipeService: RecipeService) {
  }

  onFormSubmit() {
    this.recipeService.getSearchRecipes(this.query).subscribe(response => {
      this.recipes = response;
    })
  }
}
