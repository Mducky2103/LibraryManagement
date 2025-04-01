import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './reset-password.component.html',
  styles: ``
})
export class ResetPasswordComponent implements OnInit {
  form: any;
  email: string = '';
  token: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: AuthService,
    private toastr: ToastrService
  ) {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      token: ['', Validators.required],
      newPassword: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.pattern(/(?=.*[^a-zA-Z0-9])/)]]
    });
  }

  ngOnInit(): void {
    if (this.service.isLoggedIn())
      this.router.navigateByUrl('/dashboard');

    this.route.queryParams.subscribe(params => {
      this.email = params['email'] || '';
      this.token = decodeURIComponent(params['token'] || ''); // Giải mã token

      this.form.patchValue({
        email: this.email,
        token: this.token
      });
    });
  }

  hasDisplayableError(controlName: string, error: string): boolean {
    const control = this.form.get(controlName);
    return control?.hasError(error) && (control.dirty || control.touched);
  }

  submitReset() {
    if (this.form.invalid) return;

    const payload = {
      email: this.email,
      token: this.token,
      newPassword: this.form.value.newPassword
    };

    // console.log('Submitting reset request:', payload);

    this.service.resetPassword(payload).subscribe({
      next: () => {
        this.toastr.success('Password reset successfully.', 'Success');
        this.router.navigate(['/signin']);
      },
      error: (err) => {
        this.toastr.error(err.error?.message || 'Error resetting password.', 'Error');
      }
    });
  }
}
