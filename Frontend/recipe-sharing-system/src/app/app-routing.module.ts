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
import { RecipePageComponent } from './public/components/recipe-page/recipe-page.component';
import { CategoryPageComponent } from './public/components/category-page/category-page.component';
import { LoginPageComponent } from './public/components/login-page/login-page.component';
import { RegistrationPageComponent } from './public/components/registration-page/registration-page.component';
import { authGuard } from './features/auth/guards/auth.guard';
import { AboutUsPageComponent } from './public/components/about-us-page/about-us-page.component';
import { AddRecipeGuidePageComponent } from './public/components/add-recipe-guide-page/add-recipe-guide-page.component';
import { FavoriteRecipesListComponent } from './features/favorite-recipes/components/favorite-recipes-list/favorite-recipes-list.component';
import { PersonalProfilePageComponent } from './public/components/personal-profile-page/personal-profile-page.component';

const routes: Routes = [
  {
    path: 'admin/categories',
    component: CategoryListComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/categories/add',
    component: AddCategoryComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/categories/:id',
    component: EditCategoryComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/ingredients',
    component: IngredientListComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/ingredients/add',
    component: AddIngredientComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/ingredients/:id',
    component: EditIngredientComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/recipes',
    component: RecipeListComponent,
    canActivate: [authGuard]
  },
  {
    path: 'recipes/add',
    component: AddRecipeComponent
  },
  {
    path: 'admin/recipes/:id',
    component: EditRecipeComponent,
    canActivate: [authGuard]
  },
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'recipe/:slug',
    component: RecipePageComponent,
  },
  {
    path: 'category/:slug',
    component: CategoryPageComponent
  },
  {
    path: 'login',
    component: LoginPageComponent
  },
  {
    path: 'registration',
    component: RegistrationPageComponent
  },
  {
    path: 'about-us',
    component: AboutUsPageComponent
  },
  {
    path: 'add-recipe-guide',
    component: AddRecipeGuidePageComponent
  },
  {
    path: 'favorites',
    component: FavoriteRecipesListComponent
  },
  {
    path: 'personal-profile',
    component: PersonalProfilePageComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
