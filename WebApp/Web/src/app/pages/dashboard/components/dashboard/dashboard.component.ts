import { Component, CUSTOM_ELEMENTS_SCHEMA, inject } from '@angular/core';
import { DashboardService } from '@dashboard/services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export default class DashboardComponent {

  dashboardService = inject(DashboardService);

  ngOnInit() {
    this.dashboardService.getWeather().subscribe({
      next: (weather) => {
        console.log('Weather:', weather);
      }
    });
  }

}
