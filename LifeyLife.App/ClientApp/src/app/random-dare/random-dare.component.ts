import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-random-dare',
  templateUrl: './random-dare.component.html'
})
export class RandomDareComponent {
  public randomDare!: RandomDare;
  private http: HttpClient;
  @Inject('BASE_URL') private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.http = http;

    this.http.get<RandomDare>(this.baseUrl + 'randomdare').subscribe(result => {
      this.randomDare = result;
    }, error => console.error(error));
  }

  Complete() {
    this.http.post<RandomDare>(this.baseUrl + 'randomdare/complete', this.randomDare).subscribe(
      _ => {},
        error => console.error(error)
    );

    this.http.get<RandomDare>(this.baseUrl + 'randomdare').subscribe(result => {
      this.randomDare = result;
    }, error => console.error(error));
  }

  YetAnotherDare() {
    this.http.post<RandomDare>(this.baseUrl + 'randomdare/skip', this.randomDare).subscribe(
      _ => {},
      error => console.error(error)
    );

    this.http.get<RandomDare>(this.baseUrl + 'randomdare').subscribe(result => {
      this.randomDare = result;
    }, error => console.error(error));
  }
}

interface RandomDare {
  uuid: string;
  language: string;
  context: string;
  experienceGained: number;
  givenTime: number;
}
