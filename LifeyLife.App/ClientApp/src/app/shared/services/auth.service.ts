import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient) {
    // Check initial auth status
    this.checkAuthStatus();
  }

  private checkAuthStatus(): void {
    const token = localStorage.getItem('token');
    this.isAuthenticatedSubject.next(!!token);
  }

  getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getAuthenticatedHttpOptions() {
    return {
      headers: this.getAuthHeaders()
    };
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  login(token: string): void {
    localStorage.setItem('token', token);
    this.isAuthenticatedSubject.next(true);
  }

  logout(): void {
    localStorage.removeItem('token');
    this.isAuthenticatedSubject.next(false);
  }

  // Make authenticated HTTP requests
  authenticatedGet<T>(url: string): Observable<T> {
    return this.http.get<T>(url, this.getAuthenticatedHttpOptions());
  }

  authenticatedPost<T>(url: string, body: any): Observable<T> {
    return this.http.post<T>(url, body, this.getAuthenticatedHttpOptions());
  }

  authenticatedPut<T>(url: string, body: any): Observable<T> {
    return this.http.put<T>(url, body, this.getAuthenticatedHttpOptions());
  }
}