import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { UserService } from '../../shared/services/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  user: any = null;
  editProfile: any = {};
  isEditing = false;

  constructor(private router: Router,
    private authService: AuthService,
    private userService: UserService) { }
  ngOnInit(): void {
    this.userService.getUserProfile().subscribe({
      next: (data) => this.user = data,
      error: (err) => console.error('Lỗi khi lấy thông tin user:', err)
    });
  }
  toggleEdit() {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      // Gán giá trị từ user vào editProfile khi bật chế độ chỉnh sửa
      this.editProfile = { ...this.user };
    }
  }
  saveProfile() {
    this.userService.editUserProfile(this.editProfile).subscribe({
      next: (res) => {
        this.userService.getUserProfile().subscribe({
          next: (data) => {
            this.user = data; // Reload the updated user profile
            this.isEditing = false;
            this.editProfile = {}; // Reset the edit form
          },
          error: (err) => console.error('Lỗi khi tải lại thông tin user:', err)
        });
      },
      error: (err) => console.error('Lỗi khi lưu thông tin:', err)
    });
  }
}
