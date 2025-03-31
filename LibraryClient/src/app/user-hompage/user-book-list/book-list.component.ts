// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-book-list',
//   imports: [],
//   templateUrl: './book-list.component.html',
//   styleUrl: './book-list.component.css'
// })
// export class BookListComponent {

// }
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchBarComponent } from '../search-bar/search-bar.component';
import { AuthorFilterComponent } from '../author-filter/author-filter.component';
import { PaginationComponent } from '../pagination/pagination.component';
import { Book } from '../../features/book/models/book';
import { CategoryTabsComponent } from '../category-tabs/category-tabs.component';
import { UserBookItemComponent } from '../user-book-item/book-item.component';
import { BooksService } from '../../features/book/services/books.service';

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [
    CommonModule,
    SearchBarComponent,
    AuthorFilterComponent,
    UserBookItemComponent,
    PaginationComponent,
    CategoryTabsComponent
  ],
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class UserBookListComponent implements OnInit {
  books: Book[] = [];
  currentPage: number = 1;
  booksPerPage: number = 9;
  totalPages: number = 0;

  constructor(private bookService: BooksService) { }

  ngOnInit(): void {
    this.loadBooks();
  }

  loadBooks(searchTerm: string = '', categoryId: number = 0, authorId: number = 0) {
    let observable;
    if (searchTerm) {
      observable = this.bookService.searchBooks(searchTerm);
    } else if (categoryId) {
      observable = this.bookService.getBooksByCategory(categoryId);
    } else if (authorId) {
      observable = this.bookService.getBooksByAuthor(authorId);
    } else {
      observable = this.bookService.getAllBooks();
    }

    observable.subscribe(books => {
      this.books = books;
      this.totalPages = Math.ceil(this.books.length / this.booksPerPage);
      this.currentPage = 1;
    });
  }


  get currentBooks(): Book[] {
    const start = (this.currentPage - 1) * this.booksPerPage;
    const end = start + this.booksPerPage;
    return this.books.slice(start, end);
  }

  onSearch(searchTerm: string) {
    this.loadBooks(searchTerm);
  }

  onCategorySelected(categoryId: number) {
    this.loadBooks('', categoryId);
  }

  onAuthorSelected(authorId: number) {
    this.loadBooks('', 0, authorId);
  }

  onPageChange(page: number) {
    this.currentPage = page;
  }

}
