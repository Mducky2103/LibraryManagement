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
  categoryForm: FormGroup;
  authorForm: FormGroup;
  isEdit: boolean = false;
  bookId: number | null = null;
  selectedImage: any;
  file!: File;
  authors: any[] = [];
  categories: any[] = [];
  uploadProgress: number = 0;
  isDropdownOpen = false;
  isDropdownAuthorOpen = false;
  selectedCategory: any = null;
  selectedAuthor: any = null;

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

    this.categoryForm = this.fb.group({
      name: ['', Validators.required]
    });

    this.authorForm = this.fb.group({
      name: ['', Validators.required],
      address: ['', Validators.required]
    });
  }

  saveAuthor() {
    if (this.authorForm.valid) {
      const newAuthor = this.authorForm.value;
      this.authorService.addAuthor(newAuthor).subscribe(
        (response: any) => {
          this.authorForm.reset();
          (window as any).$('#addAuthorModal').modal('hide');
          alertifyjs.success('Author added successfully');
          this.loadAuthors();
        },
        (error: any) => {
          console.error('Error adding author:', error);
          alertifyjs.error('Failed to add auhtor');
        }
      );
    }
  }

  saveCategory(): void {
    if (this.categoryForm.valid) {
      const newCategory = this.categoryForm.value;
      this.categoryService.addCategory(newCategory).subscribe(
        (response: any) => {
          this.categoryForm.reset();
          (window as any).$('#addCategoryModal').modal('hide');
          alertifyjs.success('Category added successfully');
          this.loadCategories();
        },
        (error: any) => {
          console.error('Error adding category:', error);
          alertifyjs.error('Failed to add category');
        }
      );
    }
  }

  deleteCategory(categoryId: number) {
    if (confirm('Are you sure you want to delete this category?')) {
      this.categoryService.deleteCategory(categoryId).subscribe(
        () => {
          (window as any).$('#listCategoryModal').modal('hide');
          alertifyjs.success('Category delete successfully');
          this.loadCategories();
        },
        (error) => {
          console.error('Error deleting category:', error);
          alertifyjs.error('Failed to deleting category');
        }
      );
    }
  }

  deleteAuthor(authorId: number) {
    if (confirm('Are you sure you want to delete this author?')) {
      this.authorService.deleteAuthor(authorId).subscribe(
        () => {
          (window as any).$('#listAuthorModal').modal('hide');
          alertifyjs.success('Author delete successfully');
          this.loadAuthors();
        },
        (error) => {
          console.error('Error deleting author:', error);
          alertifyjs.error('Failed to deleting author');
        }
      );
    }
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
          }, 500);

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

        this.loadBookDetails(this.bookId);
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

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  toggleDropdownAuthor() {
    this.isDropdownAuthorOpen = !this.isDropdownAuthorOpen;
  }

  selectCategory(category: any) {
    this.selectedCategory = category;

    this.bookForm.get('category')?.setValue(category.id);

    this.isDropdownOpen = false;
  }

  selectAuthor(author: any) {
    this.selectedAuthor = author;

    this.bookForm.get('author')?.setValue(author.id);

    this.isDropdownAuthorOpen = false;
  }

  loadBookDetails(bookId: number): void {
    this.bookService.getBookById(bookId).subscribe(
      (book: any) => {
        this.bookForm.patchValue({
          name: book.name,
          description: book.description,
          yearPublished: book.yearPublished,
          price: book.price,
          quantity: book.quantity,
          isAvailable: book.isAvailable,
          author: book.authorId,
          category: book.categoryId,
          image: book.image
        });

        this.selectedCategory = this.categories.find(cat => cat.id === book.categoryId) || null;
        this.selectedAuthor = this.authors.find(auth => auth.id === book.authorId) || null;

        if (book.image) {
          this.loadBookImage(book.image);
        }
      },
      (error: any) => {
        console.error('Error loading book details:', error);
        alertifyjs.error('Failed to load book details');
      }
    );
  }

  loadBookImage(imagePath: string): void {
    this.bookService.getBookImage(imagePath).subscribe(
      (imageBlob) => {
        const imageUrl = URL.createObjectURL(imageBlob);
        this.selectedImage = imageUrl;
      },
      (error) => {
        console.error('Error loading book image:', error);
        this.selectedImage = null;
      }
    );
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
    } else if (this.isEdit && this.bookForm.get('image')?.value) {
      formData.append('image', this.bookForm.get('image')?.value);
    } else {
      formData.append('picture', '');

      formData.append('image', '');
    }

    if (this.isEdit && this.bookId !== null) {
      this.bookService.updateBook(this.bookId, formData).subscribe(
        () => {
          this.router.navigate(['/book']);
          alertifyjs.success('Book updated successfully!');
        },
        (error) => {
          console.error('Update Error:', error);
          if (error.error.errors) {
            let errorMessage = '';
            for (const field in error.error.errors) {
              if (error.error.errors[field]) {
                errorMessage += `${field}: ${error.error.errors[field].join(', ')}\n`;
              }
            }
            alertifyjs.error(errorMessage);
          } else {
            alertifyjs.error('An error occurred while updating the book.');
          }
        }
      );
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
