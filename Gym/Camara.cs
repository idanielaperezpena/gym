using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Windows.Forms;

namespace Gym
{
    public class Camara

    {
        
        public bool ExistenDispositivos = false;
        public FilterInfoCollection DispositivosDeVideo;
        public VideoCaptureDevice FuenteDeVideo = null;
        private PictureBox PBFoto;
        private Button btnIniciar;

        public Camara(PictureBox PBFoto, Button btnIniciar)
        {
            this.PBFoto = PBFoto;
            this.btnIniciar = btnIniciar;
        }
        public void Iniciar()
        {
            BuscarDispositivos();
            
                if (ExistenDispositivos)
                {
                    FuenteDeVideo = new VideoCaptureDevice(DispositivosDeVideo[0].MonikerString);


                    FuenteDeVideo.NewFrame += new NewFrameEventHandler(video_NuevoFrame);
                    FuenteDeVideo.Start();
                    btnIniciar.Text = "Detener";
                    Console.WriteLine(FuenteDeVideo.IsRunning);
                    
                    
                }
                else
                    MessageBox.Show("Error: No se encuentra dispositivo.");
            

        }
       

        public void BuscarDispositivos()
        {
            DispositivosDeVideo = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (DispositivosDeVideo.Count == 0)
                ExistenDispositivos = false;
            
            else
            {
                ExistenDispositivos = true;
            }
            
        }

        public void TerminarFuenteDeVideo()
        {
            if (!(FuenteDeVideo == null))
                if (FuenteDeVideo.IsRunning)
                {
                    FuenteDeVideo.SignalToStop();
                    FuenteDeVideo = null;
                }
        }

        public void video_NuevoFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
            
            PBFoto.Image = ScaleImage(Imagen, PBFoto.Width, PBFoto.Height);

        }
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

    }
}
