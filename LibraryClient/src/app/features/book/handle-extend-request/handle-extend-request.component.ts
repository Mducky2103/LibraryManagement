import { Component } from '@angular/core';
import { BorrowService } from '../services/borrow.service';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-handle-extend-request',
  imports: [CommonModule, NgxPaginationModule, FormsModule],
  templateUrl: './handle-extend-request.component.html',
  styleUrl: './handle-extend-request.component.css'
})
export class HandleExtendRequestComponent {
  public extendBorrowRequests: any[] = [];
  currentPage: number = 1;
  pageSize: number = 7;
  totalBooks: number = 0;
  totalPages: number = 0;
  hasData: boolean = false

  constructor(private service: BorrowService,
    private authService: AuthService,
    private toatrService: ToastrService) { }

  ngOnInit(): void {
    this.onGetData();
  }

  onGetData() {
    this.service.getExtendRequestList().subscribe(
      (data) => {
        this.extendBorrowRequests = data || [];
        this.hasData = this.extendBorrowRequests.length > 0;

        if (this.hasData) {
          this.totalBooks = this.extendBorrowRequests.length;
          this.totalPages = Math.ceil(this.totalBooks / this.pageSize);

          const startIndex = (this.currentPage - 1) * this.pageSize;
          const endIndex = startIndex + this.pageSize;
          this.extendBorrowRequests = this.extendBorrowRequests.slice(startIndex, endIndex);
        } else {
          this.totalPages = 0;
          this.currentPage = 1;
        }
      },
      (error) => {
        console.error('Error fetching pending borrow requests', error);
        this.hasData = false;
        this.totalPages = 0;
        this.extendBorrowRequests = [];
      }
    );
  }

  approveRequest(detailId: number): void {
    this.service.approveExtendDueDate(detailId, true, 'Extending Date Request Approved by librarian').subscribe({
      next: (response) => {
        this.toatrService.success('Request approved successfully!');
        this.onGetData();
      },
      error: (error) => {
        console.error('Error approving request:', error);
        this.toatrService.error('Failed to approve request.');
      }
    });
  }
  cancelRequest(detailId: number): void {
    this.service.approveExtendDueDate(detailId, false, 'Extension request not approved').subscribe({
      next: (response) => {
        this.toatrService.success('Request canceled successfully!');
        this.onGetData();
      },
      error: (error) => {
        console.error('Error canceling request:', error);
        this.toatrService.error('Failed to cancel request.');
      }
    });
  }


  onPageChange(page: number): void {
    if (this.hasData) {
      this.currentPage = page;
      this.onGetData();
    }
  }

} 
