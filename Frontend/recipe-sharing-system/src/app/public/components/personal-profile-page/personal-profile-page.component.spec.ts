import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalProfilePageComponent } from './personal-profile-page.component';

describe('PersonalProfilePageComponent', () => {
  let component: PersonalProfilePageComponent;
  let fixture: ComponentFixture<PersonalProfilePageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PersonalProfilePageComponent]
    });
    fixture = TestBed.createComponent(PersonalProfilePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
