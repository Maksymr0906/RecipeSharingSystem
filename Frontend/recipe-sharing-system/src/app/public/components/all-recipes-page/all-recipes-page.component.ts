import { Component, OnInit } from '@angular/core';
import { Recipe } from 'src/app/features/recipe/models/recipe.model';
import { RecipeService } from 'src/app/features/recipe/services/recipe.service';

@Component({
  selector: 'app-all-recipes-page',
  templateUrl: './all-recipes-page.component.html',
  styleUrls: ['./all-recipes-page.component.css']
})
export class AllRecipesPageComponent implements OnInit {
  recipes: Recipe[] = [];

  constructor(private recipeService: RecipeService) {}

  ngOnInit(): void {
    this.recipeService.getAllRecipes().subscribe((recipes: Recipe[]) => {
      this.recipes = recipes;
    });
  }
}
