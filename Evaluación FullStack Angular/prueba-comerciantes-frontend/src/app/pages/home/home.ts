import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header';
import { ComercianteService } from '../../services/comerciante';
import { jwtDecode } from 'jwt-decode';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [HeaderComponent, FormsModule, CommonModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class HomeComponent implements OnInit {
  comerciantes: any[] = [];
  page = 1;
  pageSize = 5;
  totalRecords: number = 0;

  role = '';

  constructor(
    private comercianteService: ComercianteService,
    private router: Router,
  ) {
    const token: any = localStorage.getItem('token');

    if (token) {
      const decoded: any = jwtDecode(token);
      this.role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    }
  }

  ngOnInit() {
    setTimeout(() => {
      this.cargarComerciantes();
    }, 300);
  }

  cargarComerciantes() {
    this.comercianteService.getPaged(this.page, this.pageSize).subscribe((res: any) => {
      console.log('DATA TABLA:', res.data.data);

      if (res.success) {
        this.comerciantes = res.data?.data || res.data;

        this.totalRecords = res.data?.totalRecords || this.comerciantes.length;

        this.comerciantes = [...this.comerciantes];
      }
    });
  }

  cambiarEstado(id: number) {
    this.comercianteService.cambiarEstado(id).subscribe(() => this.cargarComerciantes());
  }

  eliminar(id: number) {
    if (confirm('¿Eliminar comerciante?')) {
      this.comercianteService.eliminar(id).subscribe(() => this.cargarComerciantes());
    }
  }

  editar(id: number) {
    this.router.navigate(['/comerciante', id]);
  }

  crear() {
    this.router.navigate(['/comerciante']);
  }

  descargarCSV() {
    this.comercianteService.descargarReporte().subscribe((file: any) => {
      const blob = new Blob([file], { type: 'text/csv' });
      const url = window.URL.createObjectURL(blob);

      const a = document.createElement('a');
      a.href = url;
      a.download = 'reporte_comerciantes.csv';
      a.click();
    });
  }
}
