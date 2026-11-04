import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { EditUserRequest } from 'src/app/features/user/models/edit-user-request.model';
import { UserService } from 'src/app/features/user/services/user.service';
import { NotificationService } from 'src/app/core/services/notification.service';

@Component({
  selector: 'app-personal-profile-page',
  templateUrl: './personal-profile-page.component.html',
  styleUrls: ['./personal-profile-page.component.css']
})
export class PersonalProfilePageComponent implements OnInit, OnDestroy {
  user = this.authService.getUser();
  editUserRequest: EditUserRequest = {
    userName: '',
    email: '',
    firstName: '',
    lastName: '',
    dateOfBirth: new Date(),
    postalCode: ''
  }

  getUserByIdSubscription?: Subscription;
  editUserSubscription?: Subscription;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private notifications: NotificationService
  ) {}

  ngOnInit(): void {
    if (this.user) {
      this.getUserByIdSubscription = this.userService.getUserById(this.user.userId).subscribe(response => {
        // Format the date before assigning
        this.editUserRequest = {
          ...response,
          dateOfBirth: response.dateOfBirth ? new Date(response.dateOfBirth) : undefined
        };
      });
    }
  }

  get dateOfBirthString(): string {
    if (!this.editUserRequest.dateOfBirth) return '';
    return this.formatDateForInput(this.editUserRequest.dateOfBirth);
  }

  set dateOfBirthString(value: string) {
    this.editUserRequest.dateOfBirth = value ? new Date(value) : undefined;
  }

  private formatDateForInput(date: Date): string {
    return date.toISOString().split('T')[0];
  }

  ngOnDestroy(): void {
    this.getUserByIdSubscription?.unsubscribe();
    this.editUserSubscription?.unsubscribe();
  }

  onFormSubmit() {
    if (this.user) {
      // Only the editable fields are sent; empty text boxes mean "leave unchanged".
      const request: EditUserRequest = {
        email: this.editUserRequest.email || undefined,
        userName: this.editUserRequest.userName || undefined,
        firstName: this.editUserRequest.firstName || undefined,
        lastName: this.editUserRequest.lastName || undefined,
        dateOfBirth: this.editUserRequest.dateOfBirth,
        postalCode: this.editUserRequest.postalCode || undefined
      };

      this.editUserSubscription = this.userService.updateUser(this.user.userId, request).subscribe({
        next: () => this.notifications.success('Your profile has been saved.'),
        error: () => { /* the message is shown by the error interceptor */ }
      });
    }
  }
}
