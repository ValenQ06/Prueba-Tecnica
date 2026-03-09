import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header';
import { ComercianteService } from '../../services/comerciante';
import { jwtDecode } from 'jwt-decode';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector:'app-home',
  standalone:true,
  imports:[HeaderComponent, FormsModule, CommonModule],
  templateUrl:'./home.html',
  styleUrl:'./home.css'
})
export class HomeComponent implements OnInit{

  comerciantes:any[]=[];
  page=1;
  pageSize=5;
  totalRecords:number=0;

  role='';

  constructor(private comercianteService:ComercianteService){

    const token:any = localStorage.getItem('token');

    if(token){
      const decoded:any = jwtDecode(token);
      this.role = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    }

    this.cargarComerciantes();

  }

   ngOnInit(){
    this.cargarComerciantes();
  }

  cargarComerciantes(){

  this.comercianteService.getPaged(this.page,this.pageSize)
  .subscribe((res:any)=>{

  console.log(res);

    this.comerciantes = res.data.data;
    this.totalRecords = res.data.totalRecords;

    });

  }

  cambiarEstado(id:number){

    this.comercianteService.cambiarEstado(id)
    .subscribe(()=> this.cargarComerciantes());

  }

  eliminar(id:number){

    if(confirm("¿Eliminar comerciante?")){

      this.comercianteService.eliminar(id)
      .subscribe(()=> this.cargarComerciantes());

    }

  }

  descargarCSV(){

    this.comercianteService.descargarReporte()
    .subscribe((file:any)=>{

      const blob = new Blob([file],{type:'text/csv'});
      const url = window.URL.createObjectURL(blob);

      const a = document.createElement('a');
      a.href = url;
      a.download = "reporte_comerciantes.csv";
      a.click();

    });

  }

}