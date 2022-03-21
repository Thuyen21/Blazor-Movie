
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;

namespace BlazorMovie.Server.Services
{
    public class FileService
    {
        private static readonly string projectId = "movie2-e3c7b";
        private static readonly string bucketName = "movie2-e3c7b.appspot.com";
        private static readonly StorageClient storage = StorageClient.Create();
        private static readonly Bucket bucket = storage.CreateBucket(projectId, bucketName);

        public async Task Upload(Stream stream, string filename)
        {
            await storage.UploadObjectAsync(bucketName, filename, null, stream);
        }

        public async Task Download(Stream stream, string filename)
        {
            
        }
    }
}
