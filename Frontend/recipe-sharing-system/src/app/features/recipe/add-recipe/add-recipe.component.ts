import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { AddRecipeRequest } from '../models/add-recipe-request.model';
import { ImageService } from 'src/app/shared/services/image.service';
import { RecipeService } from '../services/recipe.service';
import { Router } from '@angular/router';
import { Category } from '../../category/models/category.model';
import { CategoryService } from '../../category/services/category.service';
import { IngredientService } from '../../ingredient/services/ingredient.service';
import { AddInstructionRequest } from '../../instruction/models/add-instruction-request.model';
import { InstructionService } from '../../instruction/services/instruction.service';
import { Ingredient } from '../../ingredient/models/ingredient.model';
import { IngredientQuantity } from '../models/ingredient-quantity.model';

@Component({
  selector: 'app-add-recipe',
  templateUrl: './add-recipe.component.html',
  styleUrls: ['./add-recipe.component.css']
})
export class AddRecipeComponent implements OnInit, OnDestroy {
  categories$?: Observable<Category[]>;
  ingredients$?: Observable<Ingredient[]>;
  ingredientQuantities: { [key: string]: number } = {};
  selectedIngredients: string[] = [];
  recipeModel: AddRecipeRequest;
  ingredientsModel: IngredientQuantity[] = [];
  instructionModel: AddInstructionRequest;
  isImageSelectorVisible: boolean = false;
  imageSelectorSubscription?: Subscription;
  addRecipeSubscription?: Subscription;
  addInstructionSubscription?: Subscription;

  constructor(
    private imageService: ImageService,
    private recipeService: RecipeService,
    private categoryService: CategoryService,
    private ingredientService: IngredientService,
    private instructionService: InstructionService,
    private router: Router
  )
  {
    this.recipeModel = {
      title: '',
      shortDescription: '',
      instructionId: '',
      imageId: '',
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

      this.addRecipeSubscription = this.recipeService.addRecipe(this.recipeModel).subscribe(
        _ => {
          this.router.navigateByUrl('/admin/recipes');
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
    this.ingredients$ = this.ingredientService.getAllIngredients();

    this.imageSelectorSubscription = this.imageService.onSelectImage().subscribe({
      next: (response) => {
        this.recipeModel.imageId = response.id;
        this.closeImageSelector();
      }
    })
  }

  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
    this.addRecipeSubscription?.unsubscribe();
    this.addInstructionSubscription?.unsubscribe();
  }

  onIngredientSelect(ingredientId: string, event: any): void {
    if (event.target.checked) {
      this.selectedIngredients.push(ingredientId);
      this.ingredientQuantities[ingredientId] = 0;
    } else {
      this.selectedIngredients = this.selectedIngredients.filter(
        id => id !== ingredientId
      );
      delete this.ingredientQuantities[ingredientId];
    }
    this.updateRecipeIngredients();
  }

  onQuantityChange(ingredientId: string, quantity: number): void {
    this.ingredientQuantities[ingredientId] = quantity;
    this.updateRecipeIngredients();
  }

  isIngredientSelected(ingredientId: string): boolean {
    return this.selectedIngredients.includes(ingredientId);
  }

  updateRecipeIngredients(): void {
    this.recipeModel.ingredients = this.selectedIngredients.map(id => ({
      ingredientId: id,
      quantity: this.ingredientQuantities[id] || 0
    }));
  }
}
