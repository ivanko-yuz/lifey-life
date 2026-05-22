import { Component, HostListener, OnInit } from '@angular/core';
import { LanguageService, LocalizationType } from '../../services/language.service';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.css']
})
export class LanguageSelectorComponent implements OnInit {
  currentLanguage: LocalizationType = LocalizationType.ua;
  languages = this.languageService.getAvailableLanguages();
  isOpen = false;

  constructor(private languageService: LanguageService) {}

  ngOnInit(): void {
    this.languageService.currentLanguage$.subscribe(language => {
      this.currentLanguage = language;
    });
  }

  get currentLabel(): string {
    return this.languageService.getLanguageDisplayName(this.currentLanguage);
  }

  toggle(): void {
    this.isOpen = !this.isOpen;
  }

  selectLanguage(language: LocalizationType): void {
    this.languageService.setLanguage(language);
    this.isOpen = false;

    const token = localStorage.getItem('token');
    if (token) {
      this.languageService.updateUserLanguage(language).subscribe({
        next: () => console.log('Language preference updated'),
        error: (error) => {
          console.warn('Failed to update language preference:', error);
        }
      });
    }
  }

  /** Close the dropdown when the user clicks anywhere outside this component. */
  @HostListener('document:click')
  onDocumentClick(): void {
    this.isOpen = false;
  }
}
