import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  constructor() { }

  http = inject(HttpClient);

  getWeather(): Observable<any> {
    return this.http.get<any>('NewPortal/Weather/Load');
  }
}
