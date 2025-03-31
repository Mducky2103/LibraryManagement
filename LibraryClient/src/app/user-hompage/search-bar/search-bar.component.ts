// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-search-bar',
//   imports: [],
//   templateUrl: './search-bar.component.html',
//   styleUrl: './search-bar.component.css'
// })
// export class SearchBarComponent {

// }
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms'; // Thêm để sử dụng ngModel

@Component({
  selector: 'app-search-bar',
  standalone: true,
  imports: [FormsModule], // Thêm FormsModule để sử dụng ngModel
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent {
  searchTerm: string = '';
  @Output() search = new EventEmitter<string>();

  onSearch() {
    this.search.emit(this.searchTerm);
  }
}
