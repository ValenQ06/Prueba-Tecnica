import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environments';

@Injectable({
  providedIn: 'root'
})
export class ComercianteService {

  constructor(private http: HttpClient) {}

  // Obtener lista paginada de comerciantes
  getPaged(page:number,pageSize:number){

    return this.http.get(
      `${environment.apiUrl}/comerciantes?page=${page}&pageSize=${pageSize}`
    );

  }

  // Obtener comerciante por Id
  getById(id:number){

    return this.http.get(
      `${environment.apiUrl}/comerciantes/${id}`
    );

  }

  // Crear comerciante
  create(data:any){

    return this.http.post(
      `${environment.apiUrl}/comerciantes`,
      data
    );

  }

  // Actualizar comerciante
  update(id:number,data:any){

    return this.http.put(
      `${environment.apiUrl}/comerciantes/${id}`,
      data
    );

  }

  // Eliminar comerciante
  eliminar(id:number){

    return this.http.delete(
      `${environment.apiUrl}/comerciantes/${id}`
    );

  }

  // Cambiar estado (PATCH)
  cambiarEstado(id:number){

    return this.http.patch(
      `${environment.apiUrl}/comerciantes/${id}/estado`,
      {}
    );

  }

  // Obtener municipios
  getMunicipios(){

    return this.http.get(
      `${environment.apiUrl}/municipios`
    );

  }

  // Descargar reporte CSV
  descargarReporte(){

    return this.http.get(
      `${environment.apiUrl}/reportes/comerciantes`,
      { responseType:'blob' }
    );

  }

}