import { UserForRegistration } from './../../_interfaces/user/userForRegistration.model';
import { HttpClient } from '@angular/common/http';
import {Inject, Injectable} from '@angular/core';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api.constants';

export interface RegistrationResponse {
  success: boolean;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  constructor(private http: HttpClient) { }

  register(email: string, password: string): Observable<RegistrationResponse> {
    const body = { email, password };
    return this.http.post<RegistrationResponse>(API_ENDPOINTS.REGISTRATION, body);
  }

  public registerUser = (route: string, body: UserForRegistration) => {
    return this.http.post<RegistrationResponse> (API_ENDPOINTS.REGISTRATION, body);
  }
}
