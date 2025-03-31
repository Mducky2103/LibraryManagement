// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-author-filter',
//   imports: [],
//   templateUrl: './author-filter.component.html',
//   styleUrl: './author-filter.component.css'
// })
// export class AuthorFilterComponent {

// }
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // Thêm để sử dụng ngModel
import { Book } from '../../features/book/models/book';

@Component({
  selector: 'app-author-filter',
  standalone: true,
  imports: [CommonModule, FormsModule], // Thêm FormsModule để sử dụng ngModel
  templateUrl: './author-filter.component.html',
  styleUrls: ['./author-filter.component.css']
})
export class AuthorFilterComponent implements OnInit {
  @Input() books: Book[] = [];
  @Output() authorSelected = new EventEmitter<number>();
  authors: { id: number, name: string }[] = [];
  selectedAuthor: number | null = null;

  ngOnInit(): void {
    const uniqueAuthors = new Map<number, string>();
    this.books.forEach(book => {
      if (!uniqueAuthors.has(book.authorId)) {
        uniqueAuthors.set(book.authorId, book.authorName);
      }
    });
    this.authors = Array.from(uniqueAuthors, ([id, name]) => ({ id, name }));
  }

  onAuthorChange() {
    this.authorSelected.emit(this.selectedAuthor || 0);
  }
}
