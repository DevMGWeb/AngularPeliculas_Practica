import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-mostrar-errores',
  templateUrl: './mostrar-errores.component.html',
  styleUrls: ['./mostrar-errores.component.css']
})
export class MostrarErroresComponent implements OnInit {

  constructor() { }

  @Input()
  errores:string[] = [];

  ngOnInit(): void {
  }

}
