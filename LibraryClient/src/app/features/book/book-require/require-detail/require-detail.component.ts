import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BooksService } from '../../services/books.service';
import { ActivatedRoute } from '@angular/router';
import { BorrowService } from '../../services/borrow.service';
import { AuthService } from '../../../../shared/services/auth.service';
import { Book } from '../../models/book';

@Component({
  selector: 'app-require-detail',
  imports: [],
  templateUrl: './require-detail.component.html',
  styleUrl: './require-detail.component.css'
})
export class RequireDetailComponent implements OnInit {
  public idBook: any;
  public book: any;
  public aut: any;
  constructor(private service: BooksService,
    private route: ActivatedRoute,
    private serviceBorrow: BorrowService,
    private auth: AuthService,
    private toastService: ToastrService
  ) {
    this.aut = this.auth.getClaims();
    this.idBook = route.snapshot.paramMap.get('id');

  }
  ngOnInit(): void {

    this.route.params.subscribe((params) => {
      this.idBook = params['id'];
      this.onGetData(this.idBook);
    });
  }
  onGetData(id: any) {
    this.service.getBookById(this.idBook).subscribe((data) => {
      this.book = data as Book;
      console.log("book", this.book)
    })
  }
  onBorrow() {
    let formData = {
      userId: this.aut.userID,
      bookIds:
        [
          parseInt(this.idBook)
        ]
    }
    console.log(formData);
    this.serviceBorrow.borrow(formData).subscribe((data) => {
      this.toastService.success("Borrow book successfully");
    },
      (error) => {
        this.toastService.error("Borrow book failed");
      });
  }
}
