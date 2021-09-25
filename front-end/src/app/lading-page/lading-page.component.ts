import { Component, OnInit } from '@angular/core';
import { PeliculaDTO } from '../peliculas/pelicula';
import { PeliculasService } from '../peliculas/peliculas.service';

@Component({
  selector: 'app-lading-page',
  templateUrl: './lading-page.component.html',
  styleUrls: ['./lading-page.component.css']
})
export class LadingPageComponent implements OnInit {

  constructor(private peliculasService : PeliculasService) { }

  ocultar:boolean = false;
  peliculasEnCines: PeliculaDTO[] = [];
  peliculasProximosEstrenos: PeliculaDTO[]= [];

  ngOnInit(): void {
     this.cargarDatos();
  }

  cargarDatos(){
    this.peliculasService.obtenerLandingPage().subscribe(landing => {
      this.peliculasEnCines = landing.enCines;
      this.peliculasProximosEstrenos = landing.proximosEstrenos
     });
  }

  borrado(){
    this.cargarDatos();
  }

}
