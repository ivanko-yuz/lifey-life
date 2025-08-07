import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { API_ENDPOINTS } from '../../shared/constants/api.constants';
import { LanguageService, LocalizationType } from '../../shared/services/language.service';
import { TranslationService } from '../../shared/services/translation.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  error: string = '';
  isLoading: boolean = false;
  languages = this.languageService.getAvailableLanguages();

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private languageService: LanguageService,
    private translationService: TranslationService
  ) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
      preferredLanguage: [LocalizationType.ua, [Validators.required]]
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

      const { email, password, confirmPassword, preferredLanguage } = this.registerForm.value;

      this.http.post(API_ENDPOINTS.REGISTRATION, {
        email,
        password,
        confirmPassword,
        preferredLanguage
      }).subscribe({
        next: () => {
          this.router.navigate(['/login']);
        },
        error: (error) => {
          this.error = error.error.error || this.translationService.get('auth.registrationFailed');
          this.isLoading = false;
        }
      });
    }
  }
} 