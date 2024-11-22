import { Component, OnInit } from '@angular/core';
import { RecipeService } from 'src/app/features/recipe/services/recipe.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {
  slug?: string;

  constructor(private recipeService: RecipeService) {

  }

  ngOnInit(): void {
    this.recipeService.getRandomRecipes(1).subscribe(response => {
      this.slug = response.at(0)?.slug;
    })
  }
}
