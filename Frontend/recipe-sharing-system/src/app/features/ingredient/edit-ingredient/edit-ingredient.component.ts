import { Component, OnDestroy, OnInit } from '@angular/core';
import { EditIngredientRequest } from '../models/edit-ingredient.model';
import { Subscription } from 'rxjs';
import { ImageService } from 'src/app/shared/services/image.service';
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
  isImageSelectorVisible: boolean = false;
  imageSelectorSubscription?: Subscription;
  updateIngredientSubscription?: Subscription;
  getIngredientByIdSubscription?: Subscription;

  constructor(
    private imageService: ImageService,
    private ingredientService: IngredientService,
    private router: Router,
    private route: ActivatedRoute
  )
  {
    this.model = {
      name: '',
      imageId: ''
    };
  }

  onFormSubmit() {
    if (this.id) {
      this.updateIngredientSubscription = this.ingredientService.updateIngredient(this.id, this.model).subscribe(_ => {
        this.router.navigateByUrl('admin/ingredients');
      });
    }
  }

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) {
      this.getIngredientByIdSubscription = this.ingredientService.getIngredientById(this.id).subscribe(ingredient => {
        this.model = ingredient;
      })
    }
   
    this.imageSelectorSubscription = this.imageService.onSelectImage().subscribe({
      next: (response) => {
        this.model.imageId = response.id;
        this.closeImageSelector();
      }
    })
  }

  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
    this.updateIngredientSubscription?.unsubscribe();
    this.getIngredientByIdSubscription?.unsubscribe();
  }
}
