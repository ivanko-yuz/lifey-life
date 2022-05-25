import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-random-dare',
  templateUrl: './random-dare.component.html'
})
export class RandomDareComponent {
  public randomDares: RandomDare[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<RandomDare>(baseUrl + 'randomdare').subscribe(result => {
      this.randomDares.push(result);
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
