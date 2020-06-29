using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using MaterialSkin;//Estilo
using MaterialSkin.Controls;//Estilo
using System.Collections;

namespace Gym
{

    public partial class frmMenu : MaterialForm //Material Form para el estilo
    {
        public Camara camara;
        
        public String IDCurso;
        public String IDCliente;
        private EnlaceDatos enlace = new EnlaceDatos();
        public bool consulta;
        public String IDInscripcion;
        public String IDProgreso;


        public frmMenu()

        {
            InitializeComponent();


            enlace.ActualizarEstatus();
            //impresiones de los documentos
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printDocumentInscripcion.PrintPage += new PrintPageEventHandler(printDocumentInscripcion_PrintPage);
            PaperSize p = new PaperSize("Carta", 830, 1100);
            printDocument1.DefaultPageSettings.PaperSize = p;
            printDocumentInscripcion.DefaultPageSettings.PaperSize = p;
            // Material Manager (Estilos) 17/05/2018
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;


            // Configuración de color (Estilos) 17/05/2018
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue400, Primary.Blue500,
                Primary.Blue500, Accent.LightBlue200,
                TextShade.WHITE
            );


        }
        private void printDocumentInscripcion_PrintPage(System.Object sender, PrintPageEventArgs e)
        {
            int filas = dataGridCursosCliente.RowCount - 1;
            if (filas < 0)
            {
                filas = 0;
            }
            Console.WriteLine(filas);
            DataGridViewRow row = dataGridCursosCliente.Rows[filas];
            Console.WriteLine(row.ToString());




            Image Comprobante = Image.FromFile("Comprobante.bmp");
            string Npago = row.Cells[0].Value.ToString();
            Console.WriteLine(Npago);

            string FInicio = row.Cells[1].Value.ToString();
            Console.WriteLine(FInicio);
            FInicio = FInicio.Substring(0, 10);
            Console.WriteLine(FInicio);
            string FVence = row.Cells[2].Value.ToString();
            FVence = FVence.Substring(0, 10);
            Console.WriteLine(FVence);
            string ID_Cliente = row.Cells[4].Value.ToString();
            string Miembro = row.Cells[5].Value.ToString() + " " + row.Cells[6].Value.ToString() + " " + row.Cells[7].Value.ToString();
            string[] dias = row.Cells[11].Value.ToString().Split(',');
            string dias_aux = "";
            foreach (string i in dias)
            {
                dias_aux += i + " ";
            }

            string Actividad = row.Cells[8].Value.ToString();
            string Horario = row.Cells[9].Value.ToString() + " a " + row.Cells[10].Value.ToString();
            string Zona = row.Cells[12].Value.ToString();
            string TipoPago = row.Cells[13].Value.ToString();
            string Monto = "$" + row.Cells[14].Value.ToString() + ".00 MXN";




            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            StringFormat drawFormat = new StringFormat();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImage(Comprobante, 30, 30);
            e.Graphics.DrawString(Npago, drawFont, drawBrush, 230, 200, drawFormat);
            e.Graphics.DrawString(ID_Cliente, drawFont, drawBrush, 200, 250, drawFormat);
            e.Graphics.DrawString(Miembro, drawFont, drawBrush, 220, 295, drawFormat);
            e.Graphics.DrawString(FInicio, drawFont, drawBrush, 180, 345, drawFormat);
            e.Graphics.DrawString(FVence, drawFont, drawBrush, 480, 345, drawFormat);
            e.Graphics.DrawString(Actividad, drawFont, drawBrush, 220, 395, drawFormat);
            e.Graphics.DrawString(Horario, drawFont, drawBrush, 483, 395, drawFormat);
            e.Graphics.DrawString(dias_aux, drawFont, drawBrush, 200, 445, drawFormat);
            e.Graphics.DrawString(Zona, drawFont, drawBrush, 200, 495, drawFormat);
            e.Graphics.DrawString(TipoPago, drawFont, drawBrush, 250, 560, drawFormat);
            e.Graphics.DrawString(Monto, drawFont, drawBrush, 200, 620, drawFormat);
            drawFont = new Font("Arial", 12);
            e.Graphics.DrawString("*En pagos con tarjetas de crédito o débido se entrega una copia de la transacción", drawFont, drawBrush, 100, 1000, drawFormat);






        }
        private void printDocument1_PrintPage(System.Object sender, PrintPageEventArgs e)
        {
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            if (!consulta)
            {

                MySqlDataReader myreader = en.Consultar("SELECT MAX(ID_Clientes) AS ID FROM Clientes;");
                myreader.Read();
                IDCliente = myreader["ID"].ToString();
                Clientes cliente = new Clientes();
                myreader = cliente.Consultar(IDCliente);
                myreader.Read();
                Image Credencial = Image.FromFile("Credencial.bmp");
                String[] nom = myreader[1].ToString().Split(' ');
                string nombre = "";
                foreach (char c in nom[0].ToCharArray())
                {
                    if (c > (char)128)
                    {

                    }
                    else
                    {
                        if (nombre.Length <= 20)
                        {
                            nombre = nombre + c;
                        }
                    }
                }
                nom[0] = myreader[2].ToString();
                foreach (char c in nom[0].ToCharArray())
                {
                    if (c > (char)128)
                    {

                    }
                    else
                    {
                        if (nombre.Length <= 20)
                        {
                            nombre = nombre + c;
                        }

                    }
                }
                string Datos = myreader[0].ToString() + nombre;
                BarcodeLib.Barcode codigo = new BarcodeLib.Barcode();
                codigo.IncludeLabel = true;
                Image ImgCodigo = codigo.Encode(BarcodeLib.TYPE.CODE128, Datos, Color.Black, Color.White, 300, 100);
                Rectangle compressionRectangle = new Rectangle(185, 175, PBFoto.Width - 140, PBFoto.Height - 50);
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImage(Credencial, 0, 0);
                e.Graphics.DrawImage(PBFoto.Image, compressionRectangle);
                e.Graphics.DrawImage(ImgCodigo, 110, 507);
                string drawString = myreader[1].ToString() + " " + myreader[2].ToString();

                Font drawFont = new Font("Arial Black", 14);
                SolidBrush drawBrush = new SolidBrush(Color.WhiteSmoke);

                StringFormat drawFormat = new StringFormat();
                e.Graphics.DrawString(drawString, drawFont, drawBrush, drawString.Length + 100, 350, drawFormat);
                drawString = "No. de Cliente:   " + myreader[0].ToString();
                e.Graphics.DrawString(drawString, drawFont, drawBrush, 155, 370, drawFormat);
            }
            if (consulta)
            {
                Clientes cliente = new Clientes();

                MySqlDataReader myreader = cliente.Consultar(txtConsulta.Text);
                myreader.Read();
                Image Credencial = Image.FromFile("Credencial.bmp");
                String[] nom = myreader[1].ToString().Split(' ');
                string nombre = "";
                foreach (char c in nom[0].ToCharArray())
                {
                    if (c > (char)128)
                    {

                    }
                    else
                    {
                        if (nombre.Length < 20)
                        {
                            nombre = nombre + c;
                        }
                    }
                }
                nom[0] = myreader[2].ToString();
                foreach (char c in nom[0].ToCharArray())
                {
                    if (c > (char)128)
                    {

                    }
                    else
                    {
                        if (nombre.Length < 20)
                        {
                            nombre = nombre + c;
                        }

                    }
                }
                string Datos = myreader[0].ToString() + nombre;

                BarcodeLib.Barcode codigo = new BarcodeLib.Barcode();
                codigo.IncludeLabel = true;
                Image ImgCodigo = codigo.Encode(BarcodeLib.TYPE.CODE128, Datos, Color.Black, Color.White, 300, 100);
                Rectangle compressionRectangle = new Rectangle(185, 175, PBFoto.Width - 140, PBFoto.Height - 55);
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImage(Credencial, 0, 0);
                e.Graphics.DrawImage(PBFoto.Image, compressionRectangle);
                e.Graphics.DrawImage(ImgCodigo, 110, 507);
                string drawString = myreader[1].ToString() + " " + myreader[2].ToString() + " " + myreader[3].ToString();
                Font drawFont = new Font("Arial Black", 14);
                SolidBrush drawBrush = new SolidBrush(Color.WhiteSmoke);

                StringFormat drawFormat = new StringFormat();
                e.Graphics.DrawString(drawString, drawFont, drawBrush, drawString.Length + 100, 350, drawFormat);
                drawString = "No. de Cliente:   " + myreader[0].ToString();
                e.Graphics.DrawString(drawString, drawFont, drawBrush, 155, 370, drawFormat);

            }






        }


        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void gBAcceso_Enter(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
           
            if (txtConsulta.Text != "")
            {
                try
                {
                   
                    Clientes cliente = new Clientes();
                    //Reader que lee sólo los datos
                    MySqlDataReader myreader = cliente.Consultar(txtConsulta.Text);
                    myreader.Read();
                    txtCorreo.BackColor = SystemColors.Window;
                    gBDatosPersonales.Enabled = true;
                    gBDomicilio.Enabled = true;

                    btnGenerarCredencial.Visible = true;
                    btnGuardar.Text = "Guardar";
                    btnGuardar.Enabled = true;
                    btnCapturar.Enabled = false;
                    btnIniciar.Enabled = true;
                    PBFoto.Image = cliente.ConsultarImagen(txtConsulta.Text);

                    consulta = true;
                    IDCliente = myreader["ID_Clientes"].ToString();
                    txtNombre.Text = myreader["Nombre"].ToString();
                    txtApPat.Text = myreader["Ap_Paterno"].ToString();
                    txtApMat.Text = myreader["Ap_Materno"].ToString();
                    cmbGenero.Text = myreader["Genero"].ToString();
                    dtPNac.Value = DateTime.Parse(myreader["Fecha_Nacimiento"].ToString());

                    txtEdad.Text = myreader["Edad"].ToString();
                    txtResponsable.Text = myreader["Responsable"].ToString();
                    txtTeléfonoRes.Text = myreader["No_TelefonoC"].ToString();
                    txtCorreo.Text = myreader["Email"].ToString();
                    txtTel.Text = myreader["No_Telefono"].ToString();
                    txtCalle.Text = myreader["Calle"].ToString();
                    txtInt.Text = myreader["Num_Int"].ToString();
                    txtExt.Text = myreader["Num_Ext"].ToString();
                    txtDelegacion.Text = myreader["Delegacion"].ToString();
                    txtColonia.Text = myreader["Colonia"].ToString();
                    txtEstado.Text = myreader["Estado"].ToString();
                    txtCP.Text = myreader["CP"].ToString();


                }
                catch (Exception)
                {

                    MessageBox.Show("No se encontraron coincidencias");
                    txtConsulta.ResetText();
                }

            }
            else
            {
                DialogResult result = MessageBox.Show("Escriba el ID", "Faltan Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }






        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            txtCorreo.BackColor = SystemColors.Window;
            gBDatosPersonales.Enabled = true;
            gBDomicilio.Enabled = true;
            limpiarGeneral(gBDatosPersonales);
            limpiarGeneral(gBDomicilio);
            PBFoto.Image = null;
            txtConsulta.Text = "";
            btnGenerarCredencial.Visible = false;
            btnGuardar.Enabled = true;
            btnGuardar.Text = "Registrar";
            btnCapturar.Enabled = true;
            btnIniciar.Enabled = true;
            consulta = false;


        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Correo())
            {

                bool replica = false;

                string FNac = dtPNac.Value.ToString("yyyyMMdd");

                if (label57.Text.Contains("OBLIGATORIO"))
                {

                    if (!string.IsNullOrWhiteSpace(txtNombre.Text) && PBFoto.Image != null && !string.IsNullOrWhiteSpace(txtApMat.Text) && !string.IsNullOrWhiteSpace(txtApPat.Text) && cmbGenero.Text != "" && !string.IsNullOrWhiteSpace(txtEdad.Text) && !string.IsNullOrWhiteSpace(txtCorreo.Text) && !string.IsNullOrWhiteSpace(txtTel.Text) && !string.IsNullOrWhiteSpace(txtCalle.Text) && !string.IsNullOrWhiteSpace(txtColonia.Text) && !string.IsNullOrWhiteSpace(txtDelegacion.Text) && !string.IsNullOrWhiteSpace(txtExt.Text) && !string.IsNullOrWhiteSpace(txtEstado.Text) && !string.IsNullOrWhiteSpace(txtCP.Text) && !string.IsNullOrWhiteSpace(txtTeléfonoRes.Text) && !string.IsNullOrWhiteSpace(txtResponsable.Text))
                    {




                        if (btnGuardar.Text.Equals("Registrar"))
                        {
                            enlace.Conectar();
                            MySqlDataReader reader = enlace.Consultar("select Nombre, Ap_Paterno, Ap_Materno from Clientes;");
                            while (reader.Read())
                            {
                                if (txtNombre.Text.Equals(reader["Nombre"]) && txtApPat.Text.Equals(reader["Ap_Paterno"]) && txtApMat.Text.Equals(reader["Ap_Materno"]))
                                {

                                    replica = true;
                                    break;
                                }
                            }
                            enlace.Cerrar();
                            if (replica)
                            {
                                MessageBox.Show("Se encontro una replica del Cliente ingresado", "Cliente existente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                Menor menor = new Menor(txtNombre.Text, txtApPat.Text, txtApMat.Text, cmbGenero.Text, FNac, txtEdad.Text, txtResponsable.Text, txtCorreo.Text, "Inactivo", txtTel.Text, txtCalle.Text, txtCP.Text, txtInt.Text, txtExt.Text, txtDelegacion.Text, txtColonia.Text, txtEstado.Text, PBFoto.Image, txtTeléfonoRes.Text);
                                menor.Registrar();
                                DialogResult result = MessageBox.Show("El registro del nuevo cliente fue exitoso", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                printPreviewDialog1.Document = printDocument1;
                                printPreviewDialog1.ShowDialog();
                            }

                        }
                        if (btnGuardar.Text.Equals("Guardar"))
                        {
                            Menor menor = new Menor(txtNombre.Text, txtApPat.Text, txtApMat.Text, cmbGenero.Text, FNac, txtEdad.Text, txtResponsable.Text, txtCorreo.Text, "Inactivo", txtTel.Text, txtCalle.Text, txtCP.Text, txtInt.Text, txtExt.Text, txtDelegacion.Text, txtColonia.Text, txtEstado.Text, PBFoto.Image, txtTeléfonoRes.Text);
                            menor.Modificar(IDCliente);
                            DialogResult result = MessageBox.Show("Modificaciones guardadas", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }





                        txtConsulta.Text = "";
                        btnGenerarCredencial.Visible = false;
                        btnCapturar.Enabled = false;
                        PBFoto.Image = null;
                        limpiarGeneral(gBDatosPersonales);
                        limpiarGeneral(gBDomicilio);
                        gBDatosPersonales.Enabled = false;
                        gBDomicilio.Enabled = false;


                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Faltan campos por llenar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (txtNombre.Text != "" && PBFoto.Image != null && txtApMat.Text != "" && txtApPat.Text != "" && cmbGenero.Text != "" && txtEdad.Text != "" && txtCorreo.Text != "" && txtTel.Text != "" && txtCalle.Text != "" && txtColonia.Text != "" && txtDelegacion.Text != "" && txtExt.Text != "" && txtEstado.Text != "" && txtCP.Text != "")
                    {



                        Menor menor = new Menor(txtNombre.Text, txtApPat.Text, txtApMat.Text, cmbGenero.Text, FNac, txtEdad.Text, txtResponsable.Text, txtCorreo.Text, "Inactivo", txtTel.Text, txtCalle.Text, txtCP.Text, txtInt.Text, txtExt.Text, txtDelegacion.Text, txtColonia.Text, txtEstado.Text, PBFoto.Image, txtTeléfonoRes.Text);


                        if (btnGuardar.Text.Equals("Registrar"))
                        {

                            enlace.Conectar();
                            MySqlDataReader reader = enlace.Consultar("select Nombre, Ap_Paterno, Ap_Materno from Clientes;");
                            while (reader.Read())
                            {
                                if (txtNombre.Text.Equals(reader["Nombre"]) && txtApPat.Text.Equals(reader["Ap_Paterno"]) && txtApMat.Text.Equals(reader["Ap_Materno"]))
                                {

                                    replica = true;
                                    break;
                                }
                            }
                            enlace.Cerrar();
                            if (replica)
                            {
                                MessageBox.Show("Se encontró una replica del los datos ingresados", "Cliente existente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                menor.Registrar();
                                DialogResult result = MessageBox.Show("El registro del nuevo cliente fue exitoso", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                printPreviewDialog1.Document = printDocument1;
                                printPreviewDialog1.ShowDialog();
                            }
                        }
                        if (btnGuardar.Text.Equals("Guardar"))
                        {
                            menor.Modificar(IDCliente);
                            DialogResult result = MessageBox.Show("Modificaciones guardadas", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }





                        txtConsulta.Text = "";
                        btnGenerarCredencial.Visible = false;
                        btnCapturar.Enabled = false;
                        PBFoto.Image = null;
                        limpiarGeneral(gBDatosPersonales);
                        limpiarGeneral(gBDomicilio);
                        gBDatosPersonales.Enabled = false;
                        gBDomicilio.Enabled = false;


                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Faltan campos por llenar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }





        }

        private void label53_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardarCurso_Click(object sender, EventArgs e)
        {
            bool replica = false;
            bool encimado = false;
            if (!string.IsNullOrWhiteSpace(txtActividad.Text) && cmbEstatusCurso.Text != "" && !string.IsNullOrWhiteSpace(txtInstructor.Text) && cmbZona.Text != "" && !string.IsNullOrWhiteSpace(dtpHrFin.Text) && !string.IsNullOrWhiteSpace(dtpHrInicio.Text) && (ckLDias.GetItemChecked(1) || ckLDias.GetItemChecked(2) || ckLDias.GetItemChecked(3) || ckLDias.GetItemChecked(4) || ckLDias.GetItemChecked(5) || ckLDias.GetItemChecked(6) || ckLDias.GetItemChecked(0)) && !string.IsNullOrWhiteSpace(numCupo.Text) &&
           !string.IsNullOrWhiteSpace(txtDescrip.Text) &&
            !string.IsNullOrWhiteSpace(numPrecio.Text) && cmbClasificacion.Text!="")
            {
                try
                {
                    string zona = cmbZona.SelectedItem.ToString();
                    string dias = "";
                    foreach (object itemChecked in ckLDias.CheckedItems)
                    {

                        dias += itemChecked.ToString() + ",";


                    }

                    Cursos cursos = new Cursos(txtActividad.Text, dtpHrInicio.Text, dtpHrFin.Text, dias, cmbEstatusCurso.Text, txtInstructor.Text, txtDescrip.Text, numPrecio.Value.ToString(), numCupo.Value.ToString(), ("0").ToString(), zona, cmbClasificacion.Text);
                    if (btnGuardarCurso.Text.Equals("Registrar"))
                    {
                        enlace.Conectar();

                        MySqlDataReader reader = enlace.Consultar("select Nombre, HorarioIni, HorarioFin, Dias_semana,Instructor,Lugar, Estatus from cursos;");
                        while (reader.Read())
                        {
                            string hi = reader["HorarioIni"].ToString().Substring(0, 5);
                            string hf = reader["HorarioFin"].ToString().Substring(0, 5);
                            




                            if ((txtActividad.Text.Equals(reader["Nombre"]) && dtpHrInicio.Text.Equals(hi) && dtpHrFin.Text.Equals(hf) && dias.Contains(reader["Dias_semana"].ToString()) && txtInstructor.Text.Equals(reader["Instructor"])) || (txtActividad.Text.Equals(reader["Nombre"]) && dtpHrInicio.Text.Equals(hi) && dtpHrFin.Text.Equals(hf) && dias.Contains(reader["Dias_semana"].ToString()) && cmbZona.Text.Equals(reader["Lugar"])))
                            {
                                replica = true;



                            }
                            if (dias.Contains(reader["Dias_semana"].ToString()) && cmbZona.Text.Equals(reader["Lugar"]) && reader["Estatus"].Equals("Activo") && dtpHrInicio.Text.Equals(hi) && dtpHrFin.Text.Equals(hf))
                            {
                                encimado = true;
                            }

                        }
                        enlace.Cerrar();



                        if (replica || encimado)
                        {
                            if (replica)
                            {



                                MessageBox.Show("Existe una replica del Curso.", "Curso Existente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            if (encimado)
                            {
                                MessageBox.Show("Existe un traslape en el horario del curso.", "Traslape", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            cursos.Registrar();
                        }


                    }

                    if (btnGuardarCurso.Text.Equals("Guardar"))

                    {
                        enlace.Conectar();

                        MySqlDataReader reader = enlace.Consultar("select Nombre, HorarioIni, HorarioFin, Dias_semana,Instructor,Lugar, Estatus from cursos where ID_Cursos!=" + IDCurso + ";");
                        while (reader.Read())
                        {
                            string hi = reader["HorarioIni"].ToString().Substring(0, 5);
                            string hf = reader["HorarioFin"].ToString().Substring(0, 5);
                            




                            if ((txtActividad.Text.Equals(reader["Nombre"]) && dtpHrInicio.Text.Equals(hi) && dtpHrFin.Text.Equals(hf) && dias.Contains(reader["Dias_semana"].ToString()) && txtInstructor.Text.Equals(reader["Instructor"])) || (txtActividad.Text.Equals(reader["Nombre"]) && dtpHrInicio.Text.Equals(hi) && dtpHrFin.Text.Equals(hf) && dias.Contains(reader["Dias_semana"].ToString()) && cmbZona.Text.Equals(reader["Lugar"])))
                            {
                                replica = true;



                            }
                            if (dias.Contains(reader["Dias_semana"].ToString()) && cmbZona.Text.Equals(reader["Lugar"]) && reader["Estatus"].Equals("Activo") && dtpHrInicio.Text.Equals(hi) && dtpHrFin.Text.Equals(hf))
                            {
                                encimado = true;
                            }

                        }
                        enlace.Cerrar();



                        if (replica || encimado)
                        {
                            if (replica)
                            {



                                MessageBox.Show("Existe una replica del Curso.", "Curso Existente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            if (encimado)
                            {
                                MessageBox.Show("Existe un traslape en el horario del curso.", "Traslape", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            cursos.Modificar(IDCurso);
                        }


                    }
                    dataGridCursos.DataSource = enlace.tabla("select * from Cursos");
                    
                    enlace.Cerrar();



                    btnNuevo.Enabled = false;
                    limpiar();
                    limpiarGeneral(gBCurso);
                    numCupo.Value = numCupo.Minimum;
                    numPrecio.Value = numPrecio.Minimum;
                    txtActividad.Enabled = false;
                    cmbEstatusCurso.Enabled = false;
                    cmbClasificacion.Enabled = false;
                    txtInstructor.Enabled = false;
                    cmbZona.Enabled = false;
                    dtpHrFin.Enabled = false;
                    dtpHrInicio.Enabled = false;
                    ckLDias.Enabled = false;
                    numCupo.Enabled = false;
                    txtDescrip.Enabled = false;
                    numPrecio.Enabled = false;




                    btnGuardarCurso.Visible = false;

                }
                catch (Exception)
                {
                    MessageBox.Show("Algo salío mal, verifica tus cambios");
                }

            }
            else
            {
                DialogResult result = MessageBox.Show("Faltan campos por llenar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void frmMenu_Load(object sender, EventArgs e)
        {

            //este es el Load del formulario


            dataGridCursos.DataSource = enlace.tabla("select * from Cursos");

            enlace.Cerrar();
            dataGridCursos.ClearSelection();
           

           


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)

        {
            try
            {
                limpiar();
                btnGuardarCurso.Text = "Guardar";
                btnGuardarCurso.Visible = true;
                txtActividad.Enabled = true;
                cmbEstatusCurso.Enabled = true;
                cmbClasificacion.Enabled = true;
                txtInstructor.Enabled = true;
                cmbZona.Enabled = true;
                dtpHrFin.Enabled = true;
                dtpHrInicio.Enabled = true;
                ckLDias.Enabled = true;
                numCupo.Enabled = true;
                txtDescrip.Enabled = true;
                numPrecio.Enabled = true;







                DataGridViewRow row = dataGridCursos.Rows[e.RowIndex];
                numPrecio.Value = Convert.ToDecimal(row.Cells[8].Value);
                numCupo.Value = Convert.ToDecimal(row.Cells[9].Value);
                numPrecio.Value = Convert.ToDecimal(row.Cells[8].Value);
                numCupo.Value = Convert.ToDecimal(row.Cells[9].Value);
                numPrecio.Value = Convert.ToDecimal(row.Cells[8].Value);
                numCupo.Value = Convert.ToDecimal(row.Cells[9].Value);
                IDCurso = row.Cells[0].Value.ToString();
                txtActividad.Text = row.Cells[1].Value.ToString();
                dtpHrInicio.Value = DateTime.Parse(row.Cells[2].Value.ToString());
                dtpHrFin.Value = DateTime.Parse(row.Cells[3].Value.ToString());
                String[] dias;
                dias = row.Cells[4].Value.ToString().Split(',');
                for (int n = 0; n < dias.Length; n++)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (dias[n].Contains(ckLDias.Items[i].ToString()))
                        {

                            ckLDias.SetItemChecked(i, true);
                        }
                    }

                }
                cmbEstatusCurso.Text = row.Cells[5].Value.ToString();
                txtInstructor.Text = row.Cells[6].Value.ToString();
                txtDescrip.Text = row.Cells[7].Value.ToString();


                cmbZona.Text = row.Cells[11].Value.ToString();
                cmbClasificacion.Text = row.Cells[12].Value.ToString();
            }
            catch (Exception)
            {

            }

        }

        public void btnIniciar_Click(object sender, EventArgs e)

        {

            btnIniciar.Visible = false;
            btnCapturar.Visible = true;
            btnCapturar.Enabled = true;
            camara = new Camara(PBFoto, btnCapturar);
            camara.Iniciar();

        }

        private void btnCapturar_Click(object sender, EventArgs e)
        {
            camara.TerminarFuenteDeVideo();
            btnCapturar.Enabled = true;
            btnCapturar.Visible = false;
            btnIniciar.Visible = true;
            btnIniciar.Enabled = true;
        }
        private void limpiar()
        {

            for (int i = 0; i < 7; i++)
            {

                ckLDias.SetItemChecked(i, false);

            }

        }

        private void gBCurso_Enter(object sender, EventArgs e)
        {

        }

        private void label54_Click(object sender, EventArgs e)
        {

        }

        private void cmbEstatusCurso_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnNuevoCurso_Click(object sender, EventArgs e)
        {



            limpiar();
            limpiarGeneral(gBCurso);


            numCupo.Value = numCupo.Minimum;
            numPrecio.Value = numPrecio.Minimum;
            txtActividad.Enabled = true;
            cmbEstatusCurso.Enabled = true;
            cmbClasificacion.Enabled = true;
            txtInstructor.Enabled = true;
            cmbZona.Enabled = true;
            dtpHrFin.Enabled = true;
            dtpHrInicio.Enabled = true;
            ckLDias.Enabled = true;
            numCupo.Enabled = true;
            txtDescrip.Enabled = true;
            numPrecio.Enabled = true;



            btnGuardarCurso.Text = "Registrar";
            btnGuardarCurso.Visible = true;
        }

        private void btnConsultaInscrip_Click(object sender, EventArgs e)
        {
            if (txtConsultaInscrip.Text != "")
            {
                try
                {
                    
                    Clientes cliente = new Clientes();

                    MySqlDataReader myreader = cliente.Consultar(txtConsultaInscrip.Text);
                    myreader.Read();
                    IDCliente = myreader["ID_Clientes"].ToString();
                    txtNombreInscrip.Text = myreader["Nombre"].ToString();
                    txtApPatInscrip.Text = myreader["Ap_Paterno"].ToString();
                    txtApMatInscrip.Text = myreader["Ap_Materno"].ToString();
                    
                    dataGridCursosCliente.DataSource = enlace.tabla("select ID_Inscritos,FechaIni, FechaFin,Inscritos.Estatus, ID_ClientesF, Clientes.Nombre, Ap_Paterno, Ap_Materno, Cursos.Nombre, HorarioIni,HorarioFin,Dias_semana,Lugar, TipoPago, Precio FROM((Inscritos INNER JOIN Clientes ON Inscritos.ID_ClientesF = Clientes.ID_Clientes) INNER JOIN Cursos ON Inscritos.ID_CursosF = Cursos.ID_Cursos) where ID_ClientesF=" + txtConsultaInscrip.Text + " ORDER BY ID_Inscritos ASC ; ");
                    dataGridCursosInscripciones.Visible = true;
                    if (Convert.ToInt32(myreader["Edad"]) < 4)
                    {
                        dataGridCursosInscripciones.DataSource = enlace.tabla("select * from Cursos where Clasificacion='Bébes'");
                    }
                    else
                    {
                        if (Convert.ToInt32(myreader["Edad"]) < 17 && Convert.ToInt32(myreader["Edad"]) >3)
                        {
                            dataGridCursosInscripciones.DataSource = enlace.tabla("select * from Cursos where Clasificacion='Niños'");
                        }
                        else
                        {
                            if (Convert.ToInt32(myreader["Edad"]) > 16)
                            {
                                dataGridCursosInscripciones.DataSource = enlace.tabla("select * from Cursos where Clasificacion='Adultos'");
                            }
                        }
                    }
                    enlace.Cerrar();
                    grpCursosInscripcion.Enabled = true;
                    dataGridCursosInscripciones.Enabled = true;
                    dataGridCursosCliente.Visible = true;
                    btnEliminarInscripcion.Visible = true;
                }
                catch (Exception)
                {

                    MessageBox.Show("No se encontraron coincidencias.");
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Escriba el ID", "Faltan Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridCursosInscripciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {


                DataGridViewRow row = dataGridCursosInscripciones.Rows[e.RowIndex];
                if (row.Cells[9].Value.ToString().Equals(row.Cells[10].Value.ToString()))
                {
                    MessageBox.Show("El Curso seleccionado está lleno");
                    IDCurso = "";
                    grpPago.Enabled = false;
                    btnInscribir.Enabled = false;
                }

                else
                {
                    if (row.Cells[5].Value.ToString().Equals("Inactivo"))
                    {
                        MessageBox.Show("El Curso seleccionado está Inactivo");
                        IDCurso = "";
                        grpPago.Enabled = false;
                        btnInscribir.Enabled = false;
                    }
                    else
                    {

                        grpPago.Enabled = true;
                        btnInscribir.Enabled = true;
                        IDCurso = row.Cells[0].Value.ToString();
                    }

                }
            }
            catch (Exception)
            {


            }


        }








        private void btnConsultarProgresos_Click(object sender, EventArgs e)
        {
            if (txtConsultaProgresos.Text != "") {

                try
                {
                    IDProgreso = "";
                    LimpiarProgreso();




                    Clientes cliente = new Clientes();

                    MySqlDataReader myreader = cliente.Consultar(txtConsultaProgresos.Text);
                    myreader.Read();
                    if (Convert.ToInt32(myreader["edad"]) > 16){

                        IDCliente = myreader["ID_Clientes"].ToString();
                        txtNombreProgresos.Text = myreader["Nombre"].ToString();
                        txtApPatProgresos.Text = myreader["Ap_Paterno"].ToString();
                        txtApMatProgresos.Text = myreader["Ap_Materno"].ToString();
                        enlace.Conectar();
                        dataGridProgresos.DataSource = enlace.tabla("select * from Progreso where ID_ClientesF2=" + IDCliente);

                        enlace.Cerrar();
                        dataGridProgresos.ClearSelection();
                        dataGridProgresos.Visible = true;
                        btnNuevoProgreso.Enabled = true;
                        try
                        {
                            dataGridProgresos.Rows[0].Visible = true;
                            btnGrafico.Visible = true;
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        MessageBox.Show("Este servicio sólo es para mayores de 16", "Servicio no disponible", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                }
                catch (Exception)
                {

                    MessageBox.Show("No se encontró el usuario.", "No hay coincidencias.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

            else
            {
               MessageBox.Show("Escriba el ID", "Faltan Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnGuardarProg_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnGuardarProg.Text.Equals("Registrar"))
                {
                    enlace.Conectar();
                    MySqlDataReader reader = enlace.Consultar("select Fecha from progreso where ID_ClientesF2=" + IDCliente + " and Fecha>=date_add(current_date(), INTERVAL -30 DAY) ;;");
                    if (reader.Read())
                    {
                        MessageBox.Show("Los registros son mensuales, espera al menos 30 días.", "Fuera de Periodo",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                        enlace.Cerrar();

                    }

                    else {
                        Progresos progreso = new Progresos(IDCliente, dtpFProg.Value.ToString("yyyyMMdd"), numMasa.Value.ToString(), NumTalla.Value.ToString(), numCintura.Value.ToString(), numCadera.Value.ToString(), numPecho.Value.ToString(), numBíceps.Value.ToString(), numMunecas.Value.ToString(), numCuello.Value.ToString(), numPantorrilas.Value.ToString(), txtIMC.Text);
                        progreso.Registrar();
                        enlace.Conectar();
                        dataGridProgresos.DataSource = enlace.tabla("select * from Progreso where ID_ClientesF2=" + IDCliente);
                        enlace.Cerrar();
                    }

                }

                if (btnGuardarProg.Text.Equals("Guardar"))
                {
                    Progresos progreso = new Progresos(IDCliente, dtpFProg.Value.ToString("yyyyMMdd"), numMasa.Value.ToString(), NumTalla.Value.ToString(), numCintura.Value.ToString(), numCadera.Value.ToString(), numPecho.Value.ToString(), numBíceps.Value.ToString(), numMunecas.Value.ToString(), numCuello.Value.ToString(), numPantorrilas.Value.ToString(), txtIMC.Text);
                    progreso.Modificar(IDProgreso);
                    enlace.Conectar();
                    dataGridProgresos.DataSource = enlace.tabla("select * from Progreso where ID_ClientesF2=" + IDCliente);
                    enlace.Cerrar();
                    IDProgreso = "";
                }
                LimpiarProgreso();
                btnGuardarProg.Visible = false;
            }
            catch (Exception)
            {


                MessageBox.Show("Faltan campos por llenar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void NumTalla_ValueChanged(object sender, EventArgs e)
        {
            if (numMasa.Value != 0 && NumTalla.Value != 0)
            {
                Progresos pro = new Progresos();
                txtIMC.Text = (pro.Calcular(Convert.ToDouble(numMasa.Value), Convert.ToDouble(NumTalla.Value))).ToString();

            }
        }

        private void btnGrafico_Click(object sender, EventArgs e)
        {
            Graficos graficos = new Graficos(IDCliente);
            graficos.Show();

        }





        private void txtConsultaAcceso_KeyPress(object sender, KeyPressEventArgs e)
        {
            IDCliente = "";
            Avisos.Visible = false;
            picAviso.Visible = false;
            txtAvisos.Visible = false;
            if (e.KeyChar == (char)13)
            {


                char[] arreglo = txtConsultaAcceso.Text.ToCharArray();
                foreach (char caracter in arreglo)
                {
                    if (Char.IsDigit(caracter))
                    {
                        IDCliente += caracter;
                    }


                }

                try
                {
                    Clientes cliente = new Clientes();
                    //Reader que lee sólo los datos
                    MySqlDataReader myreader = cliente.Consultar(IDCliente);
                    myreader.Read();
                    txtNombreAcceso.Text = myreader["Nombre"].ToString();
                    txtApPatAcceso.Text = myreader["Ap_Paterno"].ToString();
                    txtApMatAcceso.Text = myreader["Ap_Materno"].ToString();
                    if (myreader["Estatus"].ToString().Equals("Inactivo"))
                    {
                        lblEstatus.ForeColor = Color.Red;
                        System.Media.SystemSounds.Beep.Play();
                        System.Media.SystemSounds.Beep.Play();
                    }
                    else
                    {
                        lblEstatus.ForeColor = Color.LightGreen;
                    }
                    lblEstatus.Text = myreader["Estatus"].ToString();
                    PBFotoAcceso.Image = cliente.ConsultarImagen(IDCliente);
                    myreader.Close();
                    try
                    {
                        enlace.Conectar();

                        myreader = enlace.Consultar("select Cursos.Nombre, FechaIni, FechaFin " +
                        "FROM((Inscritos) INNER JOIN Cursos ON Inscritos.ID_CursosF = Cursos.ID_Cursos) where ID_ClientesF =" + IDCliente + " and FechaFin < CURRENT_DATE() + 2 and FechaFin > Current_date(); ");




                        while (myreader.Read())
                        {
                            txtAvisos.Text = "Cursos próximos a vencer:\n\n";
                            txtAvisos.Visible = true;
                            Avisos.Visible = true;
                            picAviso.Visible = true;
                            txtAvisos.Visible = true;

                            txtAvisos.Text = txtAvisos.Text + myreader["Nombre"].ToString() + "   vence     " + myreader["FechaFin"].ToString().Substring(0, 10) + "\n";
                        }
                        enlace.Cerrar();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("algo salió mal");
                    }



                }
                catch (Exception)
                {

                    MessageBox.Show("No se encontro ninguna coincidencia");
                }

            }
        }



        private void tCGeneral_Click(object sender, EventArgs e)
        {
            txtConsultaAcceso.Focus();
        }

        private void btnInscribir_Click(object sender, EventArgs e)
        {
            if (cmbTipoPago.Text != "")
            {
                DialogResult result = MessageBox.Show("¿Desea inscribir al Cliente " + IDCliente + " al curso " + IDCurso + "?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string FechaIni = dtpPago.Value.ToString("yyyyMMdd");
                    string FechaFin = dtpPago.Value.AddDays(35).ToString("yyyyMMdd");


                    Inscripciones inscripciones = new Inscripciones(IDCliente, FechaIni, FechaFin, IDCurso, cmbTipoPago.Text);
                    if (inscripciones.Verificar() == 1)
                    {

                        Cursos cursos = new Cursos();
                        inscripciones.Inscribir();
                        cursos.ActualizarInscritos(IDCurso, 1);
                        dataGridCursosCliente.DataSource = enlace.tabla("select ID_Inscritos,FechaIni, FechaFin,Inscritos.Estatus, ID_ClientesF, Clientes.Nombre, Ap_Paterno, Ap_Materno, Cursos.Nombre, HorarioIni,HorarioFin,Dias_semana,Lugar, TipoPago, Precio FROM((Inscritos INNER JOIN Clientes ON Inscritos.ID_ClientesF = Clientes.ID_Clientes) INNER JOIN Cursos ON Inscritos.ID_CursosF = Cursos.ID_Cursos) where ID_ClientesF=" + IDCliente + " ORDER BY ID_Inscritos ASC ; ");
                        enlace.ActualizarEstatus();
                        enlace.Cerrar();
                        printPreviewDialogInscripcion.Document = printDocumentInscripcion;
                        printPreviewDialogInscripcion.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("ERROR.\nEl Cliente ya se encuentra inscrito a esta actividad.");
                    }




                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Faltan campos por llenar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        private void dataGridCursosCliente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = dataGridCursosCliente.Rows[e.RowIndex];
                //row.Selected = true;
                btnEliminarInscripcion.Enabled = true;
                btnEliminarInscripcion.Visible = true;
                IDInscripcion = row.Cells[0].Value.ToString();
            }
            catch (Exception)
            {

            }






        }

        private void btnEliminarInscripcion_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Desea eliminar el registro seleccionado? ", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Inscripciones inscripcion = new Inscripciones();
                inscripcion.Eliminar(IDInscripcion);
                enlace.Conectar();
                dataGridCursosCliente.DataSource = enlace.tabla("select ID_Inscritos,FechaIni, FechaFin,Inscritos.Estatus, ID_ClientesF, Clientes.Nombre, Ap_Paterno, Ap_Materno, Cursos.Nombre, HorarioIni,HorarioFin,Dias_semana,Lugar, TipoPago, Precio FROM((Inscritos INNER JOIN Clientes ON Inscritos.ID_ClientesF = Clientes.ID_Clientes) INNER JOIN Cursos ON Inscritos.ID_CursosF = Cursos.ID_Cursos) where ID_ClientesF=" + txtConsultaInscrip.Text + " ORDER BY ID_Inscritos ASC ; ");
                dataGridCursosCliente.ClearSelection();
                enlace.ActualizarEstatus();
                enlace.Cerrar();

                btnEliminarInscripcion.Enabled = false;
                btnEliminarInscripcion.Visible = false;
            }

        }

        private void dataGridProgresos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                habilitarProgreso();
                DataGridViewRow row = dataGridProgresos.Rows[e.RowIndex];
                row.Selected = true;
                btnGuardarProg.Text = "Guardar";
                btnGuardarProg.Enabled = true;
                btnGuardarProg.Visible = true;

                IDProgreso = row.Cells[0].Value.ToString();
                numMasa.Value = Convert.ToDecimal(row.Cells[3].Value);
                NumTalla.Value = Convert.ToDecimal(row.Cells[4].Value);
                numCintura.Value = Convert.ToDecimal(row.Cells[5].Value);
                numCadera.Value = Convert.ToDecimal(row.Cells[6].Value);
                numPecho.Value = Convert.ToDecimal(row.Cells[7].Value);
                numBíceps.Value = Convert.ToDecimal(row.Cells[8].Value);
                numMunecas.Value = Convert.ToDecimal(row.Cells[9].Value);
                numCuello.Value = Convert.ToDecimal(row.Cells[10].Value);
                numPantorrilas.Value = Convert.ToDecimal(row.Cells[11].Value);
                txtIMC.Text = row.Cells[12].Value.ToString();

            }
            catch (Exception)
            {


            }
        }
        private void habilitarProgreso()
        {
            numMasa.Enabled = true;
            NumTalla.Enabled = true;
            numCintura.Enabled = true;
            numCadera.Enabled = true;
            numPecho.Enabled = true;
            numBíceps.Enabled = true;
            numMunecas.Enabled = true;
            numCuello.Enabled = true;
            numPantorrilas.Enabled = true;
        }
        private void LimpiarProgreso()
        {
            IDProgreso = "";
            
            numMasa.Value = numMasa.Minimum;
            NumTalla.Value = NumTalla.Minimum;
            numCintura.Value = numCintura.Minimum;
            numCadera.Value = numCadera.Minimum;
            numPecho.Value = numPecho.Minimum;
            numBíceps.Value = numBíceps.Minimum;
            numMunecas.Value = numMunecas.Minimum;
            numCuello.Value = numCuello.Minimum;
            numPantorrilas.Value= numPantorrilas.Minimum;
            txtIMC.ResetText();
            //inhabilitar
            numMasa.Enabled = false;
            NumTalla.Enabled = false;
            numCintura.Enabled = false;
            numCadera.Enabled = false;
            numPecho.Enabled = false;
            numBíceps.Enabled = false;
            numMunecas.Enabled = false;
            numCuello.Enabled = false;
            numPantorrilas.Enabled = false;

        }


        private void btnNuevoProgreso_Click(object sender, EventArgs e)
        {
            dataGridProgresos.ClearSelection();
            IDProgreso = "";
            LimpiarProgreso();
            habilitarProgreso();
            btnNuevoProgreso.Text = "Nuevo";
            btnGuardarProg.Text = "Registrar";
            btnGuardarProg.Visible = true;
            btnGuardarProg.Enabled = true;


        }

        private void btnGenerarCredencial_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void dtPNac_ValueChanged(object sender, EventArgs e)
        {
            TimeSpan duracion = DateTime.Now - dtPNac.Value;
            Double años = (duracion.TotalDays) / 365;
            String Edad = Convert.ToString(Convert.ToInt32(años));
            txtEdad.Text = Edad;
        }
        private void limpiarGeneral(GroupBox g)
        {

            foreach (Control c in g.Controls)
            {
                Type t = c.GetType();

                if (t.Equals(typeof(System.Windows.Forms.TextBox)) || t.Equals(typeof(System.Windows.Forms.ComboBox)) || t.Equals(typeof(System.Windows.Forms.DateTimePicker)) ) {
                    c.ResetText();
                }
                else if (t.Equals(typeof(System.Windows.Forms.NumericUpDown)))
                {
                    c.Refresh();
                    

                }






            }

        }

        private void btnLimpiarProgresos_Click(object sender, EventArgs e)
        {
            txtConsultaProgresos.ResetText();
            LimpiarProgreso();
            txtNombreProgresos.ResetText();
            txtApMatProgresos.ResetText();
            txtApPatProgresos.ResetText();
            limpiarGeneral(grpSalud);
            dataGridProgresos.Visible = false;
            IDCliente = "";
            IDProgreso = "";
            btnGrafico.Visible = false;
        }



        private void splitContainer4_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Numero_entero(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar) < 48 && e.KeyChar != 8 && e.KeyChar != 46) || e.KeyChar > 57)
            {
                MessageBox.Show("Sólo se permiten Números");
                e.Handled = true;
            }
        }

        private void Letras(object sender, KeyPressEventArgs e)
        {
            Regex rgx = new Regex(@"[A-z]|\s|[\b]|á|é|í|ó|ú|ñ|Á|É|Í|Ó|Ú");
            if (rgx.IsMatch(e.KeyChar.ToString()))
            {
               
            }
            else
            {
                MessageBox.Show("Sólo se permiten letras");
                e.Handled = true;
                
            }
            Console.WriteLine(e.KeyChar.ToString());

            /*  if (((65< e.KeyChar && e.KeyChar > 90)&&(97 < e.KeyChar && e.KeyChar > 122)  && e.KeyChar != 8 && e.KeyChar != 32 && 65 < e.KeyChar && e.KeyChar < 9))
              {
                  MessageBox.Show("Sólo se permiten letras");
                  e.Handled = true;
              }*/
        }

        private bool Correo()
        {
            Regex rgx = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            String correo=txtCorreo.Text;
            if (!rgx.IsMatch(correo))
            {
                
                txtCorreo.BackColor = Color.Red;

                
                MessageBox.Show("Formato de correo inválido","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtCorreo.Focus();
                return false;


            }
            else
            {
                txtCorreo.BackColor = SystemColors.Window;
                
                return true;
            }
        }

        private void btnLimpiarInscripciones_Click(object sender, EventArgs e)
        {
            dataGridCursosInscripciones.Visible = false;
            txtConsultaInscrip.ResetText();
            txtNombreInscrip.ResetText();
            txtApMatInscrip.ResetText();
            txtApPatInscrip.ResetText();
            limpiarGeneral(grpPago);
            IDInscripcion="";
            IDCliente = "";
            dataGridCursosCliente.Visible = false;
            btnEliminarInscripcion.Visible = true;
            grpCursosInscripcion.Enabled = false;
            grpPago.Enabled = false;
            txtFiltraPagos.Text = "";
            btnEliminarInscripcion.Visible = false;

        }

        private void txtEdad_TextChanged(object sender, EventArgs e)
        {
           
           
        }

        private void txtFiltrar_TextChanged(object sender, EventArgs e)
        {
            DataTable tbl = dataGridCursos.DataSource as DataTable;
            
                tbl.DefaultView.RowFilter = $"Nombre LIKE '{txtFiltrar.Text}%'";
        }

        private void txtEdad_TextChanged_1(object sender, EventArgs e)
        {
            if (txtEdad.Text != "")
            {
                if (Convert.ToInt32(txtEdad.Text) < 18)
                {
                    label57.Text = "En caso de emergencia contactar a (OBLIGATORIO):";

                }
                else
                {
                    label57.Text = "En caso de emergencia contactar a (OPCIONAL):";
                }

            }
           

        }

        private void splitContainer9_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gBDatosPersonales_Enter(object sender, EventArgs e)
        {

        }

        private void dtpHrFin_Leave(object sender, EventArgs e)
        {
            if (dtpHrFin.Value < (dtpHrInicio.Value.AddHours(1)))
            {
                MessageBox.Show("Las clases durán 1 hora o más", "Hora incorrecta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtpHrFin.Focus();
            }
        }

        private void txtFiltraPagos_TextChanged(object sender, EventArgs e)
        {
            DataTable tbl = dataGridCursosInscripciones.DataSource as DataTable;

            tbl.DefaultView.RowFilter = $"Nombre LIKE '{txtFiltraPagos.Text}%'";
        }

       

       
        private void limpiartodo(object sender, EventArgs e)
        {
            //Cursos

            limpiar();
            limpiarGeneral(gBCurso);

            txtActividad.Enabled = false;
            cmbEstatusCurso.Enabled = false;
            cmbClasificacion.Enabled = false;
            txtInstructor.Enabled = false;
            cmbZona.Enabled = false;
            dtpHrFin.Enabled = false;
            dtpHrInicio.Enabled = false;
            ckLDias.Enabled = false;
            numCupo.Enabled = false;
            txtDescrip.Enabled = false;
            numPrecio.Enabled = false;
            btnGuardarCurso.Visible = false;
            numCupo.Value = numCupo.Minimum;
            numPrecio.Value = numPrecio.Minimum;

            //Clientes
            txtConsulta.Text = "";
            btnGenerarCredencial.Visible = false;
            btnCapturar.Enabled = false;
            PBFoto.Image = null;
            limpiarGeneral(gBDatosPersonales);
            limpiarGeneral(gBDomicilio);
            gBDatosPersonales.Enabled = false;
            gBDomicilio.Enabled = false;
            btnGuardar.Enabled = false;
            txtCorreo.BackColor = SystemColors.Window;

            //Progresos
            txtConsultaProgresos.ResetText();
            txtNombreProgresos.ResetText();
            txtApMatProgresos.ResetText();
            txtApPatProgresos.ResetText();
            limpiarGeneral(grpSalud);
            dataGridProgresos.Visible = false;
            IDCliente = "";
            IDProgreso = "";
            btnGrafico.Visible = false;
            //Acceso
            txtConsultaAcceso.Text = "";
            PBFotoAcceso.Image = null;
            txtNombreAcceso.ResetText();
            txtApMatAcceso.ResetText();
            txtApPatAcceso.ResetText();
            txtAvisos.ResetText();
            txtAvisos.Visible = false;
            picAviso.Visible = false;
            //Inscripciones
            txtConsultaInscrip.ResetText();
            txtNombreInscrip.ResetText();
            txtApMatInscrip.ResetText();
            txtApPatInscrip.ResetText();
            limpiarGeneral(grpPago);
            IDInscripcion = "";
            IDCliente = "";
            dataGridCursosCliente.Visible = false;
            btnEliminarInscripcion.Visible = true;
            grpCursosInscripcion.Enabled = false;
            grpPago.Enabled = false;
            txtFiltraPagos.Text = "";
            btnEliminarInscripcion.Visible = false;
            dataGridCursosInscripciones.Visible = false;

            IDCliente = "";
            IDCurso = "";
            IDInscripcion = "";
            IDProgreso = "";

        }


        private void gBDomicilio_Enter(object sender, EventArgs e)
        {

        }
    }
}
