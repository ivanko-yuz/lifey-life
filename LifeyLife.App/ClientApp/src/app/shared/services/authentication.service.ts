import { UserForRegistration } from './../../_interfaces/user/userForRegistration.model';
import { RegistrationResponse } from './../../_interfaces/response/registrationResponse.model';
import { HttpClient } from '@angular/common/http';
import {Inject, Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  @Inject('BASE_URL') private baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }
  public registerUser = (route: string, body: UserForRegistration) => {
    return this.http.post<RegistrationResponse> (this.baseUrl + 'accounts/registration', body);
  }
  // private createCompleteRoute = (route: string, envAddress: string) => {
  //   console.log(`${envAddress}${route}`);
  //   return `${envAddress}${route}`;
  // }
}
