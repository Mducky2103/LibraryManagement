import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserBookItemComponent } from './book-item.component';

describe('BookItemComponent', () => {
  let component: UserBookItemComponent;
  let fixture: ComponentFixture<UserBookItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserBookItemComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(UserBookItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
