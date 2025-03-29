import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_ENDPOINTS } from '../shared/constants/api.constants';

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

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadHistory();
  }

  loadHistory(): void {
    this.loading = true;
    this.error = null;

    this.http.get<DareHistory[]>(API_ENDPOINTS.RANDOM_DARE_HISTORY)
      .subscribe({
        next: (history) => {
          this.history = history;
          this.loading = false;
        },
        error: (error) => {
          this.error = error.error.message || 'Failed to load history';
          this.loading = false;
        }
      });
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleString();
  }
}
