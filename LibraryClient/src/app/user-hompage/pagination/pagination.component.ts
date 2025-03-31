// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-pagination',
//   imports: [],
//   templateUrl: './pagination.component.html',
//   styleUrl: './pagination.component.css'
// })
// export class PaginationComponent {

// }
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent {
  @Input() totalPages: number = 0;
  @Input() currentPage: number = 1;
  @Output() pageChange = new EventEmitter<number>();

  get pages(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  goToPage(page: number) {
    this.pageChange.emit(page);
  }
}