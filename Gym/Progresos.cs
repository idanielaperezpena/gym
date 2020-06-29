using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym
{
    class Progresos
    {
        private string ID_Clientes;
        private string Fecha;
        private string Masa;
        private string Talla;
        private string Cintura;
        private string Cadera;
        private string Pecho;
        private string Biceps;
        private string Munecas;
        private string Cuello;
        private string Pantorrillas;
        private string IMC;

        public Progresos(string ID_Clientes, string Fecha, string Masa, string Talla, string Cintura, string  Cadera, string Pecho, string Biceps, string Munecas, string Cuello, string Pantorillas, string IMC )
        {
             this.ID_Clientes=ID_Clientes;
            this.Fecha=Fecha;
            this.Masa=Masa;
            this.Talla=Talla;
            this.Cintura=Cintura;
            this.Cadera=Cadera;
            this.Pecho=Pecho;
            this.Biceps=Biceps;
            this.Munecas=Munecas;
            this.Cuello=Cuello;
            this.Pantorrillas=Pantorillas;
            this.IMC=IMC;

    }
        public Progresos()
        {

        }
        
        public void Registrar()
        {
           
            String query = "INSERT INTO Progreso (ID_ClientesF2, Fecha, Masa, Talla, Cintura, Cadera, Pecho, Biceps, Munecas, Cuello, Pantorrillas, IMC) values(" + ID_Clientes + "," + Fecha + "," + Masa + ","+Talla+"," + Cintura+ "," + Cadera+ "," + Pecho+ "," + Biceps + "," + Munecas + ", " +Cuello+ "," + Pantorrillas + ","+IMC+");";
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            en.Registrar(query);
            en.Cerrar();
        }
        public void Modificar(string ID)
        {

            String query = "UPDATE progreso SET Fecha ="+Fecha+", Masa ="+Masa+", Talla = "+Talla+", Cintura = "+ Cintura+", Cadera ="+Cadera+", " +
                "Pecho ="+Pecho+", Biceps ="+Biceps+", Munecas = "+Munecas+", Cuello = "+Cuello+", " +
                "Pantorrillas ="+Pantorrillas+", IMC ="+ IMC+"  WHERE ID_Progreso ="+ID+"; ";
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            en.Registrar(query);
            en.Cerrar();
        }
        public double Calcular(double m, double t)
        {
            double imc = (m) /System.Math.Pow(t, 2);
            return imc;
        }
    }
}
