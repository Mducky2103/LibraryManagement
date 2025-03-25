import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarLibrarianComponent } from './navbar-librarian.component';

describe('NavbarLibrarianComponent', () => {
  let component: NavbarLibrarianComponent;
  let fixture: ComponentFixture<NavbarLibrarianComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarLibrarianComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavbarLibrarianComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
