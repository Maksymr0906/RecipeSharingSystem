import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginRequest } from 'src/app/features/auth/models/login-request.model';
import { AuthService } from 'src/app/features/auth/services/auth.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  model: LoginRequest = {
    email: '',
    password: ''
  };

  constructor(private authService: AuthService, private router: Router) {

  }

  onFormSubmit() {
    this.authService.login(this.model).subscribe(response => {
      localStorage.setItem('token', response.token);
      localStorage.setItem('roles', response.roles.join(','));
      this.router.navigateByUrl('');
    })
  }
}
