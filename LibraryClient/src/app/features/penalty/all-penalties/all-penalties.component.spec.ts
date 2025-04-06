import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllPenaltiesComponent } from './all-penalties.component';

describe('AllPenaltiesComponent', () => {
  let component: AllPenaltiesComponent;
  let fixture: ComponentFixture<AllPenaltiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllPenaltiesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllPenaltiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
