import { Component } from '@angular/core';
import { PenaltyService } from '../services/penalty.service';
import { Penalty } from '../model/penalty';
import { CommonModule } from '@angular/common';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-all-penalties',
  imports: [CommonModule, NgxPaginationModule, FormsModule],
  templateUrl: './all-penalties.component.html',
  styleUrl: './all-penalties.component.css'
})
export class AllPenaltiesComponent {
  penalties: Penalty[] = [];
  currentPage: number = 1;
  pageSize: number = 7;
  totalBooks: number = 0;
  totalPages: number = 0;
  constructor(private penaltyService: PenaltyService,
    private toastrService: ToastrService
  ) { }

  ngOnInit(): void {
    this.loadAllPenalties();
  }

  loadAllPenalties(): void {
    this.penaltyService.getAllPenalties().subscribe(data => {
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
    this.loadAllPenalties();
  }
}
