import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

export enum LocalizationType {
  ua = 0,
  en = 1
}

export interface UserProfile {
  email: string;
  preferredLanguage: string;
}

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  private currentLanguageSubject = new BehaviorSubject<LocalizationType>(LocalizationType.ua);
  public currentLanguage$ = this.currentLanguageSubject.asObservable();

  constructor(private http: HttpClient) {
    // Load language from localStorage or default to Ukrainian
    const savedLanguage = localStorage.getItem('preferredLanguage');
    if (savedLanguage && savedLanguage in LocalizationType) {
      this.currentLanguageSubject.next(LocalizationType[savedLanguage as keyof typeof LocalizationType]);
    }
  }

  getCurrentLanguage(): LocalizationType {
    return this.currentLanguageSubject.value;
  }

  setLanguage(language: LocalizationType): void {
    this.currentLanguageSubject.next(language);
    localStorage.setItem('preferredLanguage', LocalizationType[language]);
  }

  updateUserLanguage(language: LocalizationType): Observable<any> {
    return this.http.put('/api/accounts/language', { language });
  }

  getUserProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>('/api/accounts/profile');
  }

  getLanguageDisplayName(language: LocalizationType): string {
    switch (language) {
      case LocalizationType.en:
        return 'English';
      case LocalizationType.ua:
        return 'Українська';
      default:
        return 'Українська';
    }
  }

  getAvailableLanguages(): { value: LocalizationType, label: string }[] {
    return [
      { value: LocalizationType.ua, label: 'Українська' },
      { value: LocalizationType.en, label: 'English' }
    ];
  }
}