import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { API_ENDPOINTS } from '../shared/constants/api.constants';
import { AuthService } from '../shared/services/auth.service';
import { TranslationService } from '../shared/services/translation.service';

interface CharacterStats {
  userUuid: string;
  strength: number;
  intelligence: number;
  charisma: number;
  dexterity: number;
  vitality: number;
  willpower: number;
  systematization: number;
  totalExperience: number;
}

interface StatDisplay {
  key: keyof Omit<CharacterStats, 'userUuid' | 'totalExperience'>;
  labelKey: string;
  icon: string;
  color: string;
  isWide: boolean;
  points: number;
  level: number;
  progress: number;   // 0-100 (% toward next level)
  nextLevel: number;  // XP needed for next level
}

@Component({
  selector: 'app-character',
  templateUrl: './character.component.html',
  styleUrls: ['./character.component.css']
})
export class CharacterComponent implements OnInit {
  stats: CharacterStats | null = null;
  statList: StatDisplay[] = [];
  totalLevel = 0;
  loading = false;
  error: string | null = null;

  private readonly POINTS_PER_LEVEL = 100;

  // Layout: Vitality (full-width top), then 2-column grid:
  //   Strength     | Intelligence
  //   Charisma     | Dexterity
  //   Systematization | Willpower
  private readonly STAT_DEFS: Array<{
    key: keyof Omit<CharacterStats, 'userUuid' | 'totalExperience'>;
    labelKey: string;
    icon: string;
    color: string;
    isWide: boolean;
  }> = [
    { key: 'vitality',        labelKey: 'character.vitality',        icon: '💚',  color: '#22C55E', isWide: true  },
    { key: 'strength',        labelKey: 'character.strength',        icon: '⚔️',  color: '#FF8C00', isWide: false },
    { key: 'intelligence',    labelKey: 'character.intelligence',    icon: '📚',  color: '#F5C018', isWide: false },
    { key: 'charisma',        labelKey: 'character.charisma',        icon: '🎭',  color: '#F5A623', isWide: false },
    { key: 'dexterity',       labelKey: 'character.dexterity',       icon: '⚡',  color: '#FF8C00', isWide: false },
    { key: 'systematization', labelKey: 'character.systematization', icon: '📋',  color: '#3B82F6', isWide: false },
    { key: 'willpower',       labelKey: 'character.willpower',       icon: '🔥',  color: '#F5C018', isWide: false },
  ];

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
    this.loadStats();
  }

  loadStats(): void {
    this.loading = true;
    this.error = null;

    this.authService.authenticatedGet<CharacterStats>(API_ENDPOINTS.CHARACTER)
      .subscribe({
        next: (stats) => {
          this.stats = stats;
          this.buildStatList(stats);
          this.loading = false;
        },
        error: (err) => {
          if (err.status === 401) {
            this.authService.logout();
            this.router.navigate(['/login']);
          } else {
            this.error = this.translationService.get('character.loadFailed');
            this.loading = false;
          }
        }
      });
  }

  private buildStatList(stats: CharacterStats): void {
    this.statList = this.STAT_DEFS.map(def => {
      const points = stats[def.key] as number;
      const level  = Math.floor(points / this.POINTS_PER_LEVEL) + 1;
      const progress = points % this.POINTS_PER_LEVEL;
      return {
        ...def,
        points,
        level,
        progress,
        nextLevel: this.POINTS_PER_LEVEL
      };
    });

    // Overall level = floor average of all stat levels (7 stats now)
    const sumLevels = this.statList.reduce((sum, s) => sum + s.level, 0);
    this.totalLevel = Math.floor(sumLevels / this.statList.length);
  }
}
