using Firebase.Auth;
using Firebase.Storage;
using System.Text;

namespace BlazorMovie.Server.Services
{
    public class FileService
    {
        private readonly FirebaseAuthProvider authProvider;
        private readonly string bucket;
        public FileService(string apiKey, string bucket)
        {
            this.authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            this.bucket = bucket;

        }
        public async Task<string> UploadAsync(Stream stream, string filename)
        {
            var auth = await authProvider.SignInWithEmailAndPasswordAsync("thuyenminh5@gmail.com", "thuyenminh5@gmail.com");
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
