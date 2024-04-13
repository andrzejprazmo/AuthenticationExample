import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import NavigationComponent from './navigation/navigation.component';

@Component({
    selector: 'app-layout',
    standalone: true,
    templateUrl: './layout.component.html',
    styleUrl: './layout.component.css',
    imports: [RouterModule, NavigationComponent]
})
export default class LayoutComponent {

}
