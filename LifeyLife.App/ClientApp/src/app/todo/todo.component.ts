import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { API_ENDPOINTS } from '../shared/constants/api.constants';
import { AuthService } from '../shared/services/auth.service';
import { TranslationService } from '../shared/services/translation.service';

interface TodoItem {
  uuid: string;
  title: string;
  isCompleted: boolean;
  createdAt: string;
}

interface HistoryEntry {
  uuid: string;
  dayDate: string;
  finishedAt: string;
  completedCount: number;
  totalCount: number;
  experienceAwarded: number;
}

type ViewMode = 'today' | 'history';

@Component({
  selector: 'app-todo',
  templateUrl: './todo.component.html',
  styleUrls: ['./todo.component.css']
})
export class TodoComponent implements OnInit {
  view: ViewMode = 'today';

  items: TodoItem[] = [];
  history: HistoryEntry[] = [];
  newTitle = '';

  loading = false;
  finishing = false;
  finishResult: HistoryEntry | null = null;
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
    this.loadItems();
  }

  switchView(v: ViewMode): void {
    this.view = v;
    this.error = null;
    if (v === 'history') {
      this.loadHistory();
    }
  }

  // ── Today ────────────────────────────────────────────────────────

  loadItems(): void {
    this.loading = true;
    this.error = null;

    this.authService.authenticatedGet<TodoItem[]>(API_ENDPOINTS.TODO)
      .subscribe({
        next: (items) => {
          this.items = items;
          this.loading = false;
        },
        error: (err) => {
          if (err.status === 401) {
            this.authService.logout();
            this.router.navigate(['/login']);
          } else {
            this.error = this.translationService.get('todo.loadFailed');
            this.loading = false;
          }
        }
      });
  }

  addItem(): void {
    const title = this.newTitle.trim();
    if (!title) return;

    this.authService.authenticatedPost<TodoItem>(API_ENDPOINTS.TODO, { title })
      .subscribe({
        next: (item) => {
          this.items.push(item);
          this.newTitle = '';
        },
        error: () => {
          this.error = this.translationService.get('todo.addFailed');
        }
      });
  }

  toggle(item: TodoItem): void {
    this.authService.authenticatedPut<void>(
      `${API_ENDPOINTS.TODO}/${item.uuid}/toggle`, {}
    ).subscribe({
      next: () => {
        item.isCompleted = !item.isCompleted;
      }
    });
  }

  deleteItem(item: TodoItem): void {
    this.authService.authenticatedDelete(
      `${API_ENDPOINTS.TODO}/${item.uuid}`
    ).subscribe({
      next: () => {
        this.items = this.items.filter(i => i.uuid !== item.uuid);
      }
    });
  }

  finishDay(): void {
    this.finishing = true;
    this.error = null;

    this.authService.authenticatedPost<HistoryEntry>(API_ENDPOINTS.TODO_FINISH_DAY, {})
      .subscribe({
        next: (result) => {
          this.finishResult = result;
          this.items = [];
          this.finishing = false;
        },
        error: () => {
          this.error = this.translationService.get('todo.finishFailed');
          this.finishing = false;
        }
      });
  }

  // ── History ──────────────────────────────────────────────────────

  loadHistory(): void {
    this.loading = true;

    this.authService.authenticatedGet<HistoryEntry[]>(API_ENDPOINTS.TODO_HISTORY)
      .subscribe({
        next: (h) => {
          this.history = h;
          this.loading = false;
        },
        error: () => {
          this.error = this.translationService.get('todo.historyFailed');
          this.loading = false;
        }
      });
  }

  // ── Helpers ──────────────────────────────────────────────────────

  formatDate(dateStr: string): string {
    const d = new Date(dateStr);
    return d.toLocaleDateString(undefined, { day: '2-digit', month: 'short', year: 'numeric' });
  }

  get completedCount(): number {
    return this.items.filter(i => i.isCompleted).length;
  }
}
