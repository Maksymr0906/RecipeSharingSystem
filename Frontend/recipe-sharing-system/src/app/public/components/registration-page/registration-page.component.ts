import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { RegisterRequest } from 'src/app/features/auth/models/register-request.model';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { extractErrorMessage } from 'src/app/core/utils/http-error';

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

  errorMessage = '';
  isSubmitting = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private notifications: NotificationService
  ) {
    if (this.authService.getUser()) {
      this.router.navigate(['']);
    }
  }

  onFormSubmit() {
    if (this.isSubmitting) {
      return;
    }

    this.errorMessage = '';
    this.isSubmitting = true;

    this.authService.register(this.model).subscribe({
      next: () => {
        this.notifications.success('Account created. You can log in now.');
        this.router.navigateByUrl('/login');
      },
      error: error => {
        this.isSubmitting = false;
        this.errorMessage = extractErrorMessage(error);
      }
    });
  }
}
