import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidebarLibrarianComponent } from './sidebar-librarian.component';

describe('SidebarLibrarianComponent', () => {
  let component: SidebarLibrarianComponent;
  let fixture: ComponentFixture<SidebarLibrarianComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SidebarLibrarianComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SidebarLibrarianComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
