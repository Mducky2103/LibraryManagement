import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { HideIfClaimsNotMetDirective } from '../../directives/hide-if-claims-not-met.directive';
import { claimReq } from "../../shared/utils/claimReq-utils"
import { UserService } from '../../shared/services/user.service';
import { Borrow } from '../../features/book/models/db.model';
import { BorrowService } from '../../features/book/services/borrow.service';
import { CommonModule } from '@angular/common';
import { filter } from 'rxjs';
@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet, RouterLink, HideIfClaimsNotMetDirective, CommonModule],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css',
})
export class MainLayoutComponent implements OnInit {
  constructor(private router: Router,
    private authService: AuthService,
    private userService: UserService,
    private service: BorrowService) { }
  claimReq = claimReq;
  fullName: string = '';
  showNotifications = false;
  public borrowedBooks: Borrow[] = [];
  public borrowFilter: Borrow[] = [];
  ngOnInit(): void {
    this.userService.getUserProfile().subscribe({
      next: (res: any) => this.fullName = res.fullName,
      error: (err: any) => console.log('error while retrieving user profile:\n', err)
    })
    this.onGetData();
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.showNotifications = false;
      });
  }

  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
  }

  onLogout() {
    this.authService.deleteToken();
    this.router.navigateByUrl('/signin')
  }

  onGetData() {
    this.service.getBorrowHistoryByIdUser(this.authService.getClaims().userID).subscribe((data) => {
      this.borrowedBooks = data;

      const today = new Date();

      this.borrowedBooks = data.filter((book) => {
        if (book.dueDate) {
          const dueDate = new Date(book.dueDate);

          // Tính số ngày còn lại đến hạn
          const timeDiff = dueDate.getTime() - today.getTime();
          const dayDiff = Math.ceil(timeDiff / (1000 * 60 * 60 * 24));

          return dayDiff <= 3 && dayDiff >= 0; // <= 3 ngày và chưa quá hạn
        }
        return false;
      });
    });
  }
}
