// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-book-item',
//   imports: [],
//   templateUrl: './book-item.component.html',
//   styleUrl: './book-item.component.css'
// })
// export class BookItemComponent {

// }
import { Component, Input, OnInit } from '@angular/core';
import { Book } from '../../features/book/models/book';
import { CommonModule } from '@angular/common';
import { BooksService } from '../../features/book/services/books.service';

@Component({
  selector: 'app-book-item',
  imports: [CommonModule],
  templateUrl: './book-item.component.html',
  styleUrl: './book-item.component.css'
})
export class UserBookItemComponent implements OnInit {
  @Input() book!: Book;

  constructor(private bookService: BooksService) { }

  ngOnInit(): void {

    if (this.book.image) {
      this.loadBookImage(this.book);
    }

  };

  loadBookImage(book: Book) {
    this.bookService.getBookImage(book.image).subscribe((imageBlob) => {
      const imageUrl = URL.createObjectURL(imageBlob);
      book.image = imageUrl;
    });
  }

}
