using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp3.Shared
{
    [FirestoreData]
    public class FeedbackMessageModel
    {
        [FirestoreProperty]
        [Required]
        [EmailAddress]
#pragma warning disable CS8618 // Non-nullable property 'Email' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Email { get; set; }
        [FirestoreProperty]
        [Required]
        [DataType(DataType.Text)]
#pragma warning disable CS8618 // Non-nullable property 'Email' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Name { get; set; }
        [Required]
        [FirestoreProperty]
        [DataType(DataType.Text)]
#pragma warning disable CS8618 // Non-nullable property 'Email' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Message { get; set; }


    }
}
