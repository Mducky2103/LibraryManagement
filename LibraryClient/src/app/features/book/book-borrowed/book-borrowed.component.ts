import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { NgxPaginationModule } from 'ngx-pagination';
import { Borrow } from '../models/db.model';
import { BorrowService } from '../services/borrow.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-book-borrowed',
  imports: [CommonModule,
    NgxPaginationModule,
    FormsModule
  ],
  templateUrl: './book-borrowed.component.html',
  styleUrl: './book-borrowed.component.css'
})
export class BookBorrowedComponent implements OnInit {
  public listBookBorrowed: Borrow[] = [];
  selectedBook: Borrow | null = null;
  filteredBooks: Borrow[] = [];

  public aut: any;
  currentPage: number = 1;
  pageSize: number = 7;
  totalBooks: number = 0;
  totalPages: number = 0;
  searchKeyword: string = '';

  constructor(private service: BorrowService,
    private authService: AuthService,
    private toatrService: ToastrService
  ) { }
  ngOnInit(): void {
    this.aut = this.authService.getClaims();
    this.onGetData();
  }
  onGetData() {
    this.service.getBorrowHistoryByIdUser(this.aut.userID).subscribe((data) => {
      this.listBookBorrowed = data;
      this.totalBooks = data.length;

      // this.totalPages = Math.ceil(this.totalBooks / this.pageSize);

      // const startIndex = (this.currentPage - 1) * this.pageSize;
      // const endIndex = startIndex + this.pageSize;
      // this.listBookBorrowed = data.slice(startIndex, endIndex);

      // console.log(this.listBookBorrowed);
      this.applyFilter();
    }
    );
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
