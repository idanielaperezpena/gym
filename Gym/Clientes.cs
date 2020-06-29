using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace Gym
{
    class Clientes
    {
        protected string ID;
        protected string Nombre;
        protected string ApPaterno;
        protected string ApMaterno;
        protected string Genero;
        protected string FNacimiento;
        protected string Edad;
        protected string correo;
        protected string estatus;
        protected string telefono;
      
        protected string calle;
        protected string CP;
        protected string NumInt;
        protected string NumExt;
        protected string delegacion;
        protected string colonia;
        protected string estado;
        protected Image Foto;

        public Clientes()
        {

        }

        public Clientes(string Nombre, string ApPat, string ApMat, string Genero, string FNacimiento, string Edad, string correo, string estatus, string telefono, string calle, string CP, string NumInt, string NumExt, string delegacion, string colonia, string estado, Image Foto)
        {

            this.Nombre = Nombre;
            this.ApPaterno = ApPat;
            this.ApMaterno = ApMat;
            this.Genero = Genero;
            this.FNacimiento = FNacimiento;
            this.Edad = Edad;
            this.correo = correo;
            this.estatus = estatus;
            this.telefono = telefono;
            this.calle = calle;
            this.CP = CP;
            this.NumInt = NumInt;
            this.NumExt = NumExt;
            this.delegacion = delegacion;
            this.colonia = colonia;
            this.estado = estado;
            this.Foto = Foto;

            


        }

        public void Registrar()
        {
            String query = "INSERT INTO Clientes (Nombre, Ap_Paterno, Ap_Materno, Genero, Fecha_Nacimiento, Edad, Responsable, Email, Estatus, No_Telefono, Calle, CP, Num_Int, Num_Ext, Delegacion, Colonia, Estado,Foto) values('" + Nombre + "','" + ApPaterno + "','" + ApMaterno + "','" + Genero + "'," + FNacimiento + "," + Edad + ",'','" + correo + "','" + estatus + "','" + telefono + "', '" + calle+ "', " + CP + ", '"+NumInt+ "','" + NumExt + "','" + delegacion + "','" + colonia + "','" + estado + "',@Foto);";
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();

            en.RegistrarCliente(query, Foto);
           

        }
        public void Modificar(string ID)
        {
            String query = "UPDATE Clientes SET Nombre='" + Nombre + "', Ap_Paterno='" + ApPaterno + "', Ap_Materno='" + ApMaterno + "', Genero='" + Genero + "', Fecha_Nacimiento=" + FNacimiento + ", Edad=" + Edad + ", Email='" + correo + "', Estatus='" + estatus + "', No_Telefono='" + telefono + "', Calle='" + calle + "', CP=" + CP + ", Num_Int='" + NumInt + "', Num_Ext='" + NumExt + "', Delegacion='" + delegacion + "', Colonia='" + colonia + "', Estado='" + estado + "' WHERE ID_clientes="+ID+";";
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            en.Registrar(query);

        }
        public  MySqlDataReader Consultar(String Consulta)
        {
            String query = "select * from Clientes where id_Clientes=" + Consulta + ";";
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            MySqlDataReader myreader= en.Consultar(query);
            return myreader;

        }
        public Image ConsultarImagen(String Consulta)
        {
            String query = "select Foto from Clientes where id_Clientes=" + Consulta + ";";
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            Image Foto=en.ConsultarImagen(query);
            return Foto;
            
        }

        
    }
     class Menor:Clientes
    {
        private String Responsable;
        private String TelRes;

        public Menor( string Nombre, string ApPat, string ApMat, string Genero, string FNacimiento, string Edad,string responsable, string correo, string estatus, string telefono, string calle, string CP, string NumInt, string NumExt, string delegacion, string colonia, string estado, Image Foto, string TelRes) : base( Nombre, ApPat, ApMat, Genero, FNacimiento, Edad, correo, estatus, telefono, calle, CP, NumInt,NumExt, delegacion, colonia, estado,Foto)
        {
            this.TelRes = TelRes;
            this.Nombre = Nombre;
            this.ApPaterno = ApPat;
            this.ApMaterno = ApMat;
            this.Genero = Genero;
            this.FNacimiento = FNacimiento;
            this.Edad = Edad;
            this.Responsable = responsable;
            this.correo = correo;
            this.estatus = estatus;
            this.telefono = telefono;
            this.calle = calle;
            this.CP = CP;
            this.NumInt = NumInt;
            this.NumExt = NumExt;
            this.delegacion = delegacion;
            this.colonia = colonia;
            this.estado = estado;
            this.Foto = Foto;

        }
        public new void Registrar()
        {
            String query = "INSERT INTO Clientes (Nombre, Ap_Paterno, Ap_Materno, Genero, Fecha_Nacimiento, Edad, Responsable, Email, Estatus, No_Telefono, Calle, CP, Num_Int, Num_Ext, Delegacion, Colonia, Estado, Foto,No_TelefonoC) values('" + Nombre + "','" + ApPaterno + "','" + ApMaterno + "','" + Genero + "'," + FNacimiento + "," + Edad + ",'"+Responsable+"','" + correo + "','" + estatus + "','" + telefono + "','" + calle + "', " + CP + ", '" + NumInt + "','" + NumExt + "','" + delegacion + "','" + colonia + "','" + estado + "',@Foto,'"+TelRes+"' );";
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            en.RegistrarCliente(query,Foto);
        }
        public new void Modificar(string ID)
        {
            String query = "UPDATE Clientes SET Nombre='" + Nombre + "', Ap_Paterno='" + ApPaterno + "', Ap_Materno='" + ApMaterno + "', Genero='" + Genero + "', Fecha_Nacimiento=" + FNacimiento + ", Edad=" + Edad + ", Responsable='"+Responsable+"', Email='" + correo + "', Estatus='" + estatus + "', No_Telefono='" + telefono + "', Calle='" + calle + "', CP=" + CP + ", Num_Int='" + NumInt + "', Num_Ext='" + NumExt + "', Delegacion='" + delegacion + "', Colonia='" + colonia + "', Estado='" + estado + "', No_TelefonoC='"+TelRes+"', Foto=@Foto WHERE ID_clientes=" + ID + ";";
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            en.RegistrarCliente(query,Foto);

        }

    }

}
