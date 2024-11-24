import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchRecipesComponent } from './search-recipes.component';

describe('SearchRecipesComponent', () => {
  let component: SearchRecipesComponent;
  let fixture: ComponentFixture<SearchRecipesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SearchRecipesComponent]
    });
    fixture = TestBed.createComponent(SearchRecipesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
