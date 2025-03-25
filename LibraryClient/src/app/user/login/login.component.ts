import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styles: ``
})
export class LoginComponent implements OnInit {
  form: any;
  isSumitted: boolean = false;
  constructor(
    public formBuilder: FormBuilder,
    private service: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.form = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
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

  showPassword: boolean = false;

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    this.isSumitted = true;
    if (this.form.valid) {
      this.service.signin(this.form.value).subscribe({
        next: (res: any) => {
          this.service.saveToken(res.token);
          this.router.navigateByUrl('/dashboard');
        },
        error: err => {
          if (err.status == 400)
            this.toastr.error('Incorrect email or password.', 'Login Failed')
          else
            console.log('Error During Login:\n', err);
        }
      })
    }
  }
}
