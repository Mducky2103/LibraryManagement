import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllBorrowedBookComponent } from './all-borrowed-book.component';

describe('AllBorrowedBookComponent', () => {
  let component: AllBorrowedBookComponent;
  let fixture: ComponentFixture<AllBorrowedBookComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllBorrowedBookComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllBorrowedBookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
