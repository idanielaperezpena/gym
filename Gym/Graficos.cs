
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;//Estilo
using MaterialSkin.Controls;//Estilo

namespace Gym
{
    public partial class Graficos : MaterialForm //Material Form para el estilo
    
    {
        private int Ypos;
        private int Xpos;
        public string ID_Cliente;
        private PrintDocument printDocument1 = new PrintDocument();
        public Graficos(string ID_Cliente)
        {
            InitializeComponent();
            // Material Manager (Estilos) 17/05/2018
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;


            // Configuración de color (Estilos) 17/05/2018
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue400, Primary.Blue500,
                Primary.Blue500, Accent.LightBlue200,
                TextShade.WHITE);




            this.ID_Cliente = ID_Cliente;

            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            PaperSize p = new PaperSize("Carta", 830, 1100);
            printDocument1.DefaultPageSettings.PaperSize=p;
        }

        private void Graficos_Load(object sender, EventArgs e)

        {
            Ypos = Location.Y;
            Xpos = Location.X;
            String query = "select *  from Progreso where ID_ClientesF2=" + ID_Cliente + ";";
            EnlaceDatos en = new EnlaceDatos();
            en.Conectar();
            MySqlDataReader myreader = en.Consultar(query);

            if (myreader.HasRows)
                while (myreader.Read())
                {
                    Console.WriteLine("\t{0}\t{1}", myreader.GetInt32(3), myreader.GetString(2));
                    chartIMC.Series["IMC"].Points.AddXY(myreader[2], myreader[12]);
                    chartIMC.Series["Masa"].Points.AddXY(myreader[2], myreader[3]);
                    
                    chartIMC.Series["Cintura"].Points.AddXY(myreader[2], myreader[5]);
                    chartIMC.Series["Cadera"].Points.AddXY(myreader[2], myreader[6]);
                    chartIMC.Series["Pecho"].Points.AddXY(myreader[2], myreader[7]);
                    chartIMC.Series["Bíceps"].Points.AddXY(myreader[2], myreader[8]);
                    chartIMC.Series["Muñecas"].Points.AddXY(myreader[2], myreader[9]);
                    chartIMC.Series["Cuello"].Points.AddXY(myreader[2], myreader[10]);
                    chartIMC.Series["Pantorrillas"].Points.AddXY(myreader[2], myreader[11]);
                }
            else
                Console.WriteLine("No rows returned.");

            // 




            
        }

        private void chartIMC_Click(object sender, EventArgs e)
        {

        }

        
        Bitmap memoryImage;

        private void CaptureScreen()
        {
            
            Graphics myGraphics = groupBox1.CreateGraphics();
            Size s = groupBox1.Size;
            memoryImage = new Bitmap(s.Width-40, s.Height, myGraphics);
           // memoryImage.SetResolution(380F, 380F);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CompositingQuality = CompositingQuality.HighSpeed;
            memoryGraphics.CopyFromScreen(this.Location.X+groupBox1.Location.X+40,groupBox1.Location.Y+30, 0,50, s);
            

        }
        int i=1;
        private void printDocument1_PrintPage(System.Object sender,
               System.Drawing.Printing.PrintPageEventArgs e)
        {
           if(i==2)
            {
            Image IMC = Image.FromFile("IMC.bmp");


            e.Graphics.DrawImage(IMC, 35, 30);
                i = 0;
            }
            //string src = Path.GetFullPath("\\Gym\\Resultados.bmp");
            if (i == 1)
            {
              Image Resultado = Image.FromFile("Resultados.bmp");

                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImage(Resultado, 35, 30);
                e.Graphics.DrawImage(memoryImage, 55, 400);
                i++;
               
            }
            
            e.HasMorePages = (i > 0);
        }

        private void btnImprimir_Click_1(object sender, EventArgs e)
        {
            CaptureScreen();

            //printDocument1.Print();

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void Graficos_Move(object sender, EventArgs e)
        {
            if ((Location.X != Xpos))
            {
                Location = new Point(Xpos, Ypos);
            }
        }
    }
}
