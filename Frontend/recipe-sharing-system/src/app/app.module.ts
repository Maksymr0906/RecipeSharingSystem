import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { CategoryListComponent } from './features/category/category-list/category-list.component';
import { AddCategoryComponent } from './features/category/add-category/add-category.component';
import { ImageSelectorComponent } from './shared/components/image-selector/image-selector.component';
import { EditCategoryComponent } from './features/category/edit-category/edit-category.component';
import { IngredientListComponent } from './features/ingredient/ingredient-list/ingredient-list.component';
import { AddIngredientComponent } from './features/ingredient/add-ingredient/add-ingredient.component';
import { EditIngredientComponent } from './features/ingredient/edit-ingredient/edit-ingredient.component';
import { RecipeListComponent } from './features/recipe/recipe-list/recipe-list.component';
import { AddRecipeComponent } from './features/recipe/add-recipe/add-recipe.component';
import { EditRecipeComponent } from './features/recipe/edit-recipe/edit-recipe.component';
import { MarkdownModule } from 'ngx-markdown';
import { HomeComponent } from './public/components/home/home.component';
import { RecipeCardComponent } from './public/components/recipe-card/recipe-card.component';
import { RecipePageComponent } from './public/components/recipe-page/recipe-page.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    CategoryListComponent,
    AddCategoryComponent,
    ImageSelectorComponent,
    EditCategoryComponent,
    IngredientListComponent,
    AddIngredientComponent,
    EditIngredientComponent,
    RecipeListComponent,
    AddRecipeComponent,
    EditRecipeComponent,
    HomeComponent,
    RecipeCardComponent,
    RecipePageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    MarkdownModule.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
