import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRecipeGuidePageComponent } from './add-recipe-guide-page.component';

describe('AddRecipeGuidePageComponent', () => {
  let component: AddRecipeGuidePageComponent;
  let fixture: ComponentFixture<AddRecipeGuidePageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddRecipeGuidePageComponent]
    });
    fixture = TestBed.createComponent(AddRecipeGuidePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
