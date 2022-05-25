import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public randomDareHistories: RandomDareHistory[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<RandomDareHistory[]>(baseUrl + 'randomdarehistory').subscribe(result => {
      this.randomDareHistories = result;
    }, error => console.error(error));
  }
}

interface RandomDareHistory {
  randomDareUuid: string;
  userUuid: string;
  completed: boolean;
  receivedAtUnixUtcTimestamp: number;
}
