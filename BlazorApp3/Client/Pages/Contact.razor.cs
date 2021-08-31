using BlazorApp3.Shared;
using Google.Cloud.Firestore;

namespace BlazorApp3.Client.Pages
{
    public partial class Counter
    {
        public int currentCount;
        protected AccountManagementModel acc = new();
        protected string mess = null;
        protected async Task IncrementCount()
        {
            currentCount++;
            try
            {
                FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");
                Query usersRef = db.Collection("Account").WhereEqualTo("Id", "woD7khzuOvVmaRkPv2gnvSNMdOc2");
                QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    acc = document.ConvertTo<AccountManagementModel>();
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
            }
        }
    }
}