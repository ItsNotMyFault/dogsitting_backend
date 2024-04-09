using dogsitting_backend.Domain;
using FirebaseAdmin;
using FireSharp.Config;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace dogsitting_backend.Infrastructure
{
    public class FirebaseRepository : RestApiRepository
    {
        public string? AccessToken { get; set; }
        public string? Url { get; set; }

        public FirebaseRepository()
        {
            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.GetApplicationDefault(),
            //    //Credential = GoogleCredential.FromFile("path/to/refreshToken.json"),
            //    ProjectId = "dogsitting-1d358",
            //    ServiceAccountId = "1:307695480234:web:18bf63023e4189be6ccfd3",

            //});

            //IFirebaseConfig test = new FirebaseConfig
            //{
            //    AuthSecret = "",
            //    BasePath = "https://dogsitting-1d358-default-rtdb.firebaseio.com/",
            //};


        }

        public string GetTest()
        {



            return "Test";
        }


    }
}




