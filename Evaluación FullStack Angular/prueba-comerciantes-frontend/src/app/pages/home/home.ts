import { ChangeDetectorRef } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header';
import { ComercianteService } from '../../services/comerciante';
import { jwtDecode } from 'jwt-decode';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

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

  get totalPages(): number {
    return Math.ceil(this.totalRecords / this.pageSize);
  }

  constructor(
    private comercianteService: ComercianteService,
    private router: Router,
    private cd: ChangeDetectorRef,
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
    }, 200);
  }

  cargarComerciantes() {
    this.comercianteService.getPaged(this.page, this.pageSize).subscribe((res: any) => {
      console.log('Respuesta API:', res);

      if (res.success) {
        this.comerciantes = res.data.data || [];

        this.totalRecords = res.data.totalRecords || 0;

        this.comerciantes = [...this.comerciantes];

        this.cd.detectChanges();
      }
    });
  }

  cambiarEstado(id: number) {
    this.comercianteService.cambiarEstado(id).subscribe(() => this.cargarComerciantes());
  }

  eliminar(id: number) {
    Swal.fire({
      title: '¿Estás segura?',
      text: 'Este comerciante será eliminado',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar',
      confirmButtonColor: '#dc3545',
      cancelButtonColor: '#6c757d',
    }).then((result) => {
      if (result.isConfirmed) {
        this.comercianteService.eliminar(id).subscribe({
          next: () => {
            Swal.fire({
              title: 'Eliminado',
              text: 'El comerciante fue eliminado correctamente',
              icon: 'success',
              confirmButtonColor: '#28a745',
            });

            this.cargarComerciantes();
          },
          error: (err) => {
            console.error(err);

            Swal.fire({
              title: 'Error',
              text: 'No se pudo eliminar',
              icon: 'error',
              confirmButtonColor: '#dc3545',
            });
          },
        });
      }
    });
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

  cambiarPagina(p: number) {
    this.page = p;
    this.cargarComerciantes();
  }
}
