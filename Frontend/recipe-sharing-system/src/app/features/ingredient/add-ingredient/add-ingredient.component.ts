import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddIngredientRequest } from '../models/add-ingredient.model';
import { Subscription } from 'rxjs';
import { ImageService } from 'src/app/shared/services/image.service';
import { IngredientService } from '../services/ingredient.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-ingredient',
  templateUrl: './add-ingredient.component.html',
  styleUrls: ['./add-ingredient.component.css']
})
export class AddIngredientComponent implements OnInit, OnDestroy {
  model: AddIngredientRequest;
  isImageSelectorVisible: boolean = false;
  imageSelectorSubscription?: Subscription;
  addIngredientSubscription?: Subscription;

  constructor(
    private imageService: ImageService,
    private ingredientService: IngredientService,
    private router: Router
  )
  {
    this.model = {
      name: '',
      imageId: ''
    };
  }

  onFormSubmit() {
    this.addIngredientSubscription = this.ingredientService.addIngredient(this.model).subscribe(
      _ => {
        this.router.navigateByUrl('/admin/ingredients');
      }
    );
  }

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  ngOnInit(): void {
    this.imageSelectorSubscription = this.imageService.onSelectImage().subscribe({
      next: (response) => {
        this.model.imageId = response.id;
        this.closeImageSelector();
      }
    })
  }

  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
    this.addIngredientSubscription?.unsubscribe();
  }
}
