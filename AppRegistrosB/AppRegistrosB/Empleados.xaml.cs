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
    public partial class Empleados : ContentPage
    {
        Dictionary<Button, bool> botones = new Dictionary<Button, bool>();
        Dictionary<string, Entry> campos = new Dictionary<string, Entry>();

        public Empleados()
        {
            InitializeComponent();
            llenarDatos();
            //Picker
            txtTipo.Items.Add("Planta");
            txtTipo.Items.Add("Temporal");
            //Botones
            botones.Add(btnEliminar, btnEliminar.IsVisible);
            botones.Add(btnEditar, btnEditar.IsVisible);
            botones.Add(btnActualizar, btnActualizar.IsVisible);
            botones.Add(btnCancelar, btnCancelar.IsVisible);
            //Campos
            campos.Add("ID", txtIdEmp);
            campos.Add("Nombre", txtNombre);
            campos.Add("Direccion", txtDireccion);
            campos.Add("Telefono", txtTelefono);
            campos.Add("Edad", txtEdad);
            campos.Add("CURP", txtCurp);
        }

        private void Button_Crear_Clicked(object sender, EventArgs e)
        {
            btnCrear.IsEnabled = false;
            labelAccion.Text = "Agregar Empleado";
            txtTipo.IsEnabled = true;
            foreach (var item in campos)
            {
                if (item.Key == "ID") continue;
                item.Value.Text = "";
                item.Value.IsEnabled = true;
            }
            foreach (var item in botones)
            {
                item.Key.IsVisible = false;
            }
            panelEdit.IsVisible = true;
            campos["ID"].IsVisible = false;
            btnGuardar.IsVisible = true;

        }
        private async void Button_Guardar_Clicked(object sender, EventArgs e)
        {
            foreach (var item in campos)
            {
                if (item.Key == "ID") continue;
                if (String.IsNullOrEmpty(item.Value.Text)){
                    await DisplayAlert("AVISO", "Ingresar todos los datos", "OK");
                    return;
                }
            }
            Empleado emple = new Empleado()
            {
                Nombre = txtNombre.Text,
                Direccion = txtDireccion.Text,
                Curp = txtCurp.Text,
                Edad = int.Parse(txtEdad.Text),
                Telefono = double.Parse(txtTelefono.Text),
                TipoEmpleado = txtTipo.SelectedItem.ToString(),
            };
            await App.SQLiteDB.SaveEmpleadosAsync(emple);
            foreach (var item in botones) { 
                item.Key.IsVisible = item.Value;
            }
            foreach (var item in campos)
            {
                if (item.Key == "ID"){
                    item.Value.IsVisible = true;
                    continue;
                }
                item.Value.Text = "";
                item.Value.IsEnabled = false;
            }
            txtTipo.IsEnabled = false;
            btnCrear.IsEnabled = true;
            panelEdit.IsVisible = false;
            btnGuardar.IsVisible = false;
            await DisplayAlert("AVISO", "Guardado de forma exitosa", "OK");
            llenarDatos();
        }
        private async void Button_Editar_clicked(object sender, EventArgs e)
        {
            foreach (var item in botones)
            {
                item.Key.IsVisible = !item.Value;
            }
            foreach (var item in campos)
            {
                if (item.Key == "ID") continue;
                item.Value.IsEnabled = true;
            }
            txtTipo.IsEnabled = true;
        }
        private async void Button_Cancelar_clicked(object sender, EventArgs e)
        {
            foreach (var item in campos)
            {
                if (item.Key == "ID") continue;
                item.Value.IsEnabled = false;
            }
            foreach (var item in botones)
            {
                item.Key.IsVisible = item.Value;
            }
            var emple = await App.SQLiteDB.GetEmpleadoByIdAsync(int.Parse(txtIdEmp.Text));
            if (emple != null)
            {
                txtIdEmp.Text = emple.IdEmp.ToString();
                txtNombre.Text = emple.Nombre;
                txtDireccion.Text = emple.Direccion;
                txtCurp.Text = emple.Curp;
                txtTipo.SelectedItem = emple.TipoEmpleado;
                txtEdad.Text = emple.Edad.ToString();
                txtTelefono.Text = emple.Telefono.ToString();
            }
            labelAccion.Text = "Empleado";
            txtTipo.IsEnabled = false;
        }

        private void Button_Cerrar_clicked(object sender, EventArgs e)
        {
            panelEdit.IsVisible = false;
            btnCrear.IsEnabled = true;
            btnGuardar.IsVisible = false;
            foreach (var item in campos)
            {
                if (item.Key == "ID"){
                    item.Value.IsVisible = true;
                    continue;
                }
                item.Value.IsEnabled= false;
            }
            foreach (var item in botones)
            {
                item.Key.IsVisible = item.Value;
            }
        }

        private async void Button_Actualizar_Clicked(object sender, EventArgs e)
        {
            labelAccion.Text = "Actualizar Empelado";
            if (validarDatos() == false)
            {
                await DisplayAlert("AVISO", "Ingresar todos los datos", "OK");
                return;
            }
            if (!string.IsNullOrEmpty(txtIdEmp.Text))
            {
                Empleado emple = new Empleado()
                {
                    IdEmp = int.Parse(txtIdEmp.Text),
                    Nombre = txtNombre.Text,
                    Direccion = txtDireccion.Text,
                    Curp = txtCurp.Text,
                    Edad = int.Parse(txtEdad.Text),
                    Telefono = double.Parse(txtTelefono.Text),
                    TipoEmpleado = txtTipo.SelectedItem.ToString()
                };
                Console.WriteLine(emple.ToString());
                await App.SQLiteDB.SaveEmpleadosAsync(emple);
                foreach (var item in campos)
                {
                    if (item.Key == "ID") continue;
                    item.Value.IsEnabled = false;
                }
                foreach (var item in botones)
                {
                    item.Key.IsVisible = item.Value;
                }
                txtTipo.IsEnabled = false;
                await DisplayAlert("AVISO", "Se actualizo registro de forma existosa", "Ok");
                llenarDatos();

            }

        }
        public async void llenarDatos()
        {
            var EmpleadoList = await App.SQLiteDB.GetEmpleadosAsync();
            if (EmpleadoList != null)
            {
                lsEmpleados.ItemsSource = EmpleadoList;
            }

        }

        public async void Button_Eliminar_Clicked(object sender, EventArgs e)
        {
   
            var empleado = await App.SQLiteDB.GetEmpleadoByIdAsync(int.Parse(txtIdEmp.Text));
            if (empleado != null)
            {
                await App.SQLiteDB.DeleteEmpleadoAsync(empleado);
                await DisplayAlert("AVISO", "Se elimino el registro de forma existosa", "Ok");

                foreach (var item in campos)
                {
                    item.Value.Text = "";
                }
                foreach (var item in botones)
                {
                    item.Key.IsVisible = item.Value;
                }
                btnCrear.IsEnabled = true;
                panelEdit.IsVisible = false;
                llenarDatos();
            }

        }
        private async void lstEmpleados_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var obj = (Empleado) e.SelectedItem;
            if (!string.IsNullOrEmpty(obj.IdEmp.ToString()))
            {
                var emplea = await App.SQLiteDB.GetEmpleadoByIdAsync(obj.IdEmp);
                if (emplea != null)
                {
                    txtIdEmp.Text = emplea.IdEmp.ToString();
                    txtNombre.Text = emplea.Nombre;
                    txtDireccion.Text = emplea.Direccion;
                    txtCurp.Text = emplea.Curp;
                    txtEdad.Text = emplea.Edad.ToString();
                    txtTelefono.Text = emplea.Telefono.ToString();
                    txtTipo.SelectedItem = emplea.TipoEmpleado;
                }
                panelEdit.IsVisible = true;
                btnCrear.IsEnabled = false;
                labelAccion.Text = "Empleado";
            }
        }
        public bool validarDatos()
        {
            foreach (var item in campos)
            {
                if (string.IsNullOrEmpty(item.Value.Text)) return false;
            }
            return true;
        }


    }
}
