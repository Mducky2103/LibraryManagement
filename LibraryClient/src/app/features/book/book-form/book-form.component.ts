import { Component, OnInit } from '@angular/core';
import { NavbarLibrarianComponent } from '../../../core/navbar-librarian/navbar-librarian.component';
import { FooterLibrarianComponent } from '../../../core/footer-librarian/footer-librarian.component';
import { FormBuilder, FormGroup, NgModel, ReactiveFormsModule, Validators } from '@angular/forms';
import { BooksService } from '../services/books.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-book-form',
  imports: [NavbarLibrarianComponent, FooterLibrarianComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './book-form.component.html',
  styleUrl: './book-form.component.css'
})
export class BookFormComponent implements OnInit {
  bookForm: FormGroup;
  isEdit: boolean = false;
  bookId: number | null = null;
  authors: string[] = [];  // Danh sách tác giả (giả định)
  categories: string[] = [];  // Danh sách thể loại (giả định)
  selectedImage: File | null = null;
  imagePreview: string | null = null;

  constructor(
    private fb: FormBuilder,
    private bookService: BooksService,
    private router: Router,
    private route: ActivatedRoute
    // private authorService: AuthorService,
    // private categoryService: CategoryService
  ) {
    // Khởi tạo form
    this.bookForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      yearPublished: ['', [Validators.required, Validators.pattern('^[0-9]{4}$')]],
      price: ['', [Validators.required, Validators.min(0)]],
      quantity: ['', [Validators.required, Validators.min(0)]],
      image: [''],
      isAvailable: [true],
      author: ['', Validators.required],
      category: ['', Validators.required]
    });
  }

  onFileChange(event: any): void {
    if (event.target.files && event.target.files[0]) {
      this.selectedImage = event.target.files[0];

      // Kiểm tra nếu tệp tồn tại và là một đối tượng Blob (tệp ảnh)
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result as string;
      };

      // reader.readAsDataURL(this.selectedImage);
    }
  }

  ngOnInit(): void {
    // Lấy các tham số từ URL (để kiểm tra xem đây là thêm hay sửa)
    this.route.params.subscribe(params => {
      this.bookId = params['id'];
      if (this.bookId) {
        this.isEdit = true;
        // this.loadBookDetails(this.bookId);
      }
    });

    // this.loadAuthors();
    // this.loadCategories();
  }

  // Lấy dữ liệu sách nếu là sửa
  // loadBookDetails(id: number) {
  //   this.bookService.getBookById(id).subscribe(book => {
  //     this.bookForm.patchValue({
  //       name: book.name,
  //       description: book.description,
  //       yearPublished: book.yearPublished,
  //       price: book.price,
  //       quantity: book.quantity,
  //       image: book.image,
  //       isAvailable: book.isAvailable,
  //       author: book.authorName,
  //       category: book.categoryName
  //     });
  //   });
  // }

  // loadAuthors() {
  //   this.authorService.getAuthors().subscribe(authors => {
  //     this.authors = authors;
  //   });
  // }

  // loadCategories() {
  //   this.categoryService.getCategories().subscribe(categories => {
  //     this.categories = categories;
  //   });
  // }

  onSubmit() {
    if (this.bookForm.invalid) {
      return;
    }

    const bookData = this.bookForm.value;
    const formData = new FormData();

    formData.append('name', bookData.name);
    formData.append('description', bookData.description);
    formData.append('yearPublished', bookData.yearPublished);
    formData.append('price', bookData.price);
    formData.append('quantity', bookData.quantity);
    formData.append('isAvailable', bookData.isAvailable);
    formData.append('author', bookData.author);
    formData.append('category', bookData.category);

    if (this.selectedImage) {
      formData.append('image', this.selectedImage, this.selectedImage.name);
    }

    if (this.isEdit && this.bookId !== null) {
      // this.bookService.updateBook(this.bookId, bookData).subscribe(() => {
      //   this.router.navigate(['/book']);
      // });
    } else {

      this.bookService.addBook(bookData).subscribe(() => {
        this.router.navigate(['/book']);
      });
    }
  }

  onCancel() {
    this.router.navigate(['/book']);
  }
}
