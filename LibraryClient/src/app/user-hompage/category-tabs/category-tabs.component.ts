// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-category-tabs',
//   imports: [],
//   templateUrl: './category-tabs.component.html',
//   styleUrl: './category-tabs.component.css'
// })
// export class CategoryTabsComponent {

// }
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Category } from '../../features/category/models/category';
import { CategoryService } from '../../features/category/services/category.service';

@Component({
  selector: 'app-category-tabs',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './category-tabs.component.html',
  styleUrls: ['./category-tabs.component.css']
})
export class CategoryTabsComponent implements OnInit {
  categories: Category[] = [];
  @Output() categorySelected = new EventEmitter<number>();

  constructor(private categoryService: CategoryService) { }

  ngOnInit(): void {
    this.categoryService.getAllCategories().subscribe(categories => {
      this.categories = categories;
    });
  }

  selectCategory(categoryId: number) {
    this.categorySelected.emit(categoryId);
  }
}
