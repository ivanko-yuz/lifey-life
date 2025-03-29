import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_ENDPOINTS } from '../shared/constants/api.constants';

interface RandomDare {
  uuid: string;
  context: string;
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

  constructor(private http: HttpClient) {}

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

    this.http.get<RandomDare>(API_ENDPOINTS.RANDOM_DARE)
      .subscribe({
        next: (dare) => {
          this.currentDare = dare;
          this.isLoading = false;
          this.startTimer(dare.givenTime);
        },
        error: (error) => {
          this.error = error.error.message || 'Failed to get random dare';
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
    
    this.isLoading = true;
    this.error = '';
    
    this.http.post(API_ENDPOINTS.COMPLETE_DARE(this.currentDare.uuid), this.currentDare)
      .subscribe({
        next: () => {
          this.currentDare = null;
          this.isLoading = false;
          this.getRandomDare();
        },
        error: (error) => {
          this.error = error.error.message || 'Failed to complete dare';
          this.isLoading = false;
        }
      });
  }
}
