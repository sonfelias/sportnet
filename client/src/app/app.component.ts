import { Component, OnInit } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { IPagination } from './shared/models/pagination';
// import { IProduct } from './shared/models/product';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  
  title = 'SportNet';
  //products : IProduct[];

  //constructor(private http: HttpClient){}
  constructor(){}

  ngOnInit(): void {
    // this.http.get('https://localhost:7129/api/products?pageSize=50').subscribe((response:IPagination) =>{
    //   this.products = response.data;
    // }, error => {
    //   console.log(error);
    // });
  }
}
