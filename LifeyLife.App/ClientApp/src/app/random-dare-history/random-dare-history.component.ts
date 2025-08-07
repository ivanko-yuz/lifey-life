import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { API_ENDPOINTS } from '../shared/constants/api.constants';
import { AuthService } from '../shared/services/auth.service';
import { TranslationService } from '../shared/services/translation.service';

interface DareHistory {
  uuid: string;
  context: string;
  completedAt: string;
  experienceGained: number;
}

@Component({
  selector: 'app-random-dare-history',
  templateUrl: './random-dare-history.component.html',
  styleUrls: ['./random-dare-history.component.css']
})
export class RandomeDareHistoryComponent implements OnInit {
  history: DareHistory[] = [];
  loading = false;
  error: string | null = null;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router,
    private translationService: TranslationService
  ) {}

  ngOnInit(): void {
    this.loadHistory();
  }

  loadHistory(): void {
    // Check if user is authenticated
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.loading = true;
    this.error = null;

    this.authService.authenticatedGet<DareHistory[]>(API_ENDPOINTS.RANDOM_DARE_HISTORY)
      .subscribe({
        next: (history) => {
          this.history = history;
          this.loading = false;
        },
        error: (error) => {
          if (error.status === 401) {
            // Token expired or invalid
            this.authService.logout();
            this.router.navigate(['/login']);
          } else {
            this.error = error.error.message || this.translationService.get('history.failed');
            this.loading = false;
          }
        }
      });
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleString();
  }
}
