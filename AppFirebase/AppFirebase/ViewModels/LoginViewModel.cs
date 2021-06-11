using AppFirebase.Services;
using AppFirebase.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppFirebase.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        //LOGIN
        string _username;
        public string Username
        {
            get { return this._username; }
            set
            {
                this._username = value;
                OnPropertyChanged();
            }
        }

        string _password;
        public string Password {
            get { return this._password; }
            set
            {
                this._password = value;
                OnPropertyChanged();
            } 
        }



        //SUCESSO OU NAO (BOOL)
        bool _resultado;
        public bool Resultado
        {
            get { return this._IsBusy; }
            set
            {
                this._IsBusy = value;
                OnPropertyChanged();
            }
        }


        //VERIFICA SE O LOGIN ESTA SENDO REALIZADO PARA EVITAR CONCORRENCIA.
        bool _IsBusy;
        public bool IsBusy
        {
            get { return this._resultado; }
            set
            {
                this._resultado = value;
                OnPropertyChanged();
            }
        }



        //COMMANDS BOTOES LOGIN E REGISTRO
        public Command LoginCommand { get; set; }

        //public ICommand LoginCommand = new Command(async () => await LoginCommadAsync());

        public Command RegistroCommand { get; set; }

        public LoginViewModel()
        {            
            RegistroCommand = new Command(async () => await RegistroCommandAsync());
            LoginCommand = new Command(async () => await LoginCommandAsync());
        }
      
           

        public async Task RegistroCommandAsync()
        {
            if (IsBusy)//VERFICA SE O USUARIO JA ESTA REGISTRADO, SE SIM ENTAO RETORNA.
                return;
            try
            {
                IsBusy = true;

                //INSTANCIA DA CLASSE QUE VAI ACESSAR O FIREBASE
                var usuarioService = new UsuarioService();

                //SE RETORNAR TRUE ENTAO SUCESSO.
                Resultado = await usuarioService.RegistrarUsuario(Username, Password);
                if (Resultado)
                {
                    await App.Current.MainPage.DisplayAlert("Sucesso", "Usuario Cadastrado.", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Falha", "Falha ao registrar usuario.", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Falha ao realizar login. Descrição: "+ ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoginCommandAsync()
        {
            if (IsBusy)//VERFICA SE O USUARIO JA ESTA LOGADO.
                return;
            try
            {
                IsBusy = true;

                //INSTANCIA DA CLASSE QUE VAI ACESSAR O FIREBASE
                var usuarioService = new UsuarioService();

                //SE RETORNAR TRUE ENTAO SUCESSO E VAI PRA NOVA VIEW
                Resultado = await usuarioService.Logar(Username, Password);
                if (Resultado)
                {
                    //PREFERENCES OQUE É:(permite armazenar pares de chave-valor(key-value) de tipos de dados simples
                    // em um arquivo de preferencias compartilhadas.)
                    Preferences.Set("Username", Username);
                    await App.Current.MainPage.Navigation.PushAsync(new LoginView());
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Falha", "Usuario invalido!.", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Falha ao realizar login. Descrição: " + ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
       
    }
}
