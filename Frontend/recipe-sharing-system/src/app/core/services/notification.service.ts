import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface Notification {
  type: 'success' | 'danger' | 'warning' | 'info';
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly subject = new BehaviorSubject<Notification | null>(null);
  readonly notification$ = this.subject.asObservable();

  private timer?: ReturnType<typeof setTimeout>;

  success(message: string): void { this.show({ type: 'success', message }); }
  error(message: string): void { this.show({ type: 'danger', message }); }
  warning(message: string): void { this.show({ type: 'warning', message }); }

  show(notification: Notification, durationMs = 6000): void {
    clearTimeout(this.timer);
    this.subject.next(notification);
    this.timer = setTimeout(() => this.clear(), durationMs);
  }

  clear(): void {
    clearTimeout(this.timer);
    this.subject.next(null);
  }
}
