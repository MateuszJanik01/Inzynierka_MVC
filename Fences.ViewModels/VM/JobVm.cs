﻿using Fences.Model.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fences.ViewModels.VM
{
    public class JobVm
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserPhoneNumber { get; set; } = null!;
        public string Town { get; set; } = null!;
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string ZipCode { get; set; } = null!;
        public double TotalLength { get; set; }
        public double Height { get; set; }
        public string JobType { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime DateOfExecution { get; set; }
        public double TotalPrice => Math.Round((TotalLength / 2.15) * Height * 150);
    }
}
