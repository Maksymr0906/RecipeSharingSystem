import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Recipe } from 'src/app/features/recipe/models/recipe.model';
import { FavoriteRecipesService } from '../../services/favorite-recipes.service';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { User } from 'src/app/features/auth/models/user.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-favorite-recipes-list',
  templateUrl: './favorite-recipes-list.component.html',
  styleUrls: ['./favorite-recipes-list.component.css']
})
export class FavoriteRecipesListComponent implements OnInit {
  recipes$?: Observable<Recipe[]>;
  user?: User;

  constructor(
    private favoriteService: FavoriteRecipesService,
    private authService: AuthService,
    private router: Router
  ) {

  }

  ngOnInit(): void {
    this.authService.user().subscribe(response => {
      this.user = response;
    });

    this.user = this.authService.getUser();
    if (this.user) {
      this.recipes$ = this.favoriteService.getFavoriteRecipesForUser(this.user?.userId);
    }
    else {
      this.router.navigateByUrl('login');
    }
  }
}
