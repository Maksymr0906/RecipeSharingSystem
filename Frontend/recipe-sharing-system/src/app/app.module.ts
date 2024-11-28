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
import { AboutUsPageComponent } from './public/components/about-us-page/about-us-page.component';
import { AddRecipeGuidePageComponent } from './public/components/add-recipe-guide-page/add-recipe-guide-page.component';
import { FavoriteRecipesListComponent } from './features/favorite-recipes/components/favorite-recipes-list/favorite-recipes-list.component';
import { FavoriteToggleButtonComponent } from './features/favorite-recipes/components/favorite-toggle-button/favorite-toggle-button.component';
import { FooterComponent } from './core/components/footer/footer.component';
import { PersonalProfilePageComponent } from './public/components/personal-profile-page/personal-profile-page.component';
import { AllRecipesPageComponent } from './public/components/all-recipes-page/all-recipes-page.component';
import { SearchRecipesComponent } from './features/recipe/search-recipes/search-recipes.component';
import { DisplayRecipesPageComponent } from './public/components/display-recipes-page/display-recipes-page.component';
import { AuthoredRecipesPageComponent } from './public/components/authored-recipes-page/authored-recipes-page.component';

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
    AboutUsPageComponent,
    AddRecipeGuidePageComponent,
    FavoriteRecipesListComponent,
    FavoriteToggleButtonComponent,
    FooterComponent,
    PersonalProfilePageComponent,
    AllRecipesPageComponent,
    SearchRecipesComponent,
    DisplayRecipesPageComponent,
    AuthoredRecipesPageComponent,
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
