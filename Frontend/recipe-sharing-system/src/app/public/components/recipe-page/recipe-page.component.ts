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
  slug: string | null = null;
  recipe?: Recipe;
  instruction: Instruction = {
    id: '',
    content: ''
  };

  getRecipeBySlugSubscription?: Subscription;
  getInstructionByIdSubscription?: Subscription;

  constructor(
    private recipeService: RecipeService,
    private instructionService: InstructionService,
    private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.slug = this.route.snapshot.paramMap.get('slug');

    if (this.slug) {
      this.getRecipeBySlugSubscription = this.recipeService.getRecipeBySlug(this.slug).subscribe(recipe => {
        this.recipe = recipe;
        this.getInstructionByIdSubscription = this.instructionService.getInstructionById(this.recipe.instructionId).subscribe(instruction => {
          this.instruction = instruction;
        });
      });
    }
  }

  ngOnDestroy(): void {
    this.getRecipeBySlugSubscription?.unsubscribe();
    this.getInstructionByIdSubscription?.unsubscribe();
  }
}
