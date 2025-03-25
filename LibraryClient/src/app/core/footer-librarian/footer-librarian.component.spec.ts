import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FooterLibrarianComponent } from './footer-librarian.component';

describe('FooterLibrarianComponent', () => {
  let component: FooterLibrarianComponent;
  let fixture: ComponentFixture<FooterLibrarianComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FooterLibrarianComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FooterLibrarianComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
