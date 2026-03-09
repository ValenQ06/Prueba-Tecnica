import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environments';

@Injectable({
  providedIn: 'root'
})
export class ComercianteService {

  constructor(private http: HttpClient) {}

  obtenerComerciantes(page:number, pageSize:number){
    return this.http.get(`${environment.apiUrl}/comerciantes?page=${page}&pageSize=${pageSize}`);
  }

  eliminar(id:number){
    return this.http.delete(`${environment.apiUrl}/comerciantes/${id}`);
  }

  cambiarEstado(id:number){
    return this.http.patch(`${environment.apiUrl}/comerciantes/${id}/estado`,{});
  }

  descargarReporte(){
    return this.http.get(`${environment.apiUrl}/reportes/comerciantes`,{
      responseType:'blob'
    });
  }

  getPaged(page:number,pageSize:number){

  return this.http.get(
    `${environment.apiUrl}/comerciantes?page=${page}&pageSize=${pageSize}`
  );

}

}