import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplayRecipesPageComponent } from './display-recipes-page.component';

describe('DisplayRecipesPageComponent', () => {
  let component: DisplayRecipesPageComponent;
  let fixture: ComponentFixture<DisplayRecipesPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DisplayRecipesPageComponent]
    });
    fixture = TestBed.createComponent(DisplayRecipesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
