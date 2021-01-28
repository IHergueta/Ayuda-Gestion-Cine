using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Practica4
{
    /// <summary>
    /// Lógica de interacción para Salas_cine.xaml
    /// </summary>
    public partial class Salas_cine : Window
    {

        Boolean boton = true;
        Sala sala;
        MainWindow cartelera;
        List<Asiento> comprados;
        List<Asiento> reservados;

        public Salas_cine(Sala sala, MainWindow cartelera)
        {
            InitializeComponent();
            this.sala = sala;
            this.cartelera = cartelera;
            comprados = new List<Asiento>();
            reservados = new List<Asiento>();



        }

        public Salas_cine(Sala sala)
        {
            InitializeComponent();
            this.sala = sala;
            comprados = new List<Asiento>();
            reservados = new List<Asiento>();



        }
        public Salas_cine(Sala sala,MainWindow cartelera,int nFilas, int nColumnas)
        {

            InitializeComponent();
            this.sala = sala;
            this.cartelera = cartelera;
            comprados = new List<Asiento>();
            reservados = new List<Asiento>();


            for (int f = 0; f < nFilas; f++)
                Butacas.RowDefinitions.Add(new RowDefinition());
            for (int c = 0; c < nColumnas; c++)
                Butacas.ColumnDefinitions.Add(new ColumnDefinition());


            for (int fila = 0; fila < nFilas; fila++)
                for (int columna = 0; columna < nColumnas; columna++)
                {

                    System.Windows.Controls.Button b = new System.Windows.Controls.Button();
                    b.Name = "boton" + fila + columna;

                    b.Click += butaca_Click;
                    b.Loaded += butaca1_Loaded;
                    b.Click += butaca1_MouseRightButtonDown;

                    //Crea un imagen
                    Image butaca = new Image();
                    //le meto el codigo de la imagen con el bitmapimage
                    butaca.Source = new BitmapImage(new Uri("Imagenes/armchair2.png", UriKind.Relative));
                    b.Background = Brushes.LightGreen;
                    //creo un nuevo grid
                    Grid grid = new Grid();
                    //meto el grid creado en el contexto del boton
                    b.Content = grid;
                    b.BorderThickness = new Thickness(0, 0, 0, 0);
                    Butacas.Children.Add(b);
                    Grid.SetRow(b, fila);
                    Grid.SetColumn(b, columna);
                    //añado la imagen al hijo del grid
                    grid.Children.Add(butaca);

                }
        }

        private void butaca_Click(object sender, RoutedEventArgs e)
        {

            //guardar posiciones grid
            var element = (UIElement)e.Source;

            int x = Grid.GetColumn(element);
            int y = Grid.GetRow(element);
            //guardar posiciones grid

            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            int cont = 0;


            if (boton == true)
            {

                if (sala.asientos.Count == 0)
                {


                    comprados.Add(new Asiento(y, x, "ocupado"));
                    boton = false;

                }
                else
                {

                    cont = 0;
                    foreach (Asiento a in sala.asientos)
                    {

                        if (a.columna == x && a.fila == y && a.estado.Equals("libre"))
                        {
                            
                            comprados.Add(new Asiento(y, x, "ocupado"));

                            
                            button.Background = Brushes.IndianRed;

                        }
                        else if (cont == 0 && a.columna == x && a.fila == y && a.estado.Equals("ocupado"))
                        {
                            cont++;
                            System.Windows.MessageBox.Show("Este asiento ya esta ocupado, elige otro");
                        }



                    }


                }


                




            }

        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {

            //hago una especie de query para cambiar los ocupados y reservados a libre
            sala.asientos.Where(w => w.estado == "reservado" || w.estado == "ocupado").ToList().ForEach(s => s.estado = "libre");

            this.Hide();

            cartelera.Show();



        }

        private void comprar_Click(object sender, RoutedEventArgs e)
        {
            sala.asientos.AddRange(comprados);
            
            foreach (Asiento a in sala.asientos)
            {
                if (a.estado.Equals("ocupado"))
                {

                    a.toString();

                }


            }
            this.Hide();

            Form1 report = new Form1(comprados,reservados,sala);

            report.Show();



        }

        private void reservar_Click(object sender, RoutedEventArgs e)
        {
            sala.asientos.AddRange(comprados);
            //comprados.Clear();
            foreach (Asiento a in comprados)
            {
                if (a.estado.Equals("reservado"))
                {

                    a.toString();

                }


            }
            this.Hide();

            Form1 report = new Form1(comprados, reservados, sala);

            report.Show();


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

           
        }

        private void butaca1_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Controls.Button boton = sender as System.Windows.Controls.Button;
            //guardar posiciones grid

            var element = (UIElement)e.Source;

            int x = Grid.GetColumn(boton);
            int y = Grid.GetRow(boton);
            //guardar posiciones grid

            //para que guarde los asientos generados automaticamente en la lista 
            //filas por columnas del grid creado automaticamente
            int butacas = Butacas.RowDefinitions.Count * Butacas.ColumnDefinitions.Count;

            //si el numero de hijos del grid es igual a las columnas por filas && si el numero de asientos es menor que el numero de butacas mas el numero de los asientos comprados(esto es por que no paraba de insertarme datos cada vez de inicio)
            if (Butacas.Children.Count== butacas && sala.asientos.Count<( butacas + comprados.Count))
            {

                //añade el asiento a la lista
                sala.asientos.Add(new Asiento(y, x, "libre"));

            }

            foreach (Asiento a in sala.asientos)
            {
                if (a.columna == x && a.fila == y && a.estado.Equals("ocupado"))
                {
                    boton.Background = Brushes.IndianRed;
                }
                else if (a.columna == x && a.fila == y && a.estado.Equals("reservado"))
                {
                    boton.Background = Brushes.Purple;
                }

            }
        }

        private void butaca1_MouseRightButtonDown(object sender, RoutedEventArgs e)
        {

            /*
            //guardar posiciones grid
            var element = (UIElement)e.Source;

            int x = Grid.GetColumn(element);
            int y = Grid.GetRow(element);
            //guardar posiciones grid

            Button button = sender as Button;
            int cont = 0;

            if (boton == true)
            {

                if (sala.asientos.Count == 0)
                {


                    comprados.Add(new Asiento(y, x, "reservado"));
                    boton = false;
                }
                else
                {

                    cont = 0;
                    foreach (Asiento a in sala.asientos)
                    {

                        if (a.columna == x && a.fila == y && a.estado.Equals("libre"))
                        {
                            //true = ocupado
                            //comprados.Remove(a);
                            comprados.Add(new Asiento(y, x, "reservado"));

                            button.Background = Brushes.Purple;

                        }
                        else if (cont == 0 && a.columna == x && a.fila == y && a.estado.Equals("ocupado"))
                        {
                            cont++;
                            MessageBox.Show("Este asiento ya esta ocupado, elige otro");
                        }



                    }


                }







            }*/
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

            comprados.Clear();
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

            
          

                if (e.Key == Key.F1)

                {

                Help.ShowHelp(null, @"C:\Users\Ignacio\source\repos\Practica4\Resources\bicho.chm");


                }

            
        }
    }





    



}