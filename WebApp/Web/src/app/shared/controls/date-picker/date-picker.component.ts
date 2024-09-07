import { Component, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Month, Week, WeekDay, CalendarData, Day } from './date-picker.types';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: DatePickerComponent,
      multi: true
    }
  ]
})
export default class DatePickerComponent implements ControlValueAccessor, OnInit {
  @Input() format: string = 'yyyy-MM-dd';
  @Input() placeholder: string = 'Enter date';

  value: Date | null = null;
  disabled: boolean = false;
  onChange: any = () => { }
  onTouch: any = () => { }

  calendarVisible: boolean = false;
  months: Month[] = [
    { fullName: 'Styczeń', shortName: 'sty', daysCount: 31 },
    { fullName: 'Luty', shortName: 'lut', daysCount: 29 },
    { fullName: 'Marzec', shortName: 'mar', daysCount: 31 },
    { fullName: 'Kwiecień', shortName: 'kwi', daysCount: 30 },
    { fullName: 'Maj', shortName: 'maj', daysCount: 31 },
    { fullName: 'Czerwiec', shortName: 'cze', daysCount: 30 },
    { fullName: 'Lipiec', shortName: 'lip', daysCount: 31 },
    { fullName: 'Sierpień', shortName: 'sie', daysCount: 31 },
    { fullName: 'Wrzesień', shortName: 'wrz', daysCount: 30 },
    { fullName: 'Październik', shortName: 'paź', daysCount: 31 },
    { fullName: 'Listopad', shortName: 'lis', daysCount: 30 },
    { fullName: 'Grudzień', shortName: 'gru', daysCount: 31 }
  ];

  weekDays: WeekDay[] = [
    { fullName: 'Poniedziałek', shortName: 'pn' },
    { fullName: 'Wtorek', shortName: 'wt' },
    { fullName: 'Środa', shortName: 'śr' },
    { fullName: 'Czwartek', shortName: 'cz' },
    { fullName: 'Piątek', shortName: 'pt' },
    { fullName: 'Sobota', shortName: 'so' },
    { fullName: 'Niedziela', shortName: 'nd' },
  ];

  get formattedValue(): string {
    return this.value ? this.value.toISOString().split('T')[0] : '';
  }

  get calendarData(): CalendarData {
    const date = this.value ? this.value : new Date();
    return {
      year: date.getFullYear(),
      month: date.getMonth(),
      day: date.getDay(),
      weeks: this.getWeeks(date)
    }
  }

  ngOnInit(): void {
    window.onclick = (element: any) => {
      if (!element.target.matches('.calendar-button')) {
        this.calendarVisible = false;
      }
    }
  }

  writeValue(obj: any): void {
    this.value = obj;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouch = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  toggleCalendar() {
    this.calendarVisible = !this.calendarVisible;
  }

  getWeeks(date: Date): Week[] {
    const result: Week[] = [];
    const firstDayDate = new Date(date.getFullYear(), date.getMonth(), 1);
    const lastDayDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    var firstDayOfWeek = firstDayDate.getDay() == 0 ? 6 : firstDayDate.getDay();

    const numOfWeeks = Math.ceil((firstDayOfWeek + lastDayDate.getDate()) / 7);
    var day = 0;
    for (let w = 0; w < numOfWeeks; w++) {
      var days: Day[] = [];
      for(let d = 0; d < 7; d++) {
        if(w == 0 && d < firstDayOfWeek) {
          days[d] = {
            day: 0,
            month: 0,
            year: 0,
          };
          continue;
        }
        if(day >= lastDayDate.getDate()) {
          days[d] = {
            day: 0,
            month: 0,
            year: 0,
          };
          day++;
          continue;
        }
        days[d] = {
          day: ++day,
          month: date.getMonth() + 1,
          year: date.getFullYear(),
        }
      }
      result.push({
        days: days
      });
    }
    return result;
  }

  onDayClick(day: Day) {
    console.log(day);
  }
}
