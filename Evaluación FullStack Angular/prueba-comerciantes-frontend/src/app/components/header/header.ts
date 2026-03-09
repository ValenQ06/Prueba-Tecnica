import { Component } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-header',
  standalone: true,
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class HeaderComponent {

  userName: string = '';
  role: string = '';

  constructor() {

    const token = localStorage.getItem('token');

    if (token) {

      const decoded: any = jwtDecode(token);

      this.userName = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
      this.role = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

    }

  }

  logout(){
      localStorage.removeItem("token");
      window.location.href = "/";
    }

}