import { Component } from '@angular/core';
import { LoginRequest } from 'src/app/features/auth/models/login-request.model';
import { AuthService } from 'src/app/features/auth/services/auth.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  constructor(private authService: AuthService) {

  }

  onFormSubmit() {

  }
}
