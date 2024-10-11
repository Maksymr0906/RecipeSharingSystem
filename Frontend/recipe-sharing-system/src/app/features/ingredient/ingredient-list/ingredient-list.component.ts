import { Component, OnInit } from '@angular/core';
import { IngredientService } from '../services/ingredient.service';
import { Observable } from 'rxjs';
import { Ingredient } from '../models/ingredient.model';

@Component({
  selector: 'app-ingredient-list',
  templateUrl: './ingredient-list.component.html',
  styleUrls: ['./ingredient-list.component.css']
})
export class IngredientListComponent implements OnInit {
  ingredients$?: Observable<Ingredient[]>;

  constructor(private ingredientService: IngredientService) {

  }

  ngOnInit(): void {
    this.ingredients$ = this.ingredientService.getAllIngredients();
  }

  onDelete(ingredient: Ingredient): void {
    this.ingredientService.deleteIngredient(ingredient.id).subscribe(_ => {
      this.ingredients$ = this.ingredientService.getAllIngredients();
    })
  }
}
