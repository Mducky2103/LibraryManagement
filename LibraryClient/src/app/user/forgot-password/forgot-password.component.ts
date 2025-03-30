import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../shared/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgot-password',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './forgot-password.component.html',
  styles: ``
})
export class ForgotPasswordComponent implements OnInit {
  form: any;
  isSumitted: boolean = false;
  attemptCount: number = 0; // Đếm số lần gửi
  isCooldown: boolean = false; // Kiểm soát cooldown
  constructor(
    public formBuilder: FormBuilder,
    private service: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.form = this.formBuilder.group({
      email: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    if (this.service.isLoggedIn())
      this.router.navigateByUrl('/dashboard');
  }

  hasDisplayableError(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) &&
      (this.isSumitted || Boolean(control?.touched) ||
        Boolean(control?.dirty));
  }

  submitRequest() {
    if (this.form.invalid) return;

    // Kiểm tra nếu đang trong cooldown
    if (this.isCooldown) {
      this.toastr.warning('Bạn đã yêu cầu, vui lòng thử lại sau 1 phút.', 'Chờ giây lát');
      return;
    }

    // Kiểm tra nếu đã đạt giới hạn gửi
    if (this.attemptCount >= 4) {
      this.toastr.error('Bạn đã vượt quá số lần yêu cầu. Hãy thử lại sau 24 giờ.', 'Quá giới hạn');
      return;
    }
    const email = this.form.value.email;
    console.log('Payload:', { email });
    this.isCooldown = true; // Bắt đầu cooldown
    this.attemptCount++; // Tăng số lần gửi
    this.service.forgotPassword(email).subscribe({
      next: (response) => {
        this.toastr.success('Password reset link sent to your email.', 'Success');

        if (this.attemptCount >= 4) {
          setTimeout(() => {
            this.attemptCount = 0;
          }, 24 * 60 * 60 * 1000); // 24 giờ
        }
        setTimeout(() => {
          this.isCooldown = false;
        }, 60000);
      },
      error: (err) => {
        this.toastr.error(err.error?.message || 'An error occurred. Please try again.', 'Error');
        this.isCooldown = false;
      }
    });
  }
} 
