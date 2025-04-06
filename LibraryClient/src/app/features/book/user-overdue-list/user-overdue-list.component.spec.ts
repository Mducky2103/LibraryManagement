import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserOverdueListComponent } from './user-overdue-list.component';

describe('UserOverdueListComponent', () => {
  let component: UserOverdueListComponent;
  let fixture: ComponentFixture<UserOverdueListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserOverdueListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserOverdueListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
