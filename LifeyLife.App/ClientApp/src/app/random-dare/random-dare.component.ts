import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { API_ENDPOINTS } from '../shared/constants/api.constants';
import { LanguageService, LocalizationType } from '../shared/services/language.service';
import { TranslationService } from '../shared/services/translation.service';
import { AuthService } from '../shared/services/auth.service';

interface RandomDare {
  uuid: string;
  context: string;
  language: number;
  experienceGained: number;
  givenTime: number;
}

@Component({
  selector: 'app-random-dare',
  templateUrl: './random-dare.component.html',
  styleUrls: ['./random-dare.component.css']
})
export class RandomDareComponent implements OnInit {
  currentDare: RandomDare | null = null;
  isLoading = false;
  error: string | null = null;
  timer: number = 0;
  timerInterval: any;

  constructor(
    private http: HttpClient,
    private languageService: LanguageService,
    private translationService: TranslationService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.getRandomDare();
  }

  ngOnDestroy(): void {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }
  }

  getRandomDare(): void {
    this.isLoading = true;
    this.error = '';
    this.currentDare = null;
    this.timer = 0;

    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }

    // Get current language preference and always pass it as parameter
    const currentLanguage = this.languageService.getCurrentLanguage();
    let params = new HttpParams();
    params = params.set('language', currentLanguage.toString());

    this.http.get<RandomDare>(API_ENDPOINTS.RANDOM_DARE, { params })
      .subscribe({
        next: (dare) => {
          this.currentDare = dare;
          this.isLoading = false;
          this.startTimer(dare.givenTime);
        },
        error: (error) => {
          this.error = error.error.message || this.translationService.get('randomDare.failedToGet');
          this.isLoading = false;
        }
      });
  }

  startTimer(givenTime: number): void {
    this.timer = givenTime;
    this.timerInterval = setInterval(() => {
      if (this.timer > 0) {
        this.timer--;
      } else {
        clearInterval(this.timerInterval);
      }
    }, 1000);
  }

  completeDare(): void {
    if (!this.currentDare) return;

    // Check if user is authenticated
    if (!this.authService.isLoggedIn()) {
      this.error = 'Please login to complete dares';
      return;
    }
    
    this.isLoading = true;
    this.error = '';
    
    this.authService.authenticatedPost(API_ENDPOINTS.COMPLETE_DARE, this.currentDare)
      .subscribe({
        next: () => {
          this.currentDare = null;
          this.isLoading = false;
          this.getRandomDare();
        },
        error: (error) => {
          if (error.status === 401) {
            this.authService.logout();
            this.error = 'Please login to complete dares';
          } else {
            this.error = error.error.message || this.translationService.get('randomDare.failedToComplete');
          }
          this.isLoading = false;
        }
      });
  }


}
