using AppCursosB.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppRegistrosB
{
    public partial class Cursos : ContentPage
    {
        Dictionary<Button, bool> botones = new Dictionary<Button, bool>();
        Dictionary<string, Entry> campos = new Dictionary<string, Entry>();
        public Cursos()
        {
            InitializeComponent();
            llenarDatos();
            //Picker
            txtTipo.Items.Add("Interno");
            txtTipo.Items.Add("Externo");
            //Botones
            botones.Add(btnEliminar, btnEliminar.IsVisible);
            botones.Add(btnEditar, btnEditar.IsVisible);
            botones.Add(btnActualizar, btnActualizar.IsVisible);
            botones.Add(btnCancelar, btnCancelar.IsVisible);
            //Campos
            campos.Add("ID", txtIdCurso);
            campos.Add("Nombre", txtNombre);
            campos.Add("horas", txtHoras);
            campos.Add("Desc", txtDescripcion);
        }
        //ABRIR PLANEL PARA CREAR CURSO
        private void Button_Crear_Clicked(object sender, EventArgs e)
        {
            btnCrear.IsEnabled = false;
            labelAccion.Text = "Agregar Curso";
            txtTipo.IsEnabled = true;
            foreach (var item in campos){
                if (item.Key == "ID") continue;
                item.Value.Text = "";
                item.Value.IsEnabled= true;
            }
            foreach (var item in botones){
                Console.WriteLine(item.Key == btnActualizar);

                item.Key.IsVisible = false;
            }
            panelEdit.IsVisible = true;
            campos["ID"].IsVisible = false;
            btnGuardar.IsVisible = true;
        }

        // GUARDAR EN BD
        private async void Button_Guardar_Clicked(object sender, EventArgs e)
        {
            foreach (var item in campos)
            {
                if (item.Key == "ID") continue;
                if (String.IsNullOrEmpty(item.Value.Text)) {
                    await DisplayAlert("AVISO", "Ingresar todos los datos", "OK");
                    return;
                }
            }
            CursosModel curso = new CursosModel()
            {
                Nombre = txtNombre.Text,
                Tipo = txtTipo.SelectedItem.ToString(),
                Descripcion = txtDescripcion.Text,
                Horas = double.Parse(txtHoras.Text)
            };
            await App.SQLiteDBc.SaveCursosAsync(curso);
            foreach (var item in botones)
            {
                item.Key.IsVisible = item.Value;
            }
            foreach (var item in campos)
            {
                if (item.Key == "ID") continue;
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
            foreach (var item in botones){
                item.Key.IsVisible= !item.Value;
            }
            foreach (var item in campos){
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
            var curso = await App.SQLiteDBc.GetCursoByIdAsync(int.Parse(txtIdCurso.Text));
            if (curso != null)
            {
                txtIdCurso.Text = curso.IdCurso.ToString();
                txtNombre.Text = curso.Nombre;
                txtTipo.SelectedItem = curso.Tipo;
                txtDescripcion.Text = curso.Descripcion;
                txtHoras.Text = curso.Horas.ToString();
            }
            labelAccion.Text = "Curso";
            txtTipo.IsEnabled = false;
        }
        private void Button_Cerrar_clicked(object sender, EventArgs e)
        {
            panelEdit.IsVisible = false;
            btnCrear.IsEnabled = true;
            btnGuardar.IsVisible = false;
            foreach (var item in campos)
            {
                if (item.Key == "ID")
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
            labelAccion.Text = "Actualizar Curso";
            if (validarDatos() == false)
            {
                await DisplayAlert("AVISO", "Ingresar todos los datos", "OK");
                return;
            }
            if (!string.IsNullOrEmpty(txtIdCurso.Text))
            {
                CursosModel Cursos = new CursosModel()
                {
                    IdCurso = int.Parse(txtIdCurso.Text),
                    Nombre = txtNombre.Text,
                    Tipo = txtTipo.SelectedItem.ToString(),
                    Descripcion = txtDescripcion.Text,
                    Horas = double.Parse(txtHoras.Text)
                }; 

                await App.SQLiteDBc.SaveCursosAsync(Cursos);
                foreach (var item in campos){
                    if (item.Key == "ID") continue;
                    item.Value.IsEnabled = false;
                }
                foreach (var item in botones){
                    item.Key.IsVisible = item.Value;
                }
                txtTipo.IsEnabled = false;
                await DisplayAlert("AVISO", "Se actualizo el curso de forma existosa", "Ok");
                llenarDatos();
            }

        }
        public async void llenarDatos()
        {
            var CursosList = await App.SQLiteDBc.GetCursosAsync();
            if (CursosList != null)
            {
                lsCursos.ItemsSource = CursosList;
            }

        }

        public async void Button_Eliminar_Clicked(object sender, EventArgs e)
        {
            var Cursos = await App.SQLiteDBc.GetCursoByIdAsync(int.Parse(txtIdCurso.Text));
            if (Cursos != null)
            {
                await App.SQLiteDBc.DeleteCursoAsync(Cursos);
                await DisplayAlert("AVISO", "Se elimino el curso de forma existosa", "Ok");
                foreach (var item in campos){
                    item.Value.Text="";
                }
                foreach (var item in botones)
                {
                    item.Key.IsVisible = item.Value;
                }
                btnCrear.IsEnabled = true;
                panelEdit.IsVisible= false;
                llenarDatos();
            }

        }
        private async void lstCursos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var obj = (CursosModel) e.SelectedItem;
            if (!string.IsNullOrEmpty(obj.IdCurso.ToString()))
            {
                panelEdit.IsVisible = true;
                var curso = await App.SQLiteDBc.GetCursoByIdAsync(obj.IdCurso);
                if (curso != null)
                {
                    txtIdCurso.Text = curso.IdCurso.ToString();
                    txtNombre.Text = curso.Nombre;
                    txtTipo.SelectedItem = curso.Tipo;
                    txtDescripcion.Text = curso.Descripcion;
                    txtHoras.Text = curso.Horas.ToString();
                }
                labelAccion.Text = "Curso";
                btnCrear.IsEnabled = false;
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
