import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavoriteToggleButtonComponent } from './favorite-toggle-button.component';

describe('FavoriteToggleButtonComponent', () => {
  let component: FavoriteToggleButtonComponent;
  let fixture: ComponentFixture<FavoriteToggleButtonComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FavoriteToggleButtonComponent]
    });
    fixture = TestBed.createComponent(FavoriteToggleButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
