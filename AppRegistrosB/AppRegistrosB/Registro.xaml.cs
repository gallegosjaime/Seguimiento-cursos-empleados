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
    public partial class Registro : ContentPage
    {
        public Registro()
        {
            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmailReg.Text))
            {
                await DisplayAlert("AVISO", "Debe escribir un email en el campo", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(txtContraReg.Text))
            {
                await DisplayAlert("AVISO", "Debe escribir una contraseña en el campo", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(txtNombreReg.Text))
            {
                await DisplayAlert("AVISO", "Debe escribir el Nombre Completo en el campo", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(txtEdadReg.Text))
            {
                await DisplayAlert("AVISO", "Debe escribir la edad en el campo", "Ok");
                return;
            }

            UsersModel usr = new UsersModel()
            {
                Email = txtEmailReg.Text,
                Emailpassword = txtContraReg.Text,
                NombreCompleto = txtNombreReg.Text,
                Edad = int.Parse(txtEdadReg.Text),
                FechaCreacion = DateTime.Now.ToString(),
            };

            await App.SQLiteDBlogin.SaveUserModelAsync(usr);
            await DisplayAlert("AVISO", "El registro de usuario se guardo con exito", "Ok");

            txtEmailReg.Text = "";
            txtContraReg.Text = "";
            txtNombreReg.Text = "";
            txtEdadReg.Text = "";

            await Navigation.PopAsync();
        }

    }
}