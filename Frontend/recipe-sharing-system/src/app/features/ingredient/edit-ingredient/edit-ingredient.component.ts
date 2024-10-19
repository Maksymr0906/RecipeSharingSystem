import { Component, OnDestroy, OnInit } from '@angular/core';
import { EditIngredientRequest } from '../models/edit-ingredient-request.model';
import { Subscription } from 'rxjs';
import { IngredientService } from '../services/ingredient.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-edit-ingredient',
  templateUrl: './edit-ingredient.component.html',
  styleUrls: ['./edit-ingredient.component.css']
})
export class EditIngredientComponent implements OnInit, OnDestroy {
  id: string | null = null;
  model: EditIngredientRequest;
  updateIngredientSubscription?: Subscription;
  getIngredientByIdSubscription?: Subscription;

  constructor(
    private ingredientService: IngredientService,
    private router: Router,
    private route: ActivatedRoute
  )
  {
    this.model = {
      name: '',
      slug: '',
    };
  }

  onFormSubmit() {
    if (this.id) {
      this.updateIngredientSubscription = this.ingredientService.updateIngredient(this.id, this.model).subscribe(_ => {
        this.router.navigateByUrl('admin/ingredients');
      });
    }
  }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) {
      this.getIngredientByIdSubscription = this.ingredientService.getIngredientById(this.id).subscribe(ingredient => {
        this.model = ingredient;
      })
    }
  }

  ngOnDestroy(): void {
    this.updateIngredientSubscription?.unsubscribe();
    this.getIngredientByIdSubscription?.unsubscribe();
  }
}
