import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { AddRecipeRequest } from '../models/add-recipe-request.model';
import { ImageService } from 'src/app/shared/services/image.service';
import { RecipeService } from '../services/recipe.service';
import { Router } from '@angular/router';
import { Category } from '../../category/models/category.model';
import { CategoryService } from '../../category/services/category.service';
import { AddInstructionRequest } from '../../instruction/models/add-instruction-request.model';
import { InstructionService } from '../../instruction/services/instruction.service';
import { IngredientQuantity } from '../models/ingredient-quantity.model';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'app-add-recipe',
  templateUrl: './add-recipe.component.html',
  styleUrls: ['./add-recipe.component.css']
})
export class AddRecipeComponent implements OnInit, OnDestroy {
  categories$?: Observable<Category[]>;
  ingredients: IngredientQuantity[] = [];
  recipeModel: AddRecipeRequest;
  instructionModel: AddInstructionRequest;
  isImageSelectorVisible: boolean = false;
  imageSelectorSubscription?: Subscription;
  addRecipeSubscription?: Subscription;
  addInstructionSubscription?: Subscription;

  user = this.authService.getUser();

  constructor(
    private imageService: ImageService,
    private recipeService: RecipeService,
    private categoryService: CategoryService,
    private instructionService: InstructionService,
    private authService: AuthService,
    private router: Router
  ) {
    this.recipeModel = {
      title: '',
      shortDescription: '',
      slug: '',
      instructionId: '',
      authorId: '',
      featuredImageUrl: '',
      categoryIds: [],
      ingredients: []
    };

    this.instructionModel = {
      content: ''
    };
  }

  onFormSubmit() {
    this.addInstructionSubscription = this.instructionService.addInstruction(this.instructionModel).subscribe(instruction => {
      this.recipeModel.instructionId = instruction.id;
      if (this.user) {
        this.recipeModel.authorId = this.user?.userId;
      }
      this.recipeModel.ingredients = this.ingredients;
      console.log(this.recipeModel.ingredients);
      this.addRecipeSubscription = this.recipeService.addRecipe(this.recipeModel).subscribe(
        _ => {
          this.router.navigateByUrl('/');
        }
      );
    });
  }

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();

    this.imageSelectorSubscription = this.imageService.onSelectImage().subscribe({
      next: (response) => {
        this.recipeModel.featuredImageUrl = response.url;
        this.closeImageSelector();
      }
    })
  }

  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
    this.addRecipeSubscription?.unsubscribe();
    this.addInstructionSubscription?.unsubscribe();
    this.imageService.resetSelectedImage();
  }

  addIngredient(): void {
    this.ingredients.push({ ingredientName: '', quantity: 0, measurementUnit: '' });
  }

  removeIngredient(index: number): void {
    this.ingredients.splice(index, 1);
  }

  updateIngredientName(index: number, name: string): void {
    this.ingredients[index].ingredientName = name;
  }

  updateIngredientQuantity(index: number, quantity: number): void {
    this.ingredients[index].quantity = quantity;
  }

  updateIngredientMeasurementUnit(index: number, unit: string): void {
    this.ingredients[index].measurementUnit = unit;
  }
}
