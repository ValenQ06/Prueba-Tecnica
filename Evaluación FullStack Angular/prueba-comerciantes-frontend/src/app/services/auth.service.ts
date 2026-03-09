import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environments';

@Injectable({
  providedIn:'root'
})
export class AuthService{

  constructor(private http:HttpClient){}

  login(data:any){
    return this.http.post(`${environment.apiUrl}/Auth/login`,data);
  }

}