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
    public partial class Menu : ContentPage
    {
        public Menu()
        {
            InitializeComponent();
        }

        private async void Button_Cursos(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new Cursos());
        }
        private async void Button_Empleados(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Empleados());
        }

        private async void Button_seguimiento(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Seguimiento());
        }
    }
}