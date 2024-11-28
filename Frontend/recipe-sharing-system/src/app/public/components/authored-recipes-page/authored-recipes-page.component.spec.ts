import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthoredRecipesPageComponent } from './authored-recipes-page.component';

describe('AuthoredRecipesPageComponent', () => {
  let component: AuthoredRecipesPageComponent;
  let fixture: ComponentFixture<AuthoredRecipesPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AuthoredRecipesPageComponent]
    });
    fixture = TestBed.createComponent(AuthoredRecipesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
