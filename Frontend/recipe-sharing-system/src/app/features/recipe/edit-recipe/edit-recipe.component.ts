import { Component, OnDestroy, OnInit } from '@angular/core';
import { ImageService } from 'src/app/shared/services/image.service';
import { Category } from '../../category/models/category.model';
import { Observable, Subscription } from 'rxjs';
import { Ingredient } from '../../ingredient/models/ingredient.model';
import { EditRecipeRequest } from '../models/edit-recipe-request.model';
import { EditInstructionRequest } from '../../instruction/models/edit-instruction-request.model';
import { RecipeService } from '../services/recipe.service';
import { CategoryService } from '../../category/services/category.service';
import { IngredientService } from '../../ingredient/services/ingredient.service';
import { InstructionService } from '../../instruction/services/instruction.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-edit-recipe',
  templateUrl: './edit-recipe.component.html',
  styleUrls: ['./edit-recipe.component.css']
})
export class EditRecipeComponent implements OnInit, OnDestroy {
  categories$?: Observable<Category[]>;
  ingredients$?: Observable<Ingredient[]>;
  ingredientQuantities: { [key: string]: number } = {};
  selectedIngredients: string[] = [];
  recipeModel: EditRecipeRequest;
  ingredientsModel: Ingredient[] = [];
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
    private ingredientService: IngredientService,
    private instructionService: InstructionService,
    private router: Router,
    private route: ActivatedRoute
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
    this.ingredients$ = this.ingredientService.getAllIngredients();

    this.imageSelectorSubscription = this.imageService.onSelectImage().subscribe({
      next: (response) => {
        this.recipeModel.imageId = response.id;
        this.closeImageSelector();
      }
    })

    this.recipeId = this.route.snapshot.paramMap.get('id');
    if (this.recipeId) {
      this.getRecipeByIdSubscription = this.recipeService.getRecipeById(this.recipeId, true).subscribe(recipe => {
        this.recipeModel = recipe;
        this.instructionId = recipe.instructionId;
        if (this.instructionId) {
          this.getInstructionByIdSubscription = this.instructionService.getInstructionById(this.instructionId).subscribe(instruction => {
            this.instructionModel = instruction;
          })
        }

        this.selectedIngredients = recipe.ingredients.map(ing => ing.ingredientId);
        recipe.ingredients.forEach(ingredient => {
          this.ingredientQuantities[ingredient.ingredientId] = ingredient.quantity;
        });
      })
    }
  }

  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
    this.updateRecipeSubscription?.unsubscribe();
    this.updateInstructionSubscription?.unsubscribe();
    this.getRecipeByIdSubscription?.unsubscribe();
    this.getInstructionByIdSubscription?.unsubscribe();
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
