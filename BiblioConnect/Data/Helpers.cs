using Firebase.Auth;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BiblioConnect.Data
{
    public class Helpers
    {
        //Subir los archivos a Firebase 
        public async Task<string> SetImageToFirebase(Stream archivo, string imageName, string fileName)
        {
            string email = "codigo@gmail.com";
            string clave = "codigo111";
            string ruta = "tfgportalreservas.appspot.com";
            string api_key = "AIzaSyA_4kSyoW9gwGiCX3xCXnTFCmtlIkwItoA";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child(fileName)
                .Child(imageName)
                .PutAsync(archivo, cancellation.Token);


            var downloadURL = await task;
            return downloadURL;
        }

    }
}