﻿using CoreEscuela.Entidades;
using static System.Console;
using System.Collections.Generic;
using System;
using CoreEscuela.Util;
using System.Linq;

var engine = new EscuelaEngine();
engine.Inicializar();
Printer.WriteTitle("BIENVENIDOS A LA ESCUELA");
//Printer.Beep(10000, cantidad:10);
imprimirCursosEscuela(engine.Escuela);

var listaObjetos = engine.GetObjetoEscuela(
    out int conteoEvaluaciones,
    out int conteoCursos,
    out int conteoAsignaturas,
    out int conteoAlumnos
);






static void imprimirCursosEscuela(Escuela escuela)
{
    Printer.WriteTitle("Cursos de la Escuela");

    if (escuela?.Cursos != null)
    {
        foreach (var curso in escuela.Cursos)
        {
            Console.WriteLine($"Nombre {curso.nombre}, id {curso.uniqueId} Alumno {curso.Alumnos}");
        }
    }
}












