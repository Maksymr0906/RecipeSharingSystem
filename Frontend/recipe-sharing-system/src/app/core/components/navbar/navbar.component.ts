import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from 'src/app/features/auth/models/user.model';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { Category } from 'src/app/features/category/models/category.model';
import { CategoryService } from 'src/app/features/category/services/category.service';
import { RecipeService } from 'src/app/features/recipe/services/recipe.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  categories$?: Observable<Category[]>;
  user?: User;
  slug?: string;

  constructor(
    private categoryService: CategoryService,
    private router: Router,
    private authService: AuthService,
    private recipeService: RecipeService) { }

  ngOnInit(): void {
    this.authService.user().subscribe(response => {
      this.user = response;
    });

    this.user = this.authService.getUser();
    this.categories$ = this.categoryService.getAllCategories();

    this.recipeService.getRandomRecipes(1).subscribe(response => {
      this.slug = response.at(0)?.slug;
    })
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
