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

  constructor(private recipeService: RecipeService) {

  }

  ngOnInit(): void {
    this.recipes$ = this.recipeService.getRandomRecipes(5);
  }
}
