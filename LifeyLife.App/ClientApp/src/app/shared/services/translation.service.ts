import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LanguageService, LocalizationType } from './language.service';

interface Translations {
  [key: string]: {
    ua: string;
    en: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private currentTranslations = new BehaviorSubject<any>({});
  public translations$ = this.currentTranslations.asObservable();

  private translations: Translations = {
    // Navigation
    'nav.home': { ua: 'Головна', en: 'Home' },
    'nav.randomDare': { ua: 'Випадковий виклик', en: 'Random Dare' },
    'nav.history': { ua: 'Історія', en: 'History' },
    'nav.login': { ua: 'Увійти', en: 'Login' },
    'nav.register': { ua: 'Реєстрація', en: 'Register' },
    'nav.logout': { ua: 'Вийти', en: 'Logout' },

    // Random Dare Page
    'randomDare.title': { ua: 'Випадковий виклик', en: 'Random Dare' },
    'randomDare.loading': { ua: 'Завантаження...', en: 'Loading...' },
    'randomDare.experiencePoints': { ua: 'Очки досвіду:', en: 'Experience Points:' },
    'randomDare.timeGiven': { ua: 'Час:', en: 'Time Given:' },
    'randomDare.minutes': { ua: 'хвилин', en: 'minutes' },
    'randomDare.complete': { ua: 'Виконати виклик', en: 'Complete Dare' },
    'randomDare.getAnother': { ua: 'Отримати інший виклик', en: 'Get Another Dare' },
    'randomDare.noAvailable': { ua: 'Наразі немає доступних викликів.', en: 'No dare available at the moment.' },
    'randomDare.getNew': { ua: 'Отримати новий виклик', en: 'Get New Dare' },
    'randomDare.failedToGet': { ua: 'Не вдалося отримати випадковий виклик', en: 'Failed to get random dare' },
    'randomDare.failedToComplete': { ua: 'Не вдалося виконати виклик', en: 'Failed to complete dare' },

    // Authentication
    'auth.login.title': { ua: 'Увійти', en: 'Login' },
    'auth.register.title': { ua: 'Реєстрація', en: 'Register' },
    'auth.email': { ua: 'Електронна пошта', en: 'Email' },
    'auth.password': { ua: 'Пароль', en: 'Password' },
    'auth.confirmPassword': { ua: 'Підтвердити пароль', en: 'Confirm Password' },
    'auth.preferredLanguage': { ua: 'Бажана мова', en: 'Preferred Language' },
    'auth.emailRequired': { ua: 'Електронна пошта обов\'язкова', en: 'Email is required' },
    'auth.emailInvalid': { ua: 'Введіть дійсну електронну пошту', en: 'Please enter a valid email' },
    'auth.passwordRequired': { ua: 'Пароль обов\'язковий', en: 'Password is required' },
    'auth.passwordMinLength': { ua: 'Пароль повинен містити щонайменше 6 символів', en: 'Password must be at least 6 characters' },
    'auth.passwordsMismatch': { ua: 'Паролі не співпадають', en: 'Passwords do not match' },
    'auth.loginButton': { ua: 'Увійти', en: 'Login' },
    'auth.registerButton': { ua: 'Зареєструватися', en: 'Register' },
    'auth.registrationFailed': { ua: 'Реєстрація не вдалася', en: 'Registration failed' },
    'auth.loginFailed': { ua: 'Помилка входу', en: 'Login failed' },

    // History Page
    'history.title': { ua: 'Історія викликів', en: 'Dare History' },
    'history.loading': { ua: 'Завантаження історії...', en: 'Loading history...' },
    'history.noHistory': { ua: 'Історія пуста', en: 'No history available' },
    'history.failed': { ua: 'Не вдалося завантажити історію', en: 'Failed to load history' },
    'history.completed': { ua: 'Виконано', en: 'Completed' },
    'history.skipped': { ua: 'Пропущено', en: 'Skipped' },

    // Home Page
    'home.welcome': { ua: 'Ласкаво просимо до LifeyLife', en: 'Welcome to LifeyLife' },
    'home.description': { ua: 'Зробіть своє життя цікавішим з випадковими викликами!', en: 'Make your life more interesting with random challenges!' },
    'home.getStarted': { ua: 'Почати', en: 'Get Started' },

    // Common
    'common.submit': { ua: 'Підтвердити', en: 'Submit' },
    'common.cancel': { ua: 'Скасувати', en: 'Cancel' },
    'common.save': { ua: 'Зберегти', en: 'Save' },
    'common.delete': { ua: 'Видалити', en: 'Delete' },
    'common.edit': { ua: 'Редагувати', en: 'Edit' },
    'common.back': { ua: 'Назад', en: 'Back' },
    'common.next': { ua: 'Далі', en: 'Next' },
    'common.previous': { ua: 'Попередній', en: 'Previous' },
    'common.close': { ua: 'Закрити', en: 'Close' },
  };

  constructor(private languageService: LanguageService) {
    // Subscribe to language changes and update translations
    this.languageService.currentLanguage$.subscribe(language => {
      this.updateTranslations(language);
    });
  }

  private updateTranslations(language: LocalizationType): void {
    const langKey = LocalizationType[language] as 'ua' | 'en';
    const translatedTexts: { [key: string]: string } = {};

    for (const [key, translations] of Object.entries(this.translations)) {
      translatedTexts[key] = translations[langKey];
    }

    this.currentTranslations.next(translatedTexts);
  }

  // Get a specific translation
  get(key: string): string {
    const currentLang = LocalizationType[this.languageService.getCurrentLanguage()] as 'ua' | 'en';
    return this.translations[key]?.[currentLang] || key;
  }

  // Get translation as observable
  get$(key: string): Observable<string> {
    return new Observable(observer => {
      this.translations$.subscribe(translations => {
        observer.next(translations[key] || key);
      });
    });
  }
}