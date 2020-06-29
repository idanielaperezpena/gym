using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym
{
    class Cursos
    {
        private string Nombre;
        private string HorarioIni;
        private string HorarioFin;
        private string Dias_semana;
        private string Estatus;
        private string Instructor;
        private string Descripcion;
        private string Precio;
        private string Cupo;
        private string Inscritos;
        private string Lugar;
        private string Clasificacion;

        public Cursos(string Nombre, string HorarioIni, string HorarioFin, string Dias_semana, string Estatus, string Instructor,
            string Descripcion, string Precio, string Cupo, string Inscritos, string Lugar, string Clasificacion)
        {
            this.Nombre = Nombre;
            this.HorarioIni = HorarioIni;
            this.HorarioFin= HorarioFin;
            this.Dias_semana= Dias_semana;
            this.Estatus= Estatus;
            this.Instructor = Instructor;
            this.Descripcion = Descripcion;
            this.Precio= Precio;
            this.Cupo = Cupo;
            this.Inscritos = Inscritos;
            this.Lugar = Lugar;
            this.Clasificacion = Clasificacion;
            
        }

        public Cursos()
        {
        }

        public void Modificar(String ID)
        {
            String query = "UPDATE Cursos SET Nombre='" + Nombre + "', HorarioIni='" + HorarioIni + "', HorarioFin='" + HorarioFin + "', Dias_semana='" + Dias_semana + "', Estatus='" + Estatus + "', Instructor='" + Instructor + "', Descripcion='" + Descripcion + "', Precio=" + Precio + ", Cupo=" + Cupo + ", Inscritos=" + Inscritos + ",Lugar='" + Lugar + "', Clasificacion='"+Clasificacion+"' WHERE ID_Cursos="+ID+";";
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            en.Registrar(query);

        }
        public void Registrar()
        {
            String query = "INSERT INTO Cursos (Nombre, HorarioIni, HorarioFin, Dias_semana, Estatus, Instructor, Descripcion, Precio, Cupo, Inscritos,Lugar, Clasificacion) values('" + Nombre + "','" + HorarioIni + "','" + HorarioFin + "','" + Dias_semana + "','" + Estatus + "','" + Instructor + "','" + Descripcion + "'," + Precio + "," + Cupo + ", " + Inscritos + ", '" + Lugar + "','"+Clasificacion+"');";
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            en.Registrar(query);

        }
        public void ActualizarInscritos(String ID, int accion)
        {
            String query="";
            if (accion==0)
            {
                query = "UPDATE Cursos SET inscritos = inscritos-1 WHERE ID_Cursos =" + ID + ";";
            }
            if (accion == 1)
            {
                query = "UPDATE Cursos SET inscritos=inscritos+1  WHERE ID_Cursos =" + ID + ";";
            }
           
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            en.Registrar(query);
        }

        
    }
}
