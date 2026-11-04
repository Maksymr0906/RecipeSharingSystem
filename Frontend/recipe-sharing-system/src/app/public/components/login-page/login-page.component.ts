import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginRequest } from 'src/app/features/auth/models/login-request.model';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { extractErrorMessage } from 'src/app/core/utils/http-error';

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

  errorMessage = '';
  isSubmitting = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
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

    this.authService.login(this.model).subscribe({
      next: response => {
        this.authService.setUser({
          email: response.email,
          roles: response.roles,
          userId: response.userId
        });

        this.authService.storeToken(response.token);

        // Only relative in-app addresses are accepted as the return address.
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
        this.router.navigateByUrl(returnUrl && returnUrl.startsWith('/') && !returnUrl.startsWith('//') ? returnUrl : '');
      },
      error: error => {
        this.isSubmitting = false;
        this.model.password = '';
        this.errorMessage = extractErrorMessage(error);
      }
    });
  }
}
