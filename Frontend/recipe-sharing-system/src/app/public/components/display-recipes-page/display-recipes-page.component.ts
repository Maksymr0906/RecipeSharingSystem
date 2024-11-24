import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Recipe } from 'src/app/features/recipe/models/recipe.model';

@Component({
  selector: 'app-display-recipes-page',
  templateUrl: './display-recipes-page.component.html',
  styleUrls: ['./display-recipes-page.component.css']
})
export class DisplayRecipesPageComponent implements OnChanges {
  @Input() recipes: Recipe[] = [];
  @Input() pageSize: number = 12;

  currentPage: number = 1;
  totalRecipes: number = 0;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['recipes'] && changes['recipes'].currentValue) {
      this.totalRecipes = this.recipes.length;
    }
  }

  get paginatedRecipes(): Recipe[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    return this.recipes.slice(startIndex, endIndex);
  }

  get totalPages(): number {
    return Math.ceil(this.totalRecipes / this.pageSize);
  }

  nextPage(): void {
    if ((this.currentPage * this.pageSize) < this.totalRecipes) {
      this.currentPage++;
      window.scrollTo(0, 0);
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      window.scrollTo(0, 0);
    }
  }

  get canGoNext(): boolean {
    return this.currentPage * this.pageSize < this.totalRecipes;
  }

  get canGoPrevious(): boolean {
    return this.currentPage > 1;
  }
}
