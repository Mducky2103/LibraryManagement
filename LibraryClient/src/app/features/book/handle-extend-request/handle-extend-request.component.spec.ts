import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HandleExtendRequestComponent } from './handle-extend-request.component';

describe('HandleExtendRequestComponent', () => {
  let component: HandleExtendRequestComponent;
  let fixture: ComponentFixture<HandleExtendRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HandleExtendRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HandleExtendRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
