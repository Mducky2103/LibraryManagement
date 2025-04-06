import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BorrowRequestListComponent } from './borrow-request-list.component';

describe('BorrowRequestListComponent', () => {
  let component: BorrowRequestListComponent;
  let fixture: ComponentFixture<BorrowRequestListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BorrowRequestListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BorrowRequestListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
