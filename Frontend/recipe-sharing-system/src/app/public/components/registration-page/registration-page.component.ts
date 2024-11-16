import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { RegisterRequest } from 'src/app/features/auth/models/register-request.model';
import { AuthService } from 'src/app/features/auth/services/auth.service';

@Component({
  selector: 'app-registration-page',
  templateUrl: './registration-page.component.html',
  styleUrls: ['./registration-page.component.css']
})
export class RegistrationPageComponent {
  model: RegisterRequest = {
    email: '',
    userName: '',
    password: ''
  };

  constructor(private authService: AuthService, private router: Router) {
    if (this.authService.getUser()) {
      this.router.navigate(['']);
    }
  }

  onFormSubmit() {
    this.authService.register(this.model).subscribe(_ => {
      this.router.navigateByUrl('');
    })
  }
}
