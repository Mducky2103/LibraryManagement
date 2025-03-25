import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { FirstKeyPipe } from '../../shared/pipes/first-key.pipe';
import { AuthService } from '../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-registration',
  imports: [ReactiveFormsModule,
    CommonModule, FirstKeyPipe, RouterLink],
  templateUrl: './registration.component.html',
  styles: ``
})
export class RegistrationComponent implements OnInit {
  form: any;
  isSumitted: boolean = false;

  // Custom validator for gender
  genderValidator: ValidatorFn = (control: AbstractControl): { [key: string]: any } | null => {
    const validGenders = ['Male', 'Female', 'male', 'female']; // Define valid options
    const value = control.value?.charAt(0).toUpperCase() + control.value?.slice(1).toLowerCase(); // Capitalize the first letter
    return validGenders.includes(control.value) ? null : { invalidGender: true };
  };

  // Custom validator for age
  ageValidator: ValidatorFn = (control: AbstractControl): { [key: string]: any } | null => {
    const value = control.value;
    if (!value || isNaN(value) || value < 18 || value > 100) {
      return { invalidAge: true }; // Return error if age is invalid
    }
    return null; // Valid age
  };

  passwordMatchValidator: ValidatorFn = (control: AbstractControl): null => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword?.setErrors({ passwordMismatch: true });
    }
    else
      confirmPassword?.setErrors(null);
    return null;
  }

  constructor(
    public formBuilder: FormBuilder,
    private service: AuthService,
    private toastr: ToastrService,
    private router: Router) {
    this.form = this.formBuilder.group({
      fullName: ['', Validators.required],
      email: ['', [
        Validators.required,
        Validators.email]],
      password: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.pattern(/(?=.*[^a-zA-Z0-9])/)]],
      confirmPassword: [''],
      gender: ['', [Validators.required, this.genderValidator]],
      age: ['', [Validators.required, this.ageValidator]]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    if (this.service.isLoggedIn())
      this.router.navigateByUrl('/dashboard');
  }

  onSubmit() {
    this.isSumitted = true;
    if (this.form.valid) {
      this.service.createUser(this.form.value)
        .subscribe({
          next: (res: any) => {
            if (res.succeeded) {
              this.form.reset();
              this.isSumitted = false;
              this.toastr.success('User created successfully', 'Registration successful');
            }
          },
          error: err => {
            if (err.error.errors)
              err.error.errors.forEach((x: any) => {
                switch (x.code) {
                  case "DuplicateUserName":
                    break;

                  case "DuplicateEmail":
                    this.toastr.error('Email is already taken', 'Registration failed');
                    break;
                  default:
                    this.toastr.error('Contact admin', 'Registration failed');
                    console.log(x);
                    break;
                }
              })
            else
              console.log(err);
          }
        });
    }

  }

  showPassword: boolean = false;
  showConfirmPassword: boolean = false;

  togglePasswordVisibility(field: 'password' | 'confirmPassword') {
    if (field === 'password') {
      this.showPassword = !this.showPassword;
    } else {
      this.showConfirmPassword = !this.showConfirmPassword;
    }
  }


  hasDisplayableError(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) &&
      (this.isSumitted || Boolean(control?.touched) ||
        Boolean(control?.dirty));
  }
}
