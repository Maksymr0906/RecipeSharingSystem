import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavoriteRecipesListComponent } from './favorite-recipes-list.component';

describe('FavoriteRecipesListComponent', () => {
  let component: FavoriteRecipesListComponent;
  let fixture: ComponentFixture<FavoriteRecipesListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FavoriteRecipesListComponent]
    });
    fixture = TestBed.createComponent(FavoriteRecipesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
