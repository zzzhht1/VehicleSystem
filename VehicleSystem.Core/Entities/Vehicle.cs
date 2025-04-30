using System;
using System.ComponentModel.DataAnnotations;
using VehicleSystem.Core.Enums;

namespace VehicleSystem.Core.Entities;

public class Vehicle
{
    public int Id { get; set; }
    [Required, MaxLength(20)]
    public string PlateNumber { get; set; }
    public VehicleStatus Status { get; set; }
    public bool IsDeleted { get; set; }
}