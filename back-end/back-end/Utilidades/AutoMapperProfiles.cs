using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Utilidades
{
    public class AutoMapperProfiles : Profile   
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember(x => x.Foto, options => options.Ignore());

            CreateMap<CineCreacionDTO, Cine>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(dto => 
                geometryFactory.CreatePoint(new Coordinate(dto.Latitud, dto.Longitud))));

            CreateMap<Cine, CineDTO>()
                .ForMember(x => x.Latitud, dto => dto.MapFrom(campo => campo.Ubicacion.X))
                .ForMember(x => x.Longitud, dto => dto.MapFrom(campo => campo.Ubicacion.Y));

            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(x => x.Poster, opciones => opciones.Ignore())
                .ForMember(x => x.PeliculasGeneros, opciones => opciones.MapFrom(MapearPeliculasGeneros))
                .ForMember(x => x.PeliculasCines, opciones => opciones.MapFrom(MapearPeliculasCines))
                .ForMember(x => x.PeliculasActores, opciones => opciones.MapFrom(MapearPeliculasActores));

            CreateMap<Pelicula, PeliculaDTO>()
                .ForMember(x => x.Generos, options => options.MapFrom(MapearPeliculasGeneros))
                .ForMember(x => x.Actores, options => options.MapFrom(MapearPeliculasActores))
                .ForMember(x => x.Cines, options => options.MapFrom(MapearPeliculasCines));
        }

        private List<CineDTO> MapearPeliculasCines(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            var resultado = new List<CineDTO>();

            if(pelicula.PeliculasCines != null)
            {
                foreach (var peliculasCines in pelicula.PeliculasCines)
                {
                    resultado.Add(new CineDTO()
                    {
                        Id= peliculasCines.CineId,
                        Nombre = peliculasCines.Cine.Nombre,
                        Latitud = peliculasCines.Cine.Ubicacion.X,
                        Longitud = peliculasCines.Cine.Ubicacion.Y
                    });
                }
            }

            return resultado;
        }

        private List<ActorPeliculaDTO> MapearPeliculasActores(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            var resultado = new List<ActorPeliculaDTO>();

            if(pelicula.PeliculasActores != null)
            {
                foreach (var peliculasActores in pelicula.PeliculasActores)
                {
                    resultado.Add(new ActorPeliculaDTO()
                    {
                        Id = peliculasActores.ActorId,
                        Nombre = peliculasActores.Actor.Nombre,
                        Foto = peliculasActores.Actor.Foto,
                        Orden = peliculasActores.Orden,
                        Personaje = peliculasActores.Personaje
                    });
                }
            }

            return resultado;
        }

        private List<GeneroDTO> MapearPeliculasGeneros(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            var resultado = new List<GeneroDTO>();

            if(pelicula.PeliculasGeneros != null)
            {
                foreach(var peliculasGeneros in pelicula.PeliculasGeneros)
                {
                    resultado.Add(new GeneroDTO() { Id = peliculasGeneros.GeneroId, 
                        Nombre = peliculasGeneros.Genero.Nombre });
                }
            }

            return resultado;
        }

        private List<PeliculasGeneros> MapearPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO,
           Pelicula pelicula)
        {
            var resultado = new List<PeliculasGeneros>();

            if (peliculaCreacionDTO.GenerosIds == null) { return resultado; }

            foreach (var id in peliculaCreacionDTO.GenerosIds)
            {
                resultado.Add(new PeliculasGeneros() { GeneroId = id });
            }

            return resultado;
        }
        private List<PeliculasCines> MapearPeliculasCines(PeliculaCreacionDTO peliculaCreacionDTO,
         Pelicula pelicula)
        {
            var resultado = new List<PeliculasCines>();

            if (peliculaCreacionDTO.CinesIds == null) { return resultado; }

            foreach (var id in peliculaCreacionDTO.CinesIds)
            {
                resultado.Add(new PeliculasCines() { CineId = id });
            }

            return resultado;
        }

        private List<PeliculasActores> MapearPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO,
            Pelicula pelicula)
        {
            var resultado = new List<PeliculasActores>();

            if(peliculaCreacionDTO.Actores == null) { return resultado; }

            foreach (var actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new PeliculasActores() { ActorId = actor.Id, Personaje  = actor.Personaje });
            }

            return resultado;
        }
    }
}
