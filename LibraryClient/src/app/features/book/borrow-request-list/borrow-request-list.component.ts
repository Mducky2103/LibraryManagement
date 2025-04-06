import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { BorrowService } from '../services/borrow.service';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-borrow-request-list',
  imports: [CommonModule, NgxPaginationModule, FormsModule],
  templateUrl: './borrow-request-list.component.html',
  styleUrl: './borrow-request-list.component.css'
})
export class BorrowRequestListComponent implements OnInit {
  public borrowRequests: any[] = [];
  currentPage: number = 1;
  pageSize: number = 7;
  totalBooks: number = 0;
  totalPages: number = 0;

  rejectingRequestId: number | null = null;
  rejectionNotes: string = '';
  showRejectModal: boolean = false;

  constructor(private service: BorrowService,
    private authService: AuthService,
    private toatrService: ToastrService) { }

  ngOnInit(): void {
    this.onGetData();
  }

  onGetData() {
    this.service.getBorrowRequestList().subscribe(
      (data) => {
        this.borrowRequests = data;

        this.totalBooks = this.borrowRequests.length;
        this.totalPages = Math.ceil(this.totalBooks / this.pageSize);

        const startIndex = (this.currentPage - 1) * this.pageSize;
        const endIndex = startIndex + this.pageSize;
        this.borrowRequests = this.borrowRequests.slice(startIndex, endIndex);
      },
      (error) => {
        console.error('Error fetching pending borrow requests', error);
      }
    );
  }

  approveRequest(id: number): void {
    this.service.approveRequest(id).subscribe({
      next: (response) => {
        console.log(response.message);
        this.toatrService.success('Request approved successfully!');
        this.onGetData(); // Refresh the list after approval
      },
      error: (error) => {
        console.error('Error approving borrow request:', error);
        this.toatrService.error('Error approving request!');
      }
    });
  }

  openRejectModal(id: number): void {
    this.rejectingRequestId = id;
    this.rejectionNotes = '';
    this.showRejectModal = true;
  }
  closeRejectModal(): void {
    this.showRejectModal = false;
    this.rejectingRequestId = null;
    this.rejectionNotes = '';
  }

  submitRejection(): void {
    if (this.rejectingRequestId && this.rejectionNotes.trim()) {
      this.service.rejectBorrowRequest(this.rejectingRequestId, this.rejectionNotes).subscribe({
        next: (response) => {
          this.toatrService.success('Request rejected successfully!');
          this.onGetData();
          this.closeRejectModal();
        },
        error: (error) => {
          this.toatrService.error('Error rejecting request!');
          console.error(error);
        }
      });
    } else {
      this.toatrService.warning('Please enter a reason for rejection!');
    }
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.onGetData();
  }

} 
