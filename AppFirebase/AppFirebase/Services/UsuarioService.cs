using AppFirebase.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFirebase.Services
{

    public class UsuarioService
    {
        //INSTANCIA DO FIREBASE.
        FirebaseClient client;


        //PASSANDO A INSTANCIA DO REALTIME DATABASE DO FIREBASE CLIENT.
        public UsuarioService()
        {
            //Url de acesso ao banco de dados do realtime database
            client = new FirebaseClient("https://boraracharxamarin-default-rtdb.firebaseio.com/");
        }


        //VERIFICA SE O USUARIO EXISTE NA DATABASE ATRAVES DE UM JSON.
        public async Task<bool> UsuarioExiste(string nome)
        {
            var usuario = (await client.Child("Usuarios")
                .OnceAsync<Usuario>())
                .Where(u => u.Object.Username == nome).FirstOrDefault();

            return (usuario != null);
        }


        //REGISTRA NOVO USUARIO E VERIFICA SE EXISTE.
        public async Task<bool> RegistrarUsuario(string nome, string senha)
        {
            if (await UsuarioExiste(nome) == false)//vai no metodo acima e verifica, se retorna false entao ele registra
            {
                await client.Child("Usuarios")
                .PostAsync(new Usuario()// registrando novo usuario com postAsync
                {
                    Username = nome,
                    Password = senha,

                });
                return true;
            }
            else
            {
                return false;
            }
        }
       

        //REALIZA O LOGIN.
        public async Task<bool> Logar(string nome, string senha)
        {
            var usuario = (await client.Child("Usuarios")
                .OnceAsync<Usuario>())
                .Where(u => u.Object.Username == nome && u.Object.Password == senha);

            return (usuario != null);
        }

    }
}
