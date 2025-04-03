import { Component, OnInit } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { RouterLink } from '@angular/router';

import { Book } from '../../features/book/models/book';
import { Category } from '../../features/category/models/category';
import { BooksService } from '../../features/book/services/books.service';
import { CategoryService } from '../../features/category/services/category.service';
import { BookItemComponent } from '../book-item/book-item.component';

@Component({
  selector: 'app-user-book-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    BookItemComponent,
    RouterLink
  ],
  templateUrl: './user-book-list.component.html',
  styleUrls: ['./user-book-list.component.css']
})
export class UserBookListComponent implements OnInit {
  books: Book[] = [];
  filteredBooks: Book[] = [];
  categories: Category[] = [];
  searchTerm: string = '';
  selectedCategoryId: number = 0;
  itemsPerLoad: number = 8;
  currentItems: number = 8;
  isLoading: boolean = false;

  constructor(
    private bookService: BooksService,
    private categoryService: CategoryService
  ) { }

  ngOnInit(): void {
    this.loadBooks();
    this.loadCategories();
  }

  loadBooks(): void {
    this.isLoading = true;
    this.bookService.getAllBooks().subscribe({
      next: (data) => {
        this.books = data;
        this.filteredBooks = data;
        this.currentItems = this.itemsPerLoad;
        this.isLoading = false;
        console.log('Books loaded: ', data);
      },
      error: (err) => {
        console.error('Error fetching books:', err);
        this.isLoading = false;
      }
    });
  }

  loadCategories(): void {
    this.categoryService.getAllCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
      },
      error: (err) => {
        console.error('Error fetching categories:', err);
      }
    });
  }

  onCategorySelect(categoryId: number): void {
    this.selectedCategoryId = categoryId;
    this.isLoading = true;
    if (categoryId === 0) {
      this.filteredBooks = this.books;
      this.isLoading = false;
    } else {
      this.bookService.getBooksByCategory(categoryId).subscribe({
        next: (books) => {
          this.filteredBooks = books;
          this.currentItems = this.itemsPerLoad;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error fetching books by category:', err);
          this.isLoading = false;
        }
      });
    }
  }

  onSearch(): void {
    this.isLoading = true;
    if (!this.searchTerm) {
      this.filteredBooks = this.books;
      this.isLoading = false;
    } else {
      this.bookService.searchBooks(this.searchTerm).subscribe({
        next: (books) => {
          this.filteredBooks = books;
          this.currentItems = this.itemsPerLoad;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error searching books:', err);
          this.isLoading = false;
        }
      });
    }
  }

  loadMore(): void {
    this.currentItems += this.itemsPerLoad;
  }

  get displayedBooks(): Book[] {
    return this.filteredBooks.slice(0, this.currentItems);
  }

  get canLoadMore(): boolean {
    return this.currentItems < this.filteredBooks.length;
  }
}