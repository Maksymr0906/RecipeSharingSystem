import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
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
import { CategoryPageComponent } from './public/components/category-page/category-page.component';
import { LoginPageComponent } from './public/components/login-page/login-page.component';
import { RegistrationPageComponent } from './public/components/registration-page/registration-page.component';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import { RangePipe } from './shared/pipes/range.pipe';
import { RatingInputComponent } from './features/rating/components/rating-input/rating-input.component';
import { AddReviewComponent } from './features/review/components/add-review/add-review.component';
import { RatingDisplayComponent } from './features/rating/components/rating-display/rating-display.component';
import { ReviewListComponent } from './features/review/components/review-list/review-list.component';
import { RatingReadonlyInputComponent } from './features/rating/components/rating-readonly-input/rating-readonly-input.component';

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
    CategoryPageComponent,
    LoginPageComponent,
    RegistrationPageComponent,
    RangePipe,
    RatingInputComponent,
    AddReviewComponent,
    RatingDisplayComponent,
    ReviewListComponent,
    RatingReadonlyInputComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    MarkdownModule.forRoot(),
  ],
  providers: [{
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
