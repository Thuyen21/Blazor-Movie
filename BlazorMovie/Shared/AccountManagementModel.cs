﻿using Google.Cloud.Firestore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorMovie.Shared;

[FirestoreData]
public class AccountManagementModel
{
    [FirestoreProperty]
    [Key]
    [DataType(DataType.Text)]
    [DisplayName("Id")]
    public string? Id { get; set; }
    [FirestoreProperty]
    [Required]
    [EmailAddress]
    [DisplayName("Email")]

    public string? Email { get; set; }

    [FirestoreProperty]
    [Required]
    [DisplayName("Name")]
    [DataType(DataType.Text)]
    public string? Name { get; set; }

    [FirestoreProperty]
    [Required]
    [DisplayName("Date of birth")]
    [DataType(DataType.Date)]

    public DateTime DateOfBirth { get; set; }

    [FirestoreProperty]
    [Required]
    [DisplayName("Role")]
    [DataType(DataType.Text)]
    public string? Role { get; set; }

    [FirestoreProperty]
    [DisplayName("Wallet")]
    [DataType(DataType.Text)]
    public double Wallet { get; set; }

    [FirestoreProperty]
    public string? UserAgent { get; set; }
}

