import { Pipe, PipeTransform, OnDestroy } from '@angular/core';
import { TranslationService } from '../services/translation.service';
import { Subscription } from 'rxjs';

@Pipe({
  name: 'translate',
  pure: false // Important: pipe updates when language changes
})
export class TranslatePipe implements PipeTransform, OnDestroy {
  private subscription?: Subscription;
  private lastKey?: string;
  private lastValue?: string;

  constructor(private translationService: TranslationService) {}

  transform(key: string): string {
    if (this.lastKey !== key) {
      this.lastKey = key;
      this.updateValue();
    }
    return this.lastValue || key;
  }

  private updateValue(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    
    this.subscription = this.translationService.get$(this.lastKey!).subscribe(
      value => this.lastValue = value
    );
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}