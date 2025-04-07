import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { BorrowService } from '../services/borrow.service';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-all-borrowed-book',
  imports: [CommonModule, NgxPaginationModule, FormsModule],
  templateUrl: './all-borrowed-book.component.html',
  styleUrl: './all-borrowed-book.component.css'
})
export class AllBorrowedBookComponent implements OnInit {
  public listBookBorrowed: any[] = [];
  filteredBooks: any[] = [];
  currentPage: number = 1;
  pageSize: number = 7;
  totalBooks: number = 0;
  totalPages: number = 0;
  searchKeyword: string = '';
  constructor(private service: BorrowService,
    private authService: AuthService,
    private toatrService: ToastrService) { }
  ngOnInit() {
    this.onGetData();
  }

  onGetData() {
    this.service.getAllBorrowedBooks().subscribe(
      (data) => {
        this.listBookBorrowed = data;
        this.totalBooks = data.length;
        this.applyFilter();
      },
      (error) => {
        console.error('Error fetching all borrowed books', error);
      });
  }
  onPageChange(page: number): void {
    this.currentPage = page;
    this.onGetData();
  }
  onSearchChange(): void {
    this.currentPage = 1; // Reset về trang đầu
    this.applyFilter();
  }
  applyFilter(): void {
    const keyword = this.searchKeyword.toLowerCase();
    const filtered = this.listBookBorrowed.filter(book =>
      book.bookName?.toLowerCase().includes(keyword) ||
      book.id.toString().includes(keyword)
    );

    this.totalBooks = filtered.length;
    this.totalPages = Math.ceil(this.totalBooks / this.pageSize);

    const startIndex = (this.currentPage - 1) * this.pageSize;
    this.filteredBooks = filtered.slice(startIndex, startIndex + this.pageSize);
  }
}
