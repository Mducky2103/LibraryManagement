import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgxPaginationModule } from 'ngx-pagination';
import { Penalty } from '../model/penalty';
import { PenaltyService } from '../services/penalty.service';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-penalties',
  imports: [CommonModule, NgxPaginationModule],
  templateUrl: './user-penalties.component.html',
  styleUrl: './user-penalties.component.css'
})
export class UserPenaltiesComponent implements OnInit {
  penalties: Penalty[] = [];
  currentPage: number = 1;
  pageSize: number = 7;
  totalBooks: number = 0;
  totalPages: number = 0;
  public auth: any;
  constructor(private penaltyService: PenaltyService,
    private authService: AuthService,
    private toastrService: ToastrService
  ) { }

  ngOnInit(): void {
    this.auth = this.authService.getClaims();
    this.loadUserPenalties();
  }

  loadUserPenalties() {
    this.penaltyService.getPenaltyById(this.auth.userID).subscribe(data => {
      this.penalties = data;
      this.totalBooks = data.length;

      this.totalPages = Math.ceil(this.totalBooks / this.pageSize);

      const startIndex = (this.currentPage - 1) * this.pageSize;
      const endIndex = startIndex + this.pageSize;
      this.penalties = data.slice(startIndex, endIndex);
    });
  }
  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadUserPenalties();
  }
}
