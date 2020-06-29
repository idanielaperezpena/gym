using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym
{
    class Inscripciones
    {
        private string ID_Clientes;
        private string FechaIni;
        private string FechaFin;
        private string ID_cursos;
        private string TipoPago;

        public Inscripciones( string ID_Clientes, string FechaIni, string FechaFin, string ID_Cursos, string TipoPago)
        {
            this.ID_Clientes = ID_Clientes;
            this.FechaIni = FechaIni;
            this.FechaFin = FechaFin;
            this.ID_cursos = ID_Cursos;
            this.TipoPago = TipoPago;

            
        }

        public Inscripciones()
        {
        }

        public int Verificar()
        {
            string query = "select ID_CursosF from Inscritos where ID_ClientesF="+ID_Clientes+" and FechaFin>=Current_Date();";

            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            MySqlDataReader myreader =en.Consultar(query);
            while (myreader.Read())
            {
                if (myreader["ID_CursosF"].ToString().Equals(ID_cursos))
                {
                    return 0;
                }
            }
            return 1;

        }
        public void Eliminar(String ID)
        {
            string query= "delete  from inscritos where ID_Inscritos="+ID+";";
            EnlaceDatos en = new EnlaceDatos();
            en.Registrar(query);

        }
        public void Inscribir()
        {
            String query = "INSERT INTO Inscritos (Id_ClientesF, FechaIni, FechaFin, ID_CursosF, TipoPago, Estatus) values(" + ID_Clientes + "," + FechaIni + "," + FechaFin + "," + ID_cursos + ",+'"+TipoPago+"','Activo');";
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            en.Registrar(query);
        }
    }
}
