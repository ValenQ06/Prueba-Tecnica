import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone:true,
  imports:[FormsModule],
  templateUrl:'./login.html',
  styleUrl:'./login.css'
})
export class LoginComponent{

  email:string='';
  password:string='';
  terms:boolean=false;

  constructor(private authService:AuthService,
              private router:Router){}

  login(){

  const body={
    correoElectronico:this.email,
    contrasena:this.password
  };

  this.authService.login(body).subscribe({
    next:(res:any)=>{

      localStorage.setItem("token",res.token);

      this.router.navigate(['/home']);

    },
    error:()=>{
      alert("Credenciales inválidas");
    }
  });

  }

}