import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryListComponent } from './features/category/category-list/category-list.component';
import { AddCategoryComponent } from './features/category/add-category/add-category.component';
import { EditCategoryComponent } from './features/category/edit-category/edit-category.component';
import { IngredientListComponent } from './features/ingredient/ingredient-list/ingredient-list.component';
import { AddIngredientComponent } from './features/ingredient/add-ingredient/add-ingredient.component';
import { EditIngredientComponent } from './features/ingredient/edit-ingredient/edit-ingredient.component';

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
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
