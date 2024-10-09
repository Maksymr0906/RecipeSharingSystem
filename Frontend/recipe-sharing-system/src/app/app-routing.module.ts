import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryListComponent } from './features/category/category-list/category-list.component';
import { AddCategoryComponent } from './features/category/add-category/add-category.component';
import { EditCategoryComponent } from './features/category/edit-category/edit-category.component';
import { IngredientListComponent } from './features/ingredient/ingredient-list/ingredient-list.component';
import { AddIngredientComponent } from './features/ingredient/add-ingredient/add-ingredient.component';
import { EditIngredientComponent } from './features/ingredient/edit-ingredient/edit-ingredient.component';
import { RecipeListComponent } from './features/recipe/recipe-list/recipe-list.component';
import { AddRecipeComponent } from './features/recipe/add-recipe/add-recipe.component';
import { EditRecipeComponent } from './features/recipe/edit-recipe/edit-recipe.component';
import { HomeComponent } from './public/components/home/home.component';
import { RecipeBlogComponent } from './public/components/recipe-blog/recipe-blog.component';

const routes: Routes = [
  {
    path: 'admin/categories',
    component: CategoryListComponent
  },
  {
    path: 'admin/categories/add',
    component: AddCategoryComponent
  },
  {
    path: 'admin/categories/:id',
    component: EditCategoryComponent
  },
  {
    path: 'admin/ingredients',
    component: IngredientListComponent
  },
  {
    path: 'admin/ingredients/add',
    component: AddIngredientComponent
  },
  {
    path: 'admin/ingredients/:id',
    component: EditIngredientComponent
  },
  {
    path: 'admin/recipes',
    component: RecipeListComponent
  },
  {
    path: 'admin/recipes/add',
    component: AddRecipeComponent
  },
  {
    path: 'admin/recipes/:id',
    component: EditRecipeComponent
  },
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'recipe/:id',
    component: RecipeBlogComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
