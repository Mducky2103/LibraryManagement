import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { Book } from '../../features/book/models/book';
import { BooksService } from '../../features/book/services/books.service';

@Component({
  selector: 'app-book-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './book-detail.component.html',
  styleUrls: ['./book-detail.component.css']
})
export class BookDetailComponent implements OnInit {
  book: Book | null = null;
  // borrowQuantity: number = 1;
  imageUrl: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private bookService: BooksService,
    private toastr: ToastrService
  ) { }

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

  // increaseQuantity(): void {
  //     if (this.book && this.borrowQuantity < this.book.quantity) {
  //         this.borrowQuantity++;
  //     }
  // }

  // decreaseQuantity(): void {
  //     if (this.borrowQuantity > 1) {
  //         this.borrowQuantity--;
  //     }
  // }

  // borrowBook(): void {
  //     if (this.book && this.book.isAvailable && this.borrowQuantity <= this.book.quantity) {
  //         this.bookService.borrowBook(this.book.id, this.borrowQuantity).subscribe({
  //             next: () => {
  //                 this.toastr.success(`Đã mượn ${this.borrowQuantity} cuốn "${this.book!.name}"`);
  //                 this.book!.quantity -= this.borrowQuantity;
  //                 if (this.book!.quantity === 0) {
  //                     this.book!.isAvailable = false;
  //                 }
  //             },
  //             error: (err) => {
  //                 console.error('Error borrowing book:', err);
  //                 this.toastr.error('Không thể mượn sách');
  //             }
  //         });
  //     } else {
  //         this.toastr.error('Sách không khả dụng hoặc số lượng không đủ');
  //     }
  // }
}