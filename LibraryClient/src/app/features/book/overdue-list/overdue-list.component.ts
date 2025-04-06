import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { BorrowService } from '../services/borrow.service';

@Component({
  selector: 'app-overdue-list',
  imports: [CommonModule, NgxPaginationModule, FormsModule],
  templateUrl: './overdue-list.component.html',
  styleUrl: './overdue-list.component.css'
})
export class OverdueListComponent implements OnInit {
  public overdueList: any[] = [];
  currentPage: number = 1;
  pageSize: number = 7;
  totalBooks: number = 0;
  totalPages: number = 0;

  constructor(private service: BorrowService,
    private authService: AuthService,
    private toatrService: ToastrService) { }

  ngOnInit(): void {
    this.onGetData();
  }

  onGetData() {
    this.service.getOverdueList().subscribe(
      (data) => {
        this.overdueList = data;

        this.totalBooks = this.overdueList.length;
        this.totalPages = Math.ceil(this.totalBooks / this.pageSize);

        const startIndex = (this.currentPage - 1) * this.pageSize;
        const endIndex = startIndex + this.pageSize;
        this.overdueList = this.overdueList.slice(startIndex, endIndex);
      },
      (error) => {
        console.error('Error fetching pending borrow requests', error);
      }
    );
  }
  onPageChange(page: number): void {
    this.currentPage = page;
    this.onGetData();
  }
}
