import { Component, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Month, Week, WeekDay, CalendarData, Day } from './date-picker.types';
import { CommonModule } from '@angular/common';

enum ControlMode {
  Calendar,
  Months,
  Years
}

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
  @Input() format: string = 'ymd';
  @Input() placeholder: string = 'Enter date';

  value: Date | null = null;
  disabled: boolean = false;
  onChange: any = () => { }
  onTouch: any = () => { }

  calendarVisible: boolean = false;
  ControlMode = ControlMode;
  mode: ControlMode = ControlMode.Calendar;

  calendarData: CalendarData = this.calculateCalendarData(this.value || new Date());

  monthDictionary: Month[][] = [
    [
      { index: 0, fullName: 'Styczeń', shortName: 'sty' },
      { index: 1, fullName: 'Luty', shortName: 'lut' },
      { index: 2, fullName: 'Marzec', shortName: 'mar' },
    ],
    [
      { index: 3, fullName: 'Kwiecień', shortName: 'kwi' },
      { index: 4, fullName: 'Maj', shortName: 'maj' },
      { index: 5, fullName: 'Czerwiec', shortName: 'cze' },
    ],
    [
      { index: 6, fullName: 'Lipiec', shortName: 'lip' },
      { index: 7, fullName: 'Sierpień', shortName: 'sie' },
      { index: 8, fullName: 'Wrzesień', shortName: 'wrz' },
    ],
    [
      { index: 9, fullName: 'Październik', shortName: 'paź' },
      { index: 10, fullName: 'Listopad', shortName: 'lis' },
      { index: 11, fullName: 'Grudzień', shortName: 'gru' }
    ]
  ]

  months: Month[] = [
    { index: 0, fullName: 'Styczeń', shortName: 'sty' },
    { index: 1, fullName: 'Luty', shortName: 'lut' },
    { index: 2, fullName: 'Marzec', shortName: 'mar' },
    { index: 3, fullName: 'Kwiecień', shortName: 'kwi' },
    { index: 4, fullName: 'Maj', shortName: 'maj' },
    { index: 5, fullName: 'Czerwiec', shortName: 'cze' },
    { index: 6, fullName: 'Lipiec', shortName: 'lip' },
    { index: 7, fullName: 'Sierpień', shortName: 'sie' },
    { index: 8, fullName: 'Wrzesień', shortName: 'wrz' },
    { index: 9, fullName: 'Październik', shortName: 'paź' },
    { index: 10, fullName: 'Listopad', shortName: 'lis' },
    { index: 11, fullName: 'Grudzień', shortName: 'gru' }
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
    if(this.value) {
      const day = this.value.getDate();
      const month = this.value.getMonth() + 1;
      const year = this.value.getFullYear();
      if(this.format === 'dmy') return `${day < 10 ? '0' : ''}${day}-${month < 10 ? '0' : ''}${month}-${year}`;
      if(this.format === 'ymd') return `${year}-${month < 10 ? '0' : ''}${month}-${day < 10 ? '0' : ''}${day}`;
    }
    return '';
  }

  ngOnInit(): void {
    window.onclick = (element: any) => {
      if (!element.target.matches('.date-picker-action')) {
        this.calendarVisible = false;
        this.setMode(ControlMode.Calendar);
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

  setMode(mode: ControlMode) {
    this.mode = mode;
  }

  calculateCalendarData(date: Date): CalendarData {
    const result: Week[] = [];
    const firstDayDate = new Date(date.getFullYear(), date.getMonth(), 1);
    const lastDayDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    var firstDayOfWeek = firstDayDate.getDay() == 0 ? 6 : firstDayDate.getDay();

    const numOfWeeks = Math.ceil((firstDayOfWeek + lastDayDate.getDate()) / 7);
    var day = 0;
    for (let w = 0; w < numOfWeeks; w++) {
      var days: Day[] = [];
      for(let d = 0; d < 7; d++) {
        if(w == 0 && d < firstDayOfWeek || day >= lastDayDate.getDate()) {
          continue;
        }
        days[d] = {
          day: ++day,
          month: date.getMonth(),
          year: date.getFullYear(),
        }
      }
      result.push({
        days: days
      });
    }
    return {
      year: date.getFullYear(),
      month: date.getMonth(),
      day: date.getDay(),
      monthTable: result
    };
  }

  onMonthClick(index: number) {
    this.calendarData = this.calculateCalendarData(new Date(this.calendarData.year, index, 1));
    this.mode = ControlMode.Calendar;
  }

  onDayClick(day: Day) {
    this.value = new Date(day.year, day.month, day.day);
    this.onChange(this.value);
    this.onTouch();
    this.calendarVisible = false;
  }
}
