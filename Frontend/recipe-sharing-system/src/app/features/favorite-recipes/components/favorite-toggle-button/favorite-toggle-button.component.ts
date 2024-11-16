import { Component, Input, OnInit } from '@angular/core';
import { FavoriteRecipesService } from '../../services/favorite-recipes.service';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { User } from 'src/app/features/auth/models/user.model';
import { RecipeStatus } from '../../models/recipe-status.model';

@Component({
  selector: 'app-favorite-toggle-button',
  templateUrl: './favorite-toggle-button.component.html',
  styleUrls: ['./favorite-toggle-button.component.css']
})
export class FavoriteToggleButtonComponent implements OnInit {
  @Input() recipeId!: string;

  recipeStatus: RecipeStatus = {
    isFavorite: false
  };

  user?: User;

  constructor(private favoriteService: FavoriteRecipesService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.user = this.authService.getUser();
    if (this.user) {
      this.favoriteService.isFavorite(this.user.userId, this.recipeId).subscribe(
        response => {
          this.recipeStatus = response;
        }
      );
    }
  }

  toggleFavorite() {
    if (this.user) {
      if (this.recipeStatus.isFavorite) {
        this.favoriteService.removeFromFavorites(this.user.userId, this.recipeId).subscribe(_ => {
          this.recipeStatus.isFavorite = false;
        })
      }
      else {
        this.favoriteService.addToFavorites(this.user.userId, this.recipeId).subscribe(_ => {
          this.recipeStatus.isFavorite = true;
        })
      }
    }
  }
}
