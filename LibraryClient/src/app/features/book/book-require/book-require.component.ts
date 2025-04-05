import { Component, OnInit, ViewChild } from '@angular/core';

import { NgxPaginationModule } from 'ngx-pagination';
import { BooksService } from '../services/books.service';
import { Router } from '@angular/router';
import { Book } from '../models/book';

@Component({
  selector: 'app-book-require',
  imports: [
    NgxPaginationModule
  ],
  templateUrl: './book-require.component.html',
  styleUrl: './book-require.component.css'
})
export class BookRequireComponent implements OnInit {
  public books: Book[] = [];
  p: number = 1;
  constructor(private service: BooksService,
    private router: Router
  ) { }
  ngOnInit(): void {
    this.onGetData();
  }
  onGetData() {
    this.service.getAllBooks().subscribe((data) => {
      this.books = data;
    })
  }
  onRoutingPage(id: number) {
    this.router.navigate(['book-require/detail', id]);
  }

  // genderImage(image:any){
  //   return 'http://localhost:4200/' + image
  // }
}
