import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { Book } from '../../features/book/models/book';
import { BooksService } from '../../features/book/services/books.service';
import { AuthService } from '../../shared/services/auth.service';
import { BorrowService } from '../../features/book/services/borrow.service';

@Component({
  selector: 'app-book-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './book-detail.component.html',
  styleUrls: ['./book-detail.component.css']
})
export class BookDetailComponent implements OnInit {
  public idBook: any;
  public aut: any;
  book: Book | null = null;
  // borrowQuantity: number = 1;
  imageUrl: string | null = null;
  showConfirmModal: boolean = false;
  showSuccessModal: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private bookService: BooksService,
    private toastr: ToastrService,
    private auth: AuthService,
    private serviceBorrow: BorrowService,
  ) {
    this.aut = this.auth.getClaims();
    this.idBook = route.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.bookService.getBookById(id).subscribe({
        next: (book) => {
          this.book = book;
          if (book.image) {
            this.loadBookImage(book);
          }
        },
        error: (err) => {
          console.error('Error fetching book:', err);
          this.toastr.error('Không thể tải thông tin sách');
        }
      });
    }
  }

  loadBookImage(book: Book) {
    this.bookService.getBookImage(book.image).subscribe((imageBlob) => {
      const imageUrl = URL.createObjectURL(imageBlob);
      this.imageUrl = imageUrl;
    });
  }
  onBorrow() {
    this.showConfirmModal = true;
  }

  confirmBorrow() {
    let formData = {
      userId: this.aut.userID,
      bookIds:
        [
          parseInt(this.idBook)
        ]
    }
    console.log(formData);
    this.serviceBorrow.borrow(formData).subscribe((data) => {
      this.toastr.success("Borrow book successfully");
      this.showConfirmModal = false;
      this.showSuccessModal = true;
    },
      (error) => {
        this.toastr.error("Borrow book failed");
      });
    this.showConfirmModal = false;
  }

  cancelBorrow() {
    // Đóng modal khi hủy
    this.showConfirmModal = false;
  }
  closeSuccessModal() {
    this.showSuccessModal = false; // Đóng modal thành công
  }
}