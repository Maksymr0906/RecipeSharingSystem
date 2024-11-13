import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RatingReadonlyInputComponent } from './rating-readonly-input.component';

describe('RatingReadonlyInputComponent', () => {
  let component: RatingReadonlyInputComponent;
  let fixture: ComponentFixture<RatingReadonlyInputComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RatingReadonlyInputComponent]
    });
    fixture = TestBed.createComponent(RatingReadonlyInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
