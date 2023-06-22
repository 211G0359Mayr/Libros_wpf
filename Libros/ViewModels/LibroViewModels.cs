using Libros.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Libros.ViewModels
{
    public class LibroViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        //    Disculpe maestra Ernestina, la tardanza de este código es debido a que se me descompuso el regulador de la 
        //    computadora desde el sábado, y no he estado usando la computadora más de 2 horas debido por el temor de
        //    que se me calentara demás o afectara al cargador ya que no haya nada de lo que regule, pido solamente 
        //    un poco más de tiempo es probable que lo tenga terminado para la noche, debido a que me faltan unos ligeros
        //    detalles, se supone que lo debía de haber terminado desde el día de ayer pero el tiempo no me alcanzo,
        //    espero y comprenda mi situación, y me dé la oportunidad de entregarlo un poco más tarde, además de que 
        //    estoy trabajando por las tardes así que mis tiempos libres para avanzar son pocos.

        int Indice;

        public string Error { get; set; } = "";
        public ObservableCollection<Libro>? Libros { get; set; } = new ObservableCollection<Libro>();
        public Libro Libro { get; set; } = new Libro();
        public string Vista { get; set; } = "ver";
        public ICommand? AgregarCommand { get; set; }
        public ICommand? EditarCommand { get; set; }
        public ICommand? EliminarCommand { get; set; }
        public ICommand? CambiarVistaCommand { get; set; }
        public ICommand? CancelarCommand { get; set; }
        public void Agregar()
        {
            Error = "";
            if (string.IsNullOrEmpty(Libro.TituloOriginal))
            {
                Error += "Ingrese el titulo autentico";
                MessageBox.Show("Ingrese el titulo autentico");
                return;
            }
            if (string.IsNullOrEmpty(Libro.Titulo))
            {
                Error += "Ingrese el titulo";
                MessageBox.Show("Ingrese el titulo");
                return;
            }
            if (string.IsNullOrEmpty(Libro.Genero))
            {
                Error += "Ingrese el genero correspondiente";
                MessageBox.Show("Ingrese el genero");
                return;
            }
            if (string.IsNullOrEmpty(Libro.Reseña))
            {
                Error += "Ingrese la reseña";
                MessageBox.Show("Ingrese la reseña");
                return;
            }
            if (string.IsNullOrEmpty(Libro.Autor))
            {
                Error += "Ingrese el nombre del director";
                MessageBox.Show("Ingrese el nombre del director");
                return;
            }
            if (Libros != null)
            {
                Libros.Add(Libro);
                Guardar();
                CambiarVista("ver");
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public void Eliminar()
        {
            if (Libro != null && Libros != null)
            {
                Libros.Remove(Libro);
                CambiarVista("ver");
                Guardar();
            }

        }

        public void Editar()
        {
            if (Libro != null && Libros != null)
            {
                Libros[Indice] = Libro;
                Guardar();
                CambiarVista("ver");
            }
        }

        public void CambiarVista(string vistaACambiar)
        {
            Vista = vistaACambiar;
            if (vistaACambiar == "agregar")
            {
                Libro = new Libro();
            }
            if (vistaACambiar == "editar")
            {
                if (Libros != null)
                {
                    Indice = Libros.IndexOf(Libro);
                }
                Libro CopiaL = new Libro()
                {
                    Titulo = Libro.Titulo,
                    TituloOriginal = Libro.TituloOriginal,
                    Genero = Libro.Genero,
                    Autor = Libro.Autor,
                    Reseña = Libro.Reseña,
                };
                Libro = CopiaL;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vista"));

        }

        public void Cancelar()
        {
            Vista = "ver";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));
        }

        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(Libros);
            File.WriteAllText("ProLibros.json", json);
        }

        public void Cargar()
        {
            if (File.Exists("ProLibros.Json"))
            {
                var json = File.ReadAllText("ProLibros.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<Libro>>(json);
                if (datos != null)
                {
                    Libros = (ObservableCollection<Libro>)datos;

                }
                else
                {
                    Libros = new ObservableCollection<Libro>();
                }
            }
        }

        public LibroViewModels()
        {
            Cargar();
            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(Editar);
            EliminarCommand = new RelayCommand(Eliminar);
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
            CancelarCommand = new RelayCommand(Cancelar);
        }
    }
}
