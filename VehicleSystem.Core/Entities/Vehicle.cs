using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleSystem.Core.Enums;

namespace VehicleSystem.Core.Entities;

[Table("VehicleInfo")] // ��Ӧ���ݿ����
public class Vehicle
{
    [Key]
    [Required]
    [MaxLength(20)]
    [Column("plateNumber")]
    [Display(Name = "���ƺ�")]
    public string PlateNumber { get; set; }  // ����

    [Required]
    [MaxLength(20)]
    [Display(Name = "��������")]
    public string Type { get; set; }

    [Required]
    [MaxLength(30)]
    [Display(Name = "Ʒ��")]
    public string Brand { get; set; }

    [MaxLength(20)]
    [Display(Name = "��ɫ")]
    public string Color { get; set; } = "��ɫ";  // Ĭ��ֵ

    [Required]
    [MaxLength(10)]
    [Display(Name = "ȼ������")]
    public string FuelType { get; set; }

    [Range(1, int.MaxValue)]
    [Display(Name = "��λ��")]
    public int SeatCapacity { get; set; }

    [Display(Name = "�����")]
    public int Mileage { get; set; } = 0;  // Ĭ��ֵ

    [MaxLength(10)]
    [Display(Name = "״̬")]
    public string Status { get; set; } = "����";  // Ĭ��ֵ

    [MaxLength(20)]
    [Display(Name = "����ID")]
    public string OwnerId { get; set; }

    [Display(Name = "�Ƿ�ɾ��")]
    public bool IsDeleted { get; set; }
}