import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PeliculaDTO } from '../pelicula';
import { PeliculasService } from '../peliculas.service';

@Component({
  selector: 'app-listado-peliculas',
  templateUrl: './listado-peliculas.component.html',
  styleUrls: ['./listado-peliculas.component.css']
})
export class ListadoPeliculasComponent implements OnInit {

  @Input()
  peliculas:PeliculaDTO[]= [];
  
  @Output()
  borrado: EventEmitter<void> = new EventEmitter<void>();

  constructor(private peliculaService: PeliculasService) { }

  ngOnInit(): void {}

  borrar(peliculaId:number): void{
    this.peliculaService.borrar(peliculaId).subscribe(
      () => this.borrado.emit()
    );
  }
}
