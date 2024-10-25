import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from 'src/app/features/auth/models/user.model';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { Category } from 'src/app/features/category/models/category.model';
import { CategoryService } from 'src/app/features/category/services/category.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  categories$?: Observable<Category[]>;
  user?: User;

  constructor(
    private categoryService: CategoryService,
    private router: Router,
    private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.user().subscribe(response => {
      this.user = response;
    });

    this.user = this.authService.getUser();
    this.categories$ = this.categoryService.getAllCategories();
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
