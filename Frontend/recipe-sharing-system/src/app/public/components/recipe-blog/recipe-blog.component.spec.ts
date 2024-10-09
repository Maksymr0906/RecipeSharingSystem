import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecipeBlogComponent } from './recipe-blog.component';

describe('RecipeBlogComponent', () => {
  let component: RecipeBlogComponent;
  let fixture: ComponentFixture<RecipeBlogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RecipeBlogComponent]
    });
    fixture = TestBed.createComponent(RecipeBlogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
