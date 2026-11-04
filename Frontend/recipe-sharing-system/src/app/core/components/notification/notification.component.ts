import { Component } from '@angular/core';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent {
  notification$ = this.notificationService.notification$;

  constructor(private notificationService: NotificationService) { }

  close(): void {
    this.notificationService.clear();
  }
}
