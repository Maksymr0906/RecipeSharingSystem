import { Component, OnDestroy } from '@angular/core';
import { AddIngredientRequest } from '../models/add-ingredient-request.model';
import { Subscription } from 'rxjs';
import { IngredientService } from '../services/ingredient.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-ingredient',
  templateUrl: './add-ingredient.component.html',
  styleUrls: ['./add-ingredient.component.css']
})
export class AddIngredientComponent implements OnDestroy {
  model: AddIngredientRequest;
  addIngredientSubscription?: Subscription;

  constructor(
    private ingredientService: IngredientService,
    private router: Router
  )
  {
    this.model = {
      name: '',
    };
  }

  onFormSubmit() {
    this.addIngredientSubscription = this.ingredientService.addIngredient(this.model).subscribe(
      _ => {
        this.router.navigateByUrl('/admin/ingredients');
      }
    );
  }

  ngOnDestroy(): void {
    this.addIngredientSubscription?.unsubscribe();
  }
}
