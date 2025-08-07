import { Component, OnInit } from '@angular/core';
import { LanguageService, LocalizationType } from '../../services/language.service';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.css']
})
export class LanguageSelectorComponent implements OnInit {
  currentLanguage: LocalizationType = LocalizationType.en;
  languages = this.languageService.getAvailableLanguages();

  constructor(private languageService: LanguageService) {}

  ngOnInit(): void {
    this.languageService.currentLanguage$.subscribe(language => {
      this.currentLanguage = language;
    });
  }

  onSelectChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    const language = +select.value as LocalizationType;
    this.onLanguageChange(language);
  }

  onLanguageChange(language: LocalizationType): void {
    this.languageService.setLanguage(language);
    
    // Update user preference in backend if user is logged in
    const token = localStorage.getItem('token');
    if (token) {
      this.languageService.updateUserLanguage(language).subscribe({
        next: () => console.log('Language preference updated'),
        error: (error) => {
          console.warn('Failed to update language preference (may be unauthenticated):', error);
          // Continue working - language preference is still stored locally
        }
      });
    }
  }
}