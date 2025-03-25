import { Component, inject, OnInit } from '@angular/core';
import { Book } from '../models/book';
import { BooksService } from '../services/books.service';
import { CommonModule } from '@angular/common';
import { NavbarLibrarianComponent } from "../../../core/navbar-librarian/navbar-librarian.component";
import { FooterLibrarianComponent } from "../../../core/footer-librarian/footer-librarian.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-book-list',
  imports: [CommonModule, NavbarLibrarianComponent, FooterLibrarianComponent],
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.css'
})
export class BookListComponent implements OnInit {
  books: Book[] = [];
  apiUrl: any = "https://localhost:7274/";

  constructor(private bookService: BooksService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.bookService.getAllBooks().subscribe((data) => {
      this.books = data;
    });
  }

  addNewBook() {

    this.router.navigate(['/add-book']);
  }

}
