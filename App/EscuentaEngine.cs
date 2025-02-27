using System.Linq;
using CoreEscuela.Entidades;
using CoreEscuela.Util;

namespace CoreEscuela.App
{
    public sealed class EscuelaEngine
    {
        public  Escuela Escuela { get; set; }

        
        public EscuelaEngine()
        {

        }

        public void Inicializar()
        {
            Escuela = new Escuela("Platzi Academy", 2012, TiposEscuela.Primaria, ciudad: "Bogota", pais: "Colombia");

            CargarCursos();
            CargarAsignaturas();
            CargarEvaluaciones();

        }

        public void ImprimirDiccionario (Dictionary<LlavesDiccionario, IEnumerable<ObjetoEscuelaBase>> dic, bool imprimirEval = false)
        {
            foreach (var obj in dic)
            {
                Printer.WriteTitle(obj.Key.ToString());
                foreach (var val in obj.Value)
                {
                    switch (obj.Key)
                    {
                        case LlavesDiccionario.Evaluacion:
                            if (imprimirEval)
                            {
                                Console.WriteLine(val);
                            }
                        break;
                        case LlavesDiccionario.Escuela:
                            Console.WriteLine("Escuela: " + val);
                        break;
                        case LlavesDiccionario.Alumno:
                            Console.WriteLine("Alumno: " + val.nombre);
                        break;
                        case LlavesDiccionario.Curso:
                            var cursotmp = val as Curso;
                            if (cursotmp!=null)
                            {
                                int count = cursotmp.Alumnos.Count;
                                Console.WriteLine("Curso: " + val.nombre + " Cantidad Alumnos: " + count);
                            }
                        break;
                        default:
                            Console.WriteLine(val);
                        break;
                    }                    
                }
            }
        }

        public Dictionary<LlavesDiccionario, IEnumerable<ObjetoEscuelaBase> >GetDiccionarioObjetos()
        {
            var diccionario = new Dictionary<LlavesDiccionario, IEnumerable<ObjetoEscuelaBase>>();
            diccionario.Add(LlavesDiccionario.Escuela, new[] {Escuela});
            diccionario.Add(LlavesDiccionario.Curso, Escuela.Cursos.Cast<ObjetoEscuelaBase>());
            var listatmp = new List<Evaluacion>();
            var listatmpas = new List<Asignatura>();
            var listatmpal = new List<Alumno>();
            foreach (var curso in Escuela.Cursos)
            {
                listatmpas.AddRange(curso.Asignaturas);
                listatmpal.AddRange(curso.Alumnos);
                foreach (var alumno in curso.Alumnos)
                {
                    listatmp.AddRange(alumno.Evaluaciones);
                }
            }
            diccionario.Add(LlavesDiccionario.Asignatura,  listatmpas.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlavesDiccionario.Alumno, listatmpal.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlavesDiccionario.Evaluacion, listatmp.Cast<ObjetoEscuelaBase>());
            return diccionario;
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
            bool traeEvaluaciones = true,
            bool traerAlunmos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true
            )
        {
            return GetObjetoEscuela(out int dummy, out dummy, out dummy, out dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
            out int conteoEvaluaciones,
            bool traeEvaluaciones = true,
            bool traerAlunmos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true
            )
        {
            return GetObjetoEscuela(out conteoEvaluaciones, out int dummy, out dummy, out dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,
            bool traeEvaluaciones = true,
            bool traerAlunmos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true
            )
        {
            return GetObjetoEscuela(out conteoEvaluaciones, out conteoCursos, out int dummy, out dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,
            out int conteoAsignaturas,
            bool traeEvaluaciones = true,
            bool traerAlunmos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true
            )
        {
            return GetObjetoEscuela(out conteoEvaluaciones, out conteoCursos, out conteoAsignaturas, out int dummy);
        }


        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,
            out int conteoAsignaturas,
            out int conteoAlumnos,
            bool traeEvaluaciones = true, 
            bool traerAlunmos = true, 
            bool traerAsignaturas = true, 
            bool traerCursos = true
            )
        {
            conteoEvaluaciones = conteoAsignaturas = conteoAlumnos = 0;
            var listaObj = new List<ObjetoEscuelaBase>();
            listaObj.Add(Escuela);
            if (traerCursos)
                listaObj.AddRange(Escuela.Cursos);
                conteoCursos = Escuela.Cursos.Count;
            foreach (var curso in Escuela.Cursos)
            {
                conteoAsignaturas += curso.Asignaturas.Count;
                conteoAlumnos += curso.Alumnos.Count;
                if (traerAsignaturas)
                    listaObj.AddRange(curso.Asignaturas);
                if (traerAlunmos)
                    listaObj.AddRange(curso.Alumnos);

                if (traeEvaluaciones)
                {
                    foreach (var alumno in curso.Alumnos)
                    {
                        listaObj.AddRange(alumno.Evaluaciones);
                        conteoEvaluaciones += alumno.Evaluaciones.Count;
                    }
                }
            }
            return listaObj.AsReadOnly();
        }


#region Metodos de carga 

        private void CargarEvaluaciones()
        {
            var rnd = new Random();
            foreach (var curso in Escuela.Cursos)
            {
                foreach (var asignatura in curso.Asignaturas)
                {
                    foreach (var alumno in curso.Alumnos)
                    {
                        

                        for (int i = 0; i < 5; i++)
                        {
                            var ev = new Evaluacion
                            {
                                Asignatura = asignatura,
                                nombre = $"{asignatura.nombre} Ev#{i+1}",
                                Nota = MathF.Round(5 * (float)rnd.NextDouble(),2),
                                Alumno = alumno
                            };
                            alumno.Evaluaciones.Add(ev);
                        }
                    }
                }
            }
        }



        private void CargarAsignaturas()
        {
            foreach (var curso in Escuela.Cursos)
            {
                var listaAsignaturas = new List<Asignatura>()
                {
                    new Asignatura{nombre="Matematicas"},
                    new Asignatura{nombre="Educacion Fisica"},
                    new Asignatura{nombre="Castellano"},
                    new Asignatura{nombre="Ciencias Naturales"}
                };
                curso.Asignaturas = listaAsignaturas;
            }
        }

        private List<Alumno> GenerarAlumnosAlAzar(int cantidad)
        {
            string[] nombre1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };
            string[] nombre2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };

            var listaAlumnos = from n1 in nombre1
                               from n2 in nombre2
                               from a1 in apellido1
                               select new Alumno{nombre=$"{n1} {n2} {a1}"};
            return listaAlumnos.OrderBy((Al) => Al.uniqueId).Take(cantidad).ToList();
        }

        private void CargarCursos()
        {
            Escuela.Cursos = new List<Curso>()
                            {
                                new Curso() { nombre = "101", jornada=TiposJornada.Mañana},
                                new Curso() { nombre = "201", jornada=TiposJornada.Mañana},
                                new Curso() { nombre = "301", jornada=TiposJornada.Mañana},
                                new Curso() { nombre = "401", jornada=TiposJornada.Tarde},
                                new Curso() { nombre = "501", jornada=TiposJornada.Tarde}
                            };

            Random rnd = new Random();
            
            foreach (var curso in Escuela.Cursos)
            {
                int cantidadRandom = rnd.Next(5,20);
                curso.Alumnos = GenerarAlumnosAlAzar(cantidadRandom);
            }
        }
    }
#endregion
}