using AppCursosB.model;
using AppRegistrosB.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppRegistrosB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Seguimiento : ContentPage
    {
        static int id = 0;
        Dictionary<Button, bool> botones = new Dictionary<Button, bool>();
        Dictionary<string, Picker> campos = new Dictionary<string, Picker>();
        public Seguimiento()
        {
            InitializeComponent();
            llenarDatosCursosYEmpleados();
            llenarDatos();
            //Añadir elementos para estatus
            pickStatus.Items.Add("Programado");
            pickStatus.Items.Add("En curso");
            pickStatus.Items.Add("Completo");
            //Añadir elementos para calificación
            pickCalificacion.Items.Add("1");
            pickCalificacion.Items.Add("2");
            pickCalificacion.Items.Add("3");
            pickCalificacion.Items.Add("4");
            pickCalificacion.Items.Add("5");
            pickCalificacion.Items.Add("6");
            pickCalificacion.Items.Add("7");
            pickCalificacion.Items.Add("8");
            pickCalificacion.Items.Add("9");
            pickCalificacion.Items.Add("10");
            //Campos
            campos.Add("IdEmpl",pickIdEmpl);
            campos.Add("Empleado", pickNomEmpl);
            campos.Add("curso", pickNomCurso);
            campos.Add("estatus", pickStatus);
            campos.Add("calif", pickCalificacion);
            //Botones
            botones.Add(btnEliminar, btnEliminar.IsVisible);
            botones.Add(btnEditar, btnEditar.IsVisible);
            botones.Add(btnActualizar, btnActualizar.IsVisible);
            botones.Add(btnCancelar, btnCancelar.IsVisible);
   
        }
        public async void llenarDatosCursosYEmpleados()
        {
            var EmpleadoList = await App.SQLiteDB.GetEmpleadosAsync();
            var CursoList = await App.SQLiteDBc.GetCursosAsync();
            List<string> idEmpl = new List<string>();
            List<string> nombreEmpl = new List<string>();
            List<string> nombreCurso = new List<string>();
            if (EmpleadoList != null && CursoList != null)
            {
                foreach (var item in EmpleadoList)
                {
                    idEmpl.Add(item.IdEmp.ToString());
                    nombreEmpl.Add(item.Nombre);
                }
                foreach (var item in CursoList)
                {
                    nombreCurso.Add(item.Nombre);
                }
                pickIdEmpl.ItemsSource = idEmpl;
                pickNomEmpl.ItemsSource = nombreEmpl;
                pickNomCurso.ItemsSource = nombreCurso;
            }

        }
        private void AutocompletarNombre(object sender, EventArgs e)
        {
            pickNomEmpl.SelectedIndex = pickIdEmpl.SelectedIndex;
        }
        private void AutocompletarID(object sender, EventArgs e)
        {
            pickIdEmpl.SelectedIndex = pickNomEmpl.SelectedIndex;
        }

        private void Button_Crear_Clicked(object sender, EventArgs e)
        {
            btnCrear.IsEnabled = false;
            labelAccion.Text = "Asignar curso a empleado";
            foreach (var item in campos)
            {
                item.Value.IsEnabled = true;
                item.Value.SelectedIndex = -1;
            }
            foreach (var item in botones)
            {
                item.Key.IsVisible = false;
            }
            panelEdit.IsVisible = true;
            btnGuardar.IsVisible = true;
            pickFecha.IsEnabled = true;
            pickHora.IsEnabled = true;
            IdRegistro.IsVisible = false;
        }

        private async void Button_Guardar_Clicked(object sender, EventArgs e)
        {
            foreach (var item in campos)
            {
                if (item.Value.SelectedIndex==-1)
                {
                    await DisplayAlert("AVISO", "Seleccionar todos los datos", "OK");
                    return;
                }
            }
            SeguimientoModel emple = new SeguimientoModel()
            {
                IdEmple = int.Parse(pickIdEmpl.SelectedItem.ToString()),
                NombreEmple = pickNomEmpl.SelectedItem.ToString(),
                NombreCurso = pickNomCurso.SelectedItem.ToString(),
                Fecha = pickFecha.Date.ToString("yyyy-MM-dd"),
                Hora = pickHora.Time.ToString(),
                Estatus = pickStatus.SelectedItem.ToString(),
                Calificacion = pickCalificacion.SelectedItem.ToString(), 
            };
            await App.SQLiteDBs.SaveSeguimientoAsync(emple);
            foreach (var item in botones)
            {
                item.Key.IsVisible = item.Value;
            }
            foreach (var item in campos)
            {
                if (item.Key == "IdRegistro")
                {
                    item.Value.IsVisible = true;
                    continue;
                }
                item.Value.SelectedIndex = -1;
                item.Value.IsEnabled = false;
            }
            btnCrear.IsEnabled = true;
            panelEdit.IsVisible = false;
            btnGuardar.IsVisible = false;
            await DisplayAlert("AVISO", "Guardado de forma exitosa", "OK");
            llenarDatos();
            pickFecha.IsEnabled = false;
            pickHora.IsEnabled = false;
            IdRegistro.IsVisible = true;
        }
        public async void llenarDatos()
        {
            var Empleado = await App.SQLiteDBs.GetSeguimientosAsync();
   
            if (Empleado != null)
            {
                lsSeguimiento.ItemsSource = Empleado;
            }
        }
        private void Button_Cerrar_clicked(object sender, EventArgs e)
        {
            panelEdit.IsVisible = false;
            btnCrear.IsEnabled = true;
            btnGuardar.IsVisible = false;
            pickFecha.IsEnabled = false;
            pickHora.IsEnabled = false;
            foreach (var item in campos)
            {
                if (item.Key == "IdRegistro")
                {
                    item.Value.IsVisible = true;
                    continue;
                }
                item.Value.IsEnabled = false;
            }
            foreach (var item in botones)
            {
                item.Key.IsVisible = item.Value;
            }
        }
        private async void Button_Actualizar_Clicked(object sender, EventArgs e)
        {
            labelAccion.Text = "Actualizar Empelado";
            foreach (var item in campos)
            {
                if (item.Key == "IdRegistro") continue;
                if (item.Value.SelectedIndex == -1)
                {
                    await DisplayAlert("AVISO", "Seleccionar todos los datos", "OK");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(IdRegistro.Text.ToString()))
            {
                SeguimientoModel emple = new SeguimientoModel()
                {
                    IdRegistro = int.Parse(IdRegistro.Text.ToString()),
                    IdEmple = int.Parse(pickIdEmpl.SelectedItem.ToString()),
                    NombreEmple = pickNomEmpl.SelectedItem.ToString(),
                    NombreCurso = pickNomCurso.SelectedItem.ToString(),
                    Fecha = pickFecha.Date.ToString("yyyy-MM-dd"),
                    Hora = pickHora.Time.ToString(),
                    Estatus = pickStatus.SelectedItem.ToString(),
                    Calificacion = pickCalificacion.SelectedItem.ToString(),
                };
                await App.SQLiteDBs.SaveSeguimientoAsync(emple);
                foreach (var item in campos)
                {
                    if (item.Key == "ID") continue;
                    item.Value.IsEnabled = false;
                }
                foreach (var item in botones)
                {
                    item.Key.IsVisible = item.Value;
                }
                pickFecha.IsEnabled = false;
                pickHora.IsEnabled = false;
                await DisplayAlert("AVISO", "Se actualizo registro de forma existosa", "Ok");
                llenarDatos();
            }
        }
        private async void Button_Cancelar_clicked(object sender, EventArgs e)
        {
            foreach (var item in campos)
            {
                if (item.Key == "IdRegistro") continue;
                item.Value.IsEnabled = false;
            }
            foreach (var item in botones)
            {
                item.Key.IsVisible = item.Value;
            }
            var emple = await App.SQLiteDBs.GetSeguimientoByIdAsync(int.Parse(IdRegistro.Text.ToString()));
            if (emple != null)
            {
                IdRegistro.Text = emple.IdRegistro.ToString();
                pickIdEmpl.SelectedItem = emple.IdEmple;
                pickNomEmpl.SelectedItem = emple.NombreEmple;
                pickNomCurso.SelectedItem = emple.NombreCurso;  
                pickFecha.Date = DateTime.Parse(emple.Fecha);
                pickHora.Time = TimeSpan.Parse(emple.Hora);
                pickStatus.SelectedItem = emple.Estatus;
                pickCalificacion.SelectedItem = emple.Calificacion;
                labelAccion.Text = "Empleado";
                btnCrear.IsEnabled = false;
            }
            labelAccion.Text = "Empleado";
            pickFecha.IsEnabled = false;
            pickHora.IsEnabled = false;
        }

 

        private void Button_Editar_clicked(object sender, EventArgs e)
        {
            foreach (var item in botones)
            {
                item.Key.IsVisible = !item.Value;
            }
            foreach (var item in campos)
            {
                if (item.Key == "IdRegistro") continue;
                item.Value.IsEnabled = true;
            }
            pickFecha.IsEnabled = true;
            pickHora.IsEnabled = true;
        }

        private async void Button_Eliminar_Clicked(object sender, EventArgs e)
        {
            var empleado = await App.SQLiteDBs.GetSeguimientoByIdAsync(int.Parse(IdRegistro.Text.ToString()));
            if (empleado != null)
            {
                await App.SQLiteDBs.DeleteRegistroAsync(empleado);
                await DisplayAlert("AVISO", "Se elimino el registro de forma existosa", "Ok");
                foreach (var item in botones)
                {
                    item.Key.IsVisible = item.Value;
                }
                foreach (var item in campos)
                {
                    if (item.Key == "IdRegistro")
                    {
                        item.Value.IsVisible = true;
                        continue;
                    }
                    item.Value.SelectedIndex = -1;
                    item.Value.IsEnabled = false;
                }
                btnCrear.IsEnabled = true;
                panelEdit.IsVisible = false;
                pickFecha.IsEnabled = false;
                pickHora.IsEnabled = false;
                llenarDatos();
            }
        }


        private async void lstCursos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var obj = (SeguimientoModel) e.SelectedItem;
            if (!string.IsNullOrEmpty(obj.IdRegistro.ToString()))
            { 
                var emple = await App.SQLiteDBs.GetSeguimientoByIdAsync(obj.IdRegistro);
                if (emple != null)
                {
                    IdRegistro.Text= emple.IdRegistro.ToString();
                    pickIdEmpl.SelectedItem = emple.IdEmple;
                    pickNomEmpl.SelectedItem = emple.NombreEmple;
                    pickNomCurso.SelectedItem = emple.NombreCurso;
                    pickFecha.Date = DateTime.Parse(emple.Fecha);
                    pickHora.Time = TimeSpan.Parse(emple.Hora);
                    pickStatus.SelectedItem = emple.Estatus;
                    pickCalificacion.SelectedItem = emple.Calificacion;
                    labelAccion.Text = "Empleado";
                    btnCrear.IsEnabled = false;
                    panelEdit.IsVisible = true;
                }
    
            }
        }
    }
}