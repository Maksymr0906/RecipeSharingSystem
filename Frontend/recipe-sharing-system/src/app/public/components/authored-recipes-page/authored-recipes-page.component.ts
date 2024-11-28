import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { Category } from 'src/app/features/category/models/category.model';
import { CategoryService } from 'src/app/features/category/services/category.service';
import { Recipe } from 'src/app/features/recipe/models/recipe.model';
import { RecipeService } from 'src/app/features/recipe/services/recipe.service';

@Component({
  selector: 'app-authored-recipes-page',
  templateUrl: './authored-recipes-page.component.html',
  styleUrls: ['./authored-recipes-page.component.css']
})
export class AuthoredRecipesPageComponent implements OnInit {
  recipes: Recipe[] = [];
  user = this.authService.getUser();

  constructor(
    private recipeService: RecipeService,
    private authService: AuthService) { }

  ngOnInit(): void {
    if (this.user) {
      this.recipeService.getAuthoredRecipes(this.user.userId).subscribe((recipes: Recipe[]) => {
        this.recipes = recipes;
      });
    }
  }

  onDelete(id: string): void {
    this.recipeService.deleteRecipe(id).subscribe();
  }
}
