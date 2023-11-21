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
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }
        private async void Button_Login(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmailLog.Text))
            {
                await DisplayAlert("AVISO", "Debe escribir el email", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(txtContraLog.Text))
            {
                await DisplayAlert("AVISO", "Debe escribir la contraseña", "Ok");
                return;
            }
            var resultado = await App.SQLiteDBlogin.GetUsersValidate(txtEmailLog.Text, txtContraLog.Text);

            if (resultado != null)
            {
                txtEmailLog.Text = "";
                txtContraLog.Text = "";
                await Navigation.PushAsync(new Menu());
            }
            else
            {
                txtEmailLog.Text = "";
                txtContraLog.Text = "";
                await DisplayAlert("AVISO", "El email o la contraseña estan incorrectos", "Ok");
                return;
            }

        }

        private async void Button_Registrarse(object sender, EventArgs e)
        {
            txtEmailLog.Text = "";
            txtContraLog.Text = "";

            await Navigation.PushAsync(new Registro());
        }

    }
}