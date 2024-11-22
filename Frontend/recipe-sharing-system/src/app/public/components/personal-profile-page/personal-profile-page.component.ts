import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { EditUserRequest } from 'src/app/features/user/models/edit-user-request.model';
import { UserService } from 'src/app/features/user/services/user.service';

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
    private userService: UserService
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
      this.editUserSubscription = this.userService.updateUser(this.user.userId, this.editUserRequest).subscribe();
    }
  }
}
