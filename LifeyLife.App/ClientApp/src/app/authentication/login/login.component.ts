import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { API_ENDPOINTS } from '../../shared/constants/api.constants';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  error: string = '';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    // If already logged in, redirect to home
    if (localStorage.getItem('token')) {
      this.router.navigate(['/home']);
    }
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      
      this.http.post(API_ENDPOINTS.LOGIN, { email, password })
        .subscribe({
          next: (response: any) => {
            localStorage.setItem('token', response.token);
            this.router.navigate(['/home']).then(() => {
              window.location.reload(); // Force a reload to update the navigation state
            });
          },
          error: (error) => {
            this.error = error.error.message || 'An error occurred during login';
          }
        });
    }
  }
} 