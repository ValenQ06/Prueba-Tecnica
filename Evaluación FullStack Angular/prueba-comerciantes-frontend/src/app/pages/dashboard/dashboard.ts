import { Component } from '@angular/core';
import { HeaderComponent } from '../../components/header/header';

@Component({
  selector: 'app-dashboard',
  imports: [HeaderComponent],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class DashboardComponent  {}
