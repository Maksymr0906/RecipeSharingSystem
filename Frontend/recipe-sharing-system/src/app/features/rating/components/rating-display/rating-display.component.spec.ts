import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RatingDisplayComponent } from './rating-display.component';

describe('RatingDisplayComponent', () => {
  let component: RatingDisplayComponent;
  let fixture: ComponentFixture<RatingDisplayComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RatingDisplayComponent]
    });
    fixture = TestBed.createComponent(RatingDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
