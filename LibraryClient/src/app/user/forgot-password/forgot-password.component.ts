import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../shared/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgot-password',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './forgot-password.component.html',
  styles: ``
})
export class ForgotPasswordComponent implements OnInit {
  form: any;
  isSumitted: boolean = false;
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

    const email = this.form.value.email;
    console.log('Payload:', { email });
    this.service.forgotPassword(email).subscribe({
      next: (response) => {
        this.toastr.success('Password reset link sent to your email.', 'Success');
      },
      error: (err) => {
        this.toastr.error(err.error?.message || 'An error occurred. Please try again.', 'Error');
      }
    });
  }
} 
