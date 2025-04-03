import { Component, type OnInit, Output, EventEmitter } from "@angular/core"
import { CommonModule } from "@angular/common"
import { RouterModule } from "@angular/router"

interface Category {
    id: number
    name: string
    active?: boolean
}

@Component({
    selector: "app-category-nav",
    standalone: true,
    imports: [CommonModule, RouterModule],
    template: `
    <div class="category-nav-container">
      <div class="category-nav">
        <a 
          *ngFor="let category of categories" 
          [class.active]="selectedCategoryId === category.id"
          (click)="selectCategory(category.id)"
          class="category-item"
        >
          {{ category.name }}
        </a>
      </div>
    </div>
  `,
    styles: [
        `
    .category-nav-container {
      overflow-x: auto;
      -ms-overflow-style: none;
      scrollbar-width: none;
      margin-bottom: 1.5rem;
    }
    
    .category-nav-container::-webkit-scrollbar {
      display: none;
    }
    
    .category-nav {
      display: flex;
      gap: 0.75rem;
      padding: 0.5rem 0;
      min-width: max-content;
    }
    
    .category-item {
      white-space: nowrap;
      padding: 0.5rem 1rem;
      border-radius: 4px;
      color: #333;
      text-decoration: none;
      font-size: 0.95rem;
      transition: background-color 0.2s;
      cursor: pointer;
    }
    
    .category-item:hover {
      background-color: rgba(13, 110, 253, 0.1);
    }
    
    .category-item.active {
      background-color: #0d6efd;
      color: white;
    }
  `,
    ],
})
export class CategoryNavComponent implements OnInit {
    @Output() categorySelected = new EventEmitter<number>()

    categories: Category[] = [
        { id: 0, name: "Tất cả", active: true },
        { id: 1, name: "Văn học" },
        { id: 2, name: "Khoa học viễn tưởng" },
        { id: 3, name: "Tiểu thuyết" },
        { id: 4, name: "Trinh thám" },
        { id: 5, name: "Thiếu nhi" },
        { id: 6, name: "Lãng mạn" },
        { id: 7, name: "Kinh dị" },
        { id: 8, name: "Tự truyện" },
        { id: 9, name: "Phiêu lưu" },
        { id: 10, name: "Lịch sử" },
    ]

    selectedCategoryId = 0

    ngOnInit() {
        // Initialize with the first category
    }

    selectCategory(categoryId: number) {
        this.selectedCategoryId = categoryId
        this.categorySelected.emit(categoryId)
    }
}

