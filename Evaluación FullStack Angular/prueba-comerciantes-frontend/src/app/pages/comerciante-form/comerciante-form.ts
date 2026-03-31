import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ComercianteService } from '../../services/comerciante';
import { ActivatedRoute, Router } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-comerciante-form',
  standalone: true,
  imports: [HeaderComponent, FormsModule, CommonModule],
  templateUrl: './comerciante-form.html',
  styleUrl: './comerciante-form.css',
})
export class ComercianteFormComponent implements OnInit {
  comerciante: any = {
    nombreRazonSocial: '',
    municipioId: 0,
    telefono: '',
    correoElectronico: '',
    fechaRegistro: '',
    estado: 'Activo',
  };

  municipios: any[] = [];
  id: number = 0;
  esEdicion = false;

  constructor(
    private comercianteService: ComercianteService,
    private route: ActivatedRoute,
    private router: Router,
    private cd: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      this.id = parseInt(idParam);
      this.esEdicion = true;
    }

    this.cargarMunicipios(); // 🔥 esto dispara todo
  }

  cargarMunicipios() {
    this.comercianteService.getMunicipios().subscribe((res: any) => {
      this.municipios = res.data;

      // 🔥 SOLO después de cargar municipios
      if (this.esEdicion) {
        this.cargarComerciante(this.id);
      }
    });
  }

  cargarComerciante(id: number) {
    console.log('ID que llega:', id);

    this.comercianteService.getById(id).subscribe((res: any) => {
      console.log('Respuesta API:', res);

      if (res.success) {
        const c = res.data;

        // 🔥 IMPORTANTE: NO reemplazar el objeto
        this.comerciante.nombreRazonSocial = c.nombreRazonSocial;
        this.comerciante.municipioId = c.idMunicipio;
        this.comerciante.telefono = c.telefono;
        this.comerciante.correoElectronico = c.correoElectronico;
        this.comerciante.fechaRegistro = c.fechaRegistro ? c.fechaRegistro.split('T')[0] : '';
        this.comerciante.estado = c.estado;
        this.cd.detectChanges();
      }
    });
  }

  guardar() {
    const data = {
      id: this.id,
      nombreRazonSocial: this.comerciante.nombreRazonSocial,
      idMunicipio: Number(this.comerciante.municipioId),
      municipio: this.obtenerNombreMunicipio(this.comerciante.municipioId),
      telefono: this.comerciante.telefono,
      correoElectronico: this.comerciante.correoElectronico,
      fechaRegistro: this.comerciante.fechaRegistro
        ? this.comerciante.fechaRegistro + 'T00:00:00'
        : null,
      estado: this.comerciante.estado,
    };

    console.log('DATA FINAL:', JSON.stringify(data, null, 2));

    if (this.esEdicion) {
      this.comercianteService.update(this.id, data).subscribe({
        next: () => {
          Swal.fire({
            title: '¡Éxito!',
            text: 'Comerciante actualizado correctamente',
            icon: 'success',
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#28a745',
          }).then(() => {
            this.router.navigate(['/home']);
          });
        },
        error: (err) => {
          console.log('MENSAJE BACKEND:', err.error);
          Swal.fire({
            title: 'Error',
            text: err.error?.message || 'Ocurrió un error',
            icon: 'error',
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#dc3545',
          });
        },
      });
    } else {
      this.comercianteService.create(data).subscribe({
        next: () => {
          Swal.fire({
            title: '¡Éxito!',
            text: 'Creado correctamente',
            icon: 'success',
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#28a745',
          }).then(() => {
            this.router.navigate(['/home']);
          });
        },
        error: (err) => {
          console.error(err);
          Swal.fire({
            title: 'Error',
            text: err.error?.message || 'Ocurrió un error',
            icon: 'error',
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#dc3545',
          });
        },
      });
    }
  }

  obtenerNombreMunicipio(id: number): string {
    const muni = this.municipios.find((m) => m.id === id);
    return muni ? muni.nombre : '';
  }

  cancelar() {
    this.router.navigate(['/home']);
  }
}
