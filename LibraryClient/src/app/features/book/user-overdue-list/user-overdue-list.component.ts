import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { BorrowService } from '../services/borrow.service';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Borrow } from '../models/db.model';

@Component({
  selector: 'app-user-overdue-list',
  imports: [CommonModule, NgxPaginationModule, FormsModule],
  templateUrl: './user-overdue-list.component.html',
  styleUrl: './user-overdue-list.component.css'
})
export class UserOverdueListComponent implements OnInit {
  public overdueUserList: Borrow[] = [];
  currentPage: number = 1;
  pageSize: number = 7;
  totalBooks: number = 0;
  totalPages: number = 0;
  public auth: any;
  constructor(private service: BorrowService,
    private authService: AuthService,
    private toatrService: ToastrService) { }

  ngOnInit(): void {
    this.auth = this.authService.getClaims();
    this.onGetOverdueBooks();
  }

  onGetOverdueBooks() {
    this.service.getOverdueBooksByUser(this.auth.userID).subscribe((data) => {
      this.overdueUserList = data;
      console.log("Overdue Books", this.overdueUserList);
      this.totalBooks = data.length;

      this.totalPages = Math.ceil(this.totalBooks / this.pageSize);

      const startIndex = (this.currentPage - 1) * this.pageSize;
      const endIndex = startIndex + this.pageSize;
      this.overdueUserList = data.slice(startIndex, endIndex);
    }
    );
  }

  returnOverdueWithPenalty(bookId: number) {
    this.service.returnOverdueBookWithPenalty(bookId).subscribe({
      next: (response) => {
        this.toatrService.success('Book returned successfully and penalty applied.');
        this.onGetOverdueBooks(); // Refresh the list
      },
      error: (error) => {
        console.error('Error returning overdue book with penalty:', error);
        this.toatrService.error('Failed to return book and apply penalty.');
      }
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.onGetOverdueBooks();
  }

}
