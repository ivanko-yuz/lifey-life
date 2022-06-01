import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'random-dare-history',
  templateUrl: './random-dare-history.component.html'
})
export class RandomeDareHistoryComponent {
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
  context: string;
  completed: boolean;
  completedAt: Date;
}
