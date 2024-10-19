import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { Category } from 'src/app/features/category/models/category.model';
import { CategoryService } from 'src/app/features/category/services/category.service';
import { Recipe } from 'src/app/features/recipe/models/recipe.model';
import { RecipeService } from 'src/app/features/recipe/services/recipe.service';

@Component({
  selector: 'app-category-page',
  templateUrl: './category-page.component.html',
  styleUrls: ['./category-page.component.css']
})
export class CategoryPageComponent implements OnInit, OnDestroy {
  categoryId: string | null = null;
  category: Category = {
    id: '',
    name: '',
    slug: '',
    featuredImageUrl: ''
  };
  recipes$?: Observable<Recipe[]>;
  routeSubscription?: Subscription;

  constructor(
    private categoryService: CategoryService,
    private recipeService: RecipeService,
    private route: ActivatedRoute
  ) {

  }

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe(params => {
      this.categoryId = params.get('id')
      if (this.categoryId) {
        this.recipes$ = this.recipeService.getRecipesByCategoryId(this.categoryId);
        this.categoryService.getCategoryById(this.categoryId).subscribe(category => {
          this.category = category;
        })
      }
    })
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
  }
}
