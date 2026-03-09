import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ComercianteService } from '../../services/comerciante';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-comerciante-form',
  standalone: true,
  imports: [HeaderComponent, FormsModule, CommonModule],
  templateUrl: './comerciante-form.html',
  styleUrl: './comerciante-form.css'
})
export class ComercianteFormComponent implements OnInit{

  comerciante:any={
    nombreRazonSocial:'',
    municipioId:0,
    telefono:'',
    correoElectronico:'',
    fechaRegistro:'',
    estado:'Activo'
  };

  municipios:any[]=[];
  id:number=0;
  esEdicion=false;

  constructor(
    private comercianteService:ComercianteService,
    private route:ActivatedRoute,
    private router:Router
  ){}

  ngOnInit(){

    this.cargarMunicipios();

    const idParam=this.route.snapshot.paramMap.get('id');

    if(idParam){
      this.id=parseInt(idParam);
      this.esEdicion=true;
      this.cargarComerciante();
    }

  }

  cargarMunicipios(){
    this.comercianteService.getMunicipios()
    .subscribe((res:any)=>{
      this.municipios=res.data;
    });
  }

  cargarComerciante(){

    this.comercianteService.getById(this.id)
    .subscribe((res:any)=>{
      this.comerciante=res.data;
    });

  }

  guardar(){

    if(this.esEdicion){

      this.comercianteService.update(this.id,this.comerciante)
      .subscribe(()=>{
        this.router.navigate(['/home']);
      });

    }else{

      this.comercianteService.create(this.comerciante)
      .subscribe(()=>{
        this.router.navigate(['/home']);
      });

    }

  }

}