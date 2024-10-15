import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from 'src/app/features/category/models/category.model';
import { CategoryService } from 'src/app/features/category/services/category.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  categories$?: Observable<Category[]>;

  constructor(private categoryService: CategoryService) {

  }

  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();
  }
}
