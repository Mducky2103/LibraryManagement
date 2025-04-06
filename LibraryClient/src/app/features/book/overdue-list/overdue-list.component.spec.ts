import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OverdueListComponent } from './overdue-list.component';

describe('OverdueListComponent', () => {
  let component: OverdueListComponent;
  let fixture: ComponentFixture<OverdueListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OverdueListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OverdueListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
