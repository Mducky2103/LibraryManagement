import { Component, OnInit } from '@angular/core';
import { NavbarLibrarianComponent } from '../../../core/navbar-librarian/navbar-librarian.component';
import { FooterLibrarianComponent } from '../../../core/footer-librarian/footer-librarian.component';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BooksService } from '../services/books.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import * as alertifyjs from 'alertifyjs';
import { CategoryService } from '../../category/services/category.service';
import { AuthorService } from '../../author/services/author.service';

@Component({
  selector: 'app-book-form',
  imports: [NavbarLibrarianComponent, FooterLibrarianComponent, ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './book-form.component.html',
  styleUrl: './book-form.component.css'
})
export class BookFormComponent implements OnInit {
  bookForm: FormGroup;
  isEdit: boolean = false;
  bookId: number | null = null;
  selectedImage: any;
  file!: File;
  authors: any[] = [];
  categories: any[] = [];
  uploadProgress: number = 0;

  constructor(
    private fb: FormBuilder,
    private bookService: BooksService,
    private categoryService: CategoryService,
    private authorService: AuthorService,
    private router: Router,
    private route: ActivatedRoute
  ) {
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

  onchange(event: any) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.file = input.files[0];

      const reader = new FileReader();

      let progress = 0;

      this.uploadProgress = progress;

      const interval = setInterval(() => {
        progress += 10;

        this.uploadProgress = progress;

        if (progress >= 100) {
          clearInterval(interval);

          setTimeout(() => {
            this.uploadProgress = 0;
          }, 1000);

        }
      }, 200);

      reader.onload = () => {
        this.selectedImage = reader.result;
      };

      reader.readAsDataURL(this.file);
    }

  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {

      this.bookId = params['id'];

      if (this.bookId) {

        this.isEdit = true;
        // Load book details for editing
        // this.loadBookDetails(this.bookId);
      }
    });

    this.loadAuthors();

    this.loadCategories();
  }

  loadAuthors(): void {
    this.authorService.getAllAuthors().subscribe((data: any[]) => {
      this.authors = data;
    });
  }

  loadCategories(): void {
    this.categoryService.getAllCategories().subscribe((data: any[]) => {
      this.categories = data;
    });
  }

  onSubmit(): void {
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

    formData.append('authorId', bookData.author);

    formData.append('categoryId', bookData.category);

    if (this.selectedImage) {
      formData.append('picture', this.file, this.file.name);

      formData.append('image', this.file.name);
    } else {
      formData.append('picture', '');

      formData.append('image', '');
    }

    if (this.isEdit && this.bookId !== null) {
      // this.bookService.updateBook(this.bookId, formData).subscribe(() => {
      //   this.router.navigate(['/book']);
      // });
    } else {
      this.bookService.addBook(formData).subscribe(() => {
        this.router.navigate(['/book']);

        alertifyjs.success("Add book successful!");
      }, (error) => {
        console.error("API Error: ", error);

        if (error.error.errors) {
          let errorMessage = '';

          for (const field in error.error.errors) {
            if (error.error.errors[field]) {
              errorMessage += `${field}: ${error.error.errors[field].join(', ')}\n`;
            }
          }
          alertifyjs.error(errorMessage);
        } else {
          alertifyjs.error("An error occurred while adding the book.");
        }
      });
    }
  }

}
