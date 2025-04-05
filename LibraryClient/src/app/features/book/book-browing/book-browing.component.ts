import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { Borrow } from '../models/db.model';
import { BorrowService } from '../services/borrow.service';
import { AuthService } from '../../../shared/services/auth.service';

@Component({
  selector: 'app-book-browing',
  imports: [
    CommonModule
  ],
  templateUrl: './book-browing.component.html',
  styleUrl: './book-browing.component.css'
})
export class BookBrowingComponent implements OnInit {
  public listBookBorrowed: Borrow[] = [];
  currentPage: number = 1;
  pageSize: number = 7;
  totalBooks: number = 0;
  totalPages: number = 0;
  selectedBook: Borrow | null = null;
  public aut: any;
  constructor(private service: BorrowService,
    private authService: AuthService,
    private toatrService: ToastrService,
  ) { }
  ngOnInit(): void {
    this.aut = this.authService.getClaims();
    this.onGetData();
  }
  onGetData() {
    this.service.getBorrowingingByIdUser(this.aut.userID).subscribe((data) => {
      this.listBookBorrowed = data
      console.log("Info", this.listBookBorrowed);
      this.totalBooks = data.length;

      this.totalPages = Math.ceil(this.totalBooks / this.pageSize);

      const startIndex = (this.currentPage - 1) * this.pageSize;
      const endIndex = startIndex + this.pageSize;
      this.listBookBorrowed = data.slice(startIndex, endIndex);
    }
    );
  }
  openExtendModal(book: Borrow): void {
    this.selectedBook = book;
    const extendDateInput = document.getElementById('extendDate') as HTMLInputElement;
    extendDateInput.value = '';
    const modal = document.getElementById('extendModal') as HTMLElement;
    modal.style.display = 'flex';
  }

  onReturnBook(receiptDetailId: number): void {
    this.service.returnBook(receiptDetailId).subscribe({
      next: (response: any) => {
        if (response) {
          alert('Book returned successfully!');
          this.onGetData(); // Refresh the list after returning the book
        } else {
          alert('Failed to return the book.');
        }
      },
      error: (error: any) => {
        console.error('Error returning book:', error);
        alert('An error occurred while returning the book.');
      }
    });
  }

  closeExtendModal(): void {
    const modal = document.getElementById('extendModal') as HTMLElement;
    modal.style.display = 'none';
  }

  extendBorrowPeriod(): void {
    if (this.selectedBook) {
      const extendDateInput = document.getElementById('extendDate') as HTMLInputElement;
      const newDueDate = extendDateInput.value;
      if (newDueDate) {
        const note: any = extendDateInput.value;
        this.service.dueDateborrow(this.selectedBook.id, newDueDate).subscribe((data) => {
          this.toatrService.info(data.message);
          this.closeExtendModal();
        })

      } else {
        alert('Input your required!');
      }
    }
  }
  onPageChange(page: number): void {
    this.currentPage = page;
    this.onGetData();
  }
}
