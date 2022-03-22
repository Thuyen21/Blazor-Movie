using BlazorMovie.Server.Options;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.Extensions.Options;
using System.Text;

namespace BlazorMovie.Server.Services
{
    public class FileService
    {
        private readonly FirebaseAuthProvider authProvider;
        private readonly string bucket;
        private readonly string email;
        private readonly string password;
        public FileService(string apiKey, string bucket, string email, string password)
        {
            this.authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            this.bucket = bucket;
            this.email = email;
            this.password = password;
        }
        public async Task<string> UploadAsync(Stream stream, string filename)
        {
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            var task = new FirebaseStorage(bucket,
     new FirebaseStorageOptions
     {
         AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken),
         ThrowOnCancel = true,
     })
    .Child(filename)
    .PutAsync(stream);
            var url = await task;
            return url;
        }
    }
}
