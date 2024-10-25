import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
  isAdmin = false;
  isAuthenticated = false;

  constructor(private categoryService: CategoryService, private router: Router) {

  }

  ngOnInit(): void {
     const roles = localStorage.getItem('roles')?.split(',') || [];
    const token = localStorage.getItem('token');

    // Set authentication status and admin role
    this.isAuthenticated = !!token;
    this.isAdmin = roles.includes('Admin');
    this.categories$ = this.categoryService.getAllCategories();
  }

  logout(): void {
    // Clear user data from localStorage
    localStorage.removeItem('token');
    localStorage.removeItem('roles');
    this.isAuthenticated = false;
    this.isAdmin = false;

    // Navigate to the login page or home page
    this.router.navigate(['/']);
  }
}
