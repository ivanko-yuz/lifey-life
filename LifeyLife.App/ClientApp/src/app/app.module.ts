import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './shared/nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { RandomDareComponent } from './random-dare/random-dare.component';
import { RandomeDareHistoryComponent } from './random-dare-history/random-dare-history.component';
import { AuthenticationModule } from './authentication/authentication.module';
import { HistoryComponent } from './history/history.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    RandomDareComponent,
    RandomeDareHistoryComponent,
    HistoryComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatProgressBarModule,
    MatToolbarModule,
    MatIconModule,
    MatSidenavModule,
    MatListModule,
    AuthenticationModule,
    RouterModule.forRoot([
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'random-dare', component: RandomDareComponent },
      { path: 'random-dare-history', component: RandomeDareHistoryComponent },
      { path: 'history', component: HistoryComponent },
      { path: '**', redirectTo: '/home', pathMatch: 'full' }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})

export class AppModule { }