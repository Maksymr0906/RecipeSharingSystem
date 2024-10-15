import { Component, OnDestroy, OnInit } from '@angular/core';
import { ImageService } from 'src/app/shared/services/image.service';
import { Category } from '../../category/models/category.model';
import { Observable, Subscription } from 'rxjs';
import { EditRecipeRequest } from '../models/edit-recipe-request.model';
import { EditInstructionRequest } from '../../instruction/models/edit-instruction-request.model';
import { RecipeService } from '../services/recipe.service';
import { CategoryService } from '../../category/services/category.service';
import { InstructionService } from '../../instruction/services/instruction.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IngredientQuantity } from '../models/ingredient-quantity.model';

@Component({
  selector: 'app-edit-recipe',
  templateUrl: './edit-recipe.component.html',
  styleUrls: ['./edit-recipe.component.css']
})
export class EditRecipeComponent implements OnInit, OnDestroy {
  categories$?: Observable<Category[]>;
  ingredients: IngredientQuantity[] = [];
  recipeModel: EditRecipeRequest;
  instructionModel: EditInstructionRequest;
  isImageSelectorVisible: boolean = false;
  imageSelectorSubscription?: Subscription;
  updateRecipeSubscription?: Subscription;
  updateInstructionSubscription?: Subscription;
  getRecipeByIdSubscription?: Subscription;
  getInstructionByIdSubscription?: Subscription;
  instructionId: string | null = null;
  recipeId: string | null = null;

  constructor(
    private imageService: ImageService,
    private recipeService: RecipeService,
    private categoryService: CategoryService,
    private instructionService: InstructionService,
    private router: Router,
    private route: ActivatedRoute
  )
  {
    this.recipeModel = {
      title: '',
      shortDescription: '',
      instructionId: '',
      featuredImageUrl: '',
      categoryIds: [],
      ingredients: []
    };

    this.instructionModel = {
      content: ''
    };
  }

  onFormSubmit() {
    if (this.instructionId) {
      this.updateInstructionSubscription = this.instructionService.updateInstruction(this.instructionId, this.instructionModel).subscribe(instruction => {        
        if (this.recipeId) {
          this.updateRecipeSubscription = this.recipeService.updateRecipe(this.recipeId, this.recipeModel).subscribe(
            _ => {
              this.router.navigateByUrl('/admin/recipes');
            }
          );
        }
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
    this.categories$ = this.categoryService.getAllCategories();

    this.imageSelectorSubscription = this.imageService.onSelectImage().subscribe({
      next: (response) => {
        this.recipeModel.featuredImageUrl = response.url;
        this.closeImageSelector();
      }
    })

    this.recipeId = this.route.snapshot.paramMap.get('id');
    if (this.recipeId) {
      this.getRecipeByIdSubscription = this.recipeService.getRecipeById(this.recipeId).subscribe(recipe => {
        this.recipeModel = recipe;
        this.instructionId = recipe.instructionId;
        this.ingredients = recipe.ingredients
        if (this.instructionId) {
          this.getInstructionByIdSubscription = this.instructionService.getInstructionById(this.instructionId).subscribe(instruction => {
            this.instructionModel = instruction;
          })
        }
      })
    }
  }

  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
    this.updateRecipeSubscription?.unsubscribe();
    this.updateInstructionSubscription?.unsubscribe();
    this.getRecipeByIdSubscription?.unsubscribe();
    this.getInstructionByIdSubscription?.unsubscribe();
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
