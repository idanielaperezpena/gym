using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Gym
{
    class EnlaceDatos
    {

        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        MySqlConnection conn = new MySqlConnection();
        public void Conectar()
        {

            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "1234";
            builder.Database = "centro";
            builder.SslMode = MySqlSslMode.None;

            conn = new MySqlConnection(builder.ToString());
            conn.Open();
        }

        public void Registrar(String comando)
        {

            Conectar();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = comando;

            cmd.ExecuteNonQuery();
            

        }
        public void Cerrar()
        {
            conn.Close();
        }
        public void RegistrarCliente(String comando, Image Foto)
        {


            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = comando;
            MemoryStream stream = new MemoryStream();

            Foto.Save(stream, ImageFormat.Jpeg);
            byte[] fotobyte = stream.ToArray();


            cmd.Parameters.AddWithValue("@Foto", fotobyte);

            cmd.ExecuteNonQuery();


            conn.Close();

        }

        public MySqlDataReader Consultar(String comando)
        {
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = comando;
            MySqlDataReader myreader = cmd.ExecuteReader();

            return myreader;




        }
        public void ActualizarEstatus(){
           Conectar();
            Registrar("Update Clientes set Estatus='Activo' where ID_Clientes in (select ID_ClientesF from Inscritos where  FechaFin>=CURRENT_DATE() group by ID_ClientesF);" +
                "Update Clientes set Estatus = 'Inactivo' where not ID_Clientes in (select ID_ClientesF from Inscritos where FechaFin >= CURRENT_DATE() group by ID_ClientesF); ");

            MySqlDataReader dataReader = Consultar("select ID_Inscritos,ID_CursosF from Inscritos where  FechaFin=CURRENT_DATE() and Estatus='Activo';");
            int i = 0;
            
            List<String> ID_CursosF = new List<String>();

            List<String> ID_Inscritos = new List<String>();

            while (dataReader.Read())
            {
                ID_CursosF.Add(dataReader["ID_CursosF"].ToString());
                ID_Inscritos.Add(dataReader["ID_Inscritos"].ToString());


            }
            dataReader.Close();
            foreach (string s in ID_Inscritos)
            {
                Registrar("Update Cursos set Inscritos=Inscritos-1 where ID_Cursos=" + ID_CursosF[i] + ";");
                Registrar("Update Inscritos set Estatus='Vencido' where ID_Inscritos=" + ID_Inscritos[i] + ";");
                Console.WriteLine(ID_Inscritos[0] + " " + ID_CursosF[0]);
                i++;

            }
            Cerrar();

        }
        public Image ConsultarImagen(String comando)
        {
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = comando;
            byte[] imgArr = (byte[])cmd.ExecuteScalar();
            imgArr = (byte[])cmd.ExecuteScalar();
            MemoryStream stream = new MemoryStream(imgArr);


            Image img = Image.FromStream(stream);
            return img;

        }
        public MySqlDataReader ConsultarM(String comando)
        {
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = comando;
            MySqlDataReader myreader = cmd.ExecuteReader();
            myreader.Read();
            return myreader;





        }
        public DataTable tabla(string sql)
        {
            //el datatable nos ayuda a guardar los datos de la tabla que hemos selecionado en la consulta
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter();
            DataSet ds = new DataSet();

            //en esta parte se ejecuta la consulta sql para obtener la tabla
            Conectar();
            da.SelectCommand = new MySqlCommand(sql, conn);
            //aqui llenamos el dataset con lo que contiene el dataadapter
            da.Fill(ds);
            //aqui guardamos en el datatable la tabla específica del dataset
            dt = ds.Tables[0];
            //finalmente retornamos el dt
            return dt;
        }


    }
}
