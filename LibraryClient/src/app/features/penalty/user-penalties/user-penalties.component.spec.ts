import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserPenaltiesComponent } from './user-penalties.component';

describe('UserPenaltiesComponent', () => {
  let component: UserPenaltiesComponent;
  let fixture: ComponentFixture<UserPenaltiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserPenaltiesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserPenaltiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
