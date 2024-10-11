import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Instruction } from 'src/app/features/instruction/models/instruction.model';
import { InstructionService } from 'src/app/features/instruction/services/instruction.service';
import { Recipe } from 'src/app/features/recipe/models/recipe.model';
import { RecipeService } from 'src/app/features/recipe/services/recipe.service';

@Component({
  selector: 'app-recipe-page',
  templateUrl: './recipe-page.component.html',
  styleUrls: ['./recipe-page.component.css']
})
export class RecipePageComponent implements OnInit, OnDestroy {
  id: string | null = null;
  recipe?: Recipe;
  instruction: Instruction = {
    id: '',
    content: ''
  };

  getRecipeByIdSubscription?: Subscription;
  getInstructionByIdSubscription?: Subscription;

  constructor(
    private recipeService: RecipeService,
    private instructionService: InstructionService,
    private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');

    if (this.id) {
      this.getRecipeByIdSubscription = this.recipeService.getRecipeById(this.id).subscribe(recipe => {
        this.recipe = recipe;
        this.getInstructionByIdSubscription = this.instructionService.getInstructionById(this.recipe.instructionId).subscribe(instruction => {
          this.instruction = instruction;
        });
      });
    }
  }

  ngOnDestroy(): void {
    this.getRecipeByIdSubscription?.unsubscribe();
    this.getInstructionByIdSubscription?.unsubscribe();
  }
}
