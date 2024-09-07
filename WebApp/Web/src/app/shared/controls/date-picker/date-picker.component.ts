import { Component, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [],
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
export default class DatePickerComponent implements ControlValueAccessor {
  @Input() format: string = 'yyyy-MM-dd';
  @Input() placeholder: string = 'Enter date';

  value: Date | null = null;
  disabled: boolean = false;
  onChange: any = () => { }
  onTouch: any = () => { }

  get formattedValue(): string {
    return this.value ? this.value.toISOString().split('T')[0] : '';
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

}
