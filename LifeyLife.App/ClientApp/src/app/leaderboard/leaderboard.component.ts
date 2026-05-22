import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { API_ENDPOINTS } from '../shared/constants/api.constants';
import { AuthService } from '../shared/services/auth.service';
import { TranslationService } from '../shared/services/translation.service';

interface LeaderboardEntry {
  rank: number;
  displayName: string;
  totalExperience: number;
  totalLevel: number;
}

@Component({
  selector: 'app-leaderboard',
  templateUrl: './leaderboard.component.html',
  styleUrls: ['./leaderboard.component.css']
})
export class LeaderboardComponent implements OnInit {
  entries: LeaderboardEntry[] = [];
  loading = false;
  error: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private translationService: TranslationService
  ) {}

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadLeaderboard();
  }

  loadLeaderboard(): void {
    this.loading = true;
    this.error = null;

    this.authService.authenticatedGet<LeaderboardEntry[]>(API_ENDPOINTS.LEADERBOARD)
      .subscribe({
        next: (data) => {
          this.entries = data;
          this.loading = false;
        },
        error: (err) => {
          if (err.status === 401) {
            this.authService.logout();
            this.router.navigate(['/login']);
          } else {
            this.error = this.translationService.get('leaderboard.failed');
            this.loading = false;
          }
        }
      });
  }

  rankLabel(rank: number): string {
    if (rank === 1) return '🥇';
    if (rank === 2) return '🥈';
    if (rank === 3) return '🥉';
    return `#${rank}`;
  }
}
