import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { API_ENDPOINTS } from '../../shared/constants/api.constants';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  error: string = '';
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, { validator: this.passwordMatchValidator });
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password')?.value === g.get('confirmPassword')?.value
      ? null
      : { 'mismatch': true };
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.isLoading = true;
      this.error = '';

      const { email, password, confirmPassword } = this.registerForm.value;

      this.http.post(API_ENDPOINTS.REGISTRATION, {
        email,
        password,
        confirmPassword
      }).subscribe({
        next: () => {
          this.router.navigate(['/login']);
        },
        error: (error) => {
          this.error = error.error.error || 'Registration failed';
          this.isLoading = false;
        }
      });
    }
  }
} 