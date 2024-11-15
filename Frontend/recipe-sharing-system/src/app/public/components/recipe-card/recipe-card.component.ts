import { Component, Input, OnInit } from '@angular/core';
import { Category } from 'src/app/features/category/models/category.model';
import { CategoryService } from 'src/app/features/category/services/category.service';
import { Recipe } from 'src/app/features/recipe/models/recipe.model';

@Component({
  selector: 'app-recipe-card',
  templateUrl: './recipe-card.component.html',
  styleUrls: ['./recipe-card.component.css']
})
export class RecipeCardComponent implements OnInit {
  @Input() recipe: Recipe = {
      id: '',
      title: '',
      shortDescription: '',
      slug: '',
      featuredImageUrl: '',
      instructionId: '',
      authorId: '',
      rating: 0,
      categoryIds: [],
      ingredients: []
  };
  
  categories: Category[] = []

  constructor(private categoryService: CategoryService) {
  }

  ngOnInit(): void {
    this.recipe.categoryIds.forEach(id => {
      this.categoryService.getCategoryById(id).subscribe(category => {
        this.categories.push(category);
      })
    })
  }
}
