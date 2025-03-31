import { Component, inject, OnInit } from '@angular/core';
import { Book } from '../models/book';
import { BooksService } from '../services/books.service';
import { CommonModule } from '@angular/common';
import { NavbarLibrarianComponent } from "../../../core/navbar-librarian/navbar-librarian.component";
import { FooterLibrarianComponent } from "../../../core/footer-librarian/footer-librarian.component";
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import * as alertifyjs from 'alertifyjs';

@Component({
  selector: 'app-book-list',
  imports: [CommonModule, NavbarLibrarianComponent, FooterLibrarianComponent, RouterLink, FormsModule],
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.css'
})
export class BookListComponent implements OnInit {
  books: Book[] = [];
  filteredBooks: Book[] = [];
  currentPage: number = 1;
  pageSize: number = 5;
  totalBooks: number = 0;
  totalPages: number = 0;
  searchQuery: string = '';
  selectedBook: Book | null = null;

  constructor(private bookService: BooksService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadBooks();
  }

  loadBooks(): void {
    this.bookService.getAllBooks(this.currentPage, this.pageSize).subscribe((data: any) => {
      this.books = data.sort((a: Book, b: Book) => b.id - a.id);;

      this.totalBooks = this.books.length;

      this.totalPages = Math.ceil(this.totalBooks / this.pageSize);

      const startIndex = (this.currentPage - 1) * this.pageSize;
      const endIndex = startIndex + this.pageSize;
      this.filteredBooks = this.books.slice(startIndex, endIndex);


      this.books.forEach(book => {
        if (book.image) {
          this.loadBookImage(book);
        }
      });

    });
  }

  toggleBookDetail(book: Book) {
    if (book.isSelected) {
      book.isSelected = false;
    } else {
      this.filteredBooks.forEach(b => b.isSelected = false); // Bỏ chọn tất cả sách khác
      book.isSelected = true; // Chọn sách hiện tại
    }
  }

  selectBook(book: Book) {
    this.selectedBook = this.selectedBook?.id === book.id ? null : book;
  }

  loadBookImage(book: Book) {
    this.bookService.getBookImage(book.image).subscribe((imageBlob) => {
      const imageUrl = URL.createObjectURL(imageBlob);
      book.image = imageUrl;
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadBooks();
  }

  onSearch(): void {
    if (this.searchQuery) {
      const filtered = this.books.filter(book =>
        book.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        book.authorName.toLowerCase().includes(this.searchQuery.toLowerCase())
      );
      this.totalBooks = filtered.length;
      this.totalPages = Math.ceil(this.totalBooks / this.pageSize);

      const startIndex = (this.currentPage - 1) * this.pageSize;
      const endIndex = startIndex + this.pageSize;
      this.filteredBooks = filtered.slice(startIndex, endIndex);
    } else {
      this.totalBooks = this.books.length;
      this.totalPages = Math.ceil(this.totalBooks / this.pageSize);
      const startIndex = (this.currentPage - 1) * this.pageSize;
      const endIndex = startIndex + this.pageSize;
      this.filteredBooks = this.books.slice(startIndex, endIndex);
    }
  }

  deleteBook(id: number): void {
    if (confirm('Are you sure you want to delete this book?')) {
      this.bookService.deleteBook(id).subscribe({
        next: () => {
          this.loadBooks();
          alertifyjs.success(`Book with ID ${id} deleted successfully`);
        },
        error: (err) => {
          console.error('Error deleting book:', err);
          alertifyjs.error('Failed to delete the book.')
        }
      });
    }
  }

}
