using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Microsoft.Reporting.WinForms;

namespace Practica4
{
   
    public partial class Form1 : Form
    {
        List<Asiento> compra;
        List<Asiento> reserva;
        Sala sala;
        public Form1()
        {
            InitializeComponent();
        }

        public Form1(List<Asiento> compra, List<Asiento> reserva,Sala sala)
        {
            InitializeComponent();
            this.compra = compra;
            this.reserva = reserva;
            this.sala = sala;
            compra.AddRange(reserva);
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();

        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

            reportViewer1.LocalReport.EnableExternalImages = true;

            String asientos = "";

            foreach(Asiento a in compra)
            {
                asientos += "Asiento_" + a.columna + "_" + a.fila;
            }
            string ruta = Application.StartupPath + sala.ruta;

            var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            var qrCode = qrEncoder.Encode(asientos);
            var renderer = new GraphicsRenderer(new FixedModuleSize(5,
            QuietZoneModules.Two), Brushes.Black, Brushes.White);
            using (var stream = new FileStream(Application.StartupPath +
            @"\imagenes\qrcode.png", FileMode.Create))
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
            /*Para cargar la imagen de manera dinámica, primero hemos de
            agregar un parámetro para la ruta. Luego insertamos en el informe
            una imagen, el origen de la imagen ha de ser “externo2 y en valor
            establecemos el parámetro.
            Luego insertamos la ruta de la imagen en el parámetro string
            "rutaImagen" siendo qrcode.png el código generado en el paso
            anterior (como se muestra continuación)*/

            reportViewer1.LocalReport.EnableExternalImages = true;

            string ruta_qr = Application.StartupPath + @"\imagenes\qrcode.png";

            ReportParameter paramImagen = new ReportParameter("rutaimg_qr",
            @"file:\" + ruta_qr, true);
            reportViewer1.LocalReport.SetParameters(paramImagen);
            reportViewer1.RefreshReport();

            ReportParameter cartelera = new ReportParameter("rutaimg_cartelera",@"file:\"+ ruta, true);
            reportViewer1.LocalReport.SetParameters(cartelera);

            ReportParameter nombre = new ReportParameter("nombre_evento", sala.nombre_evento, true);
            reportViewer1.LocalReport.SetParameters(nombre);

            ReportParameter fecha = new ReportParameter("fecha_hora_evento", sala.hora, true);
            reportViewer1.LocalReport.SetParameters(fecha);

            double precio = 6.75 * compra.Count();
            ReportParameter importe = new ReportParameter("importe", precio+"€", true);
            reportViewer1.LocalReport.SetParameters(importe);



            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Asiento" ,compra));

            reportViewer1.RefreshReport();
        }
    }
}
