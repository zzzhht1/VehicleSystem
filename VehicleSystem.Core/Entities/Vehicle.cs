using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleSystem.Core.Enums;

namespace VehicleSystem.Core.Entities;

[Table("VehicleInfo")] // 对应数据库表名
public class Vehicle
{
    [Key]
    [Required]
    [MaxLength(20)]
    [Column("plateNumber")]
    [Display(Name = "车牌号")]
    public string PlateNumber { get; set; }  // 主键

    [Required]
    [MaxLength(20)]
    [Display(Name = "车辆类型")]
    public string Type { get; set; }

    [Required]
    [MaxLength(30)]
    [Display(Name = "品牌")]
    public string Brand { get; set; }

    [MaxLength(20)]
    [Display(Name = "颜色")]
    public string Color { get; set; } = "白色";  // 默认值

    [Required]
    [MaxLength(10)]
    [Display(Name = "燃油类型")]
    public string FuelType { get; set; }

    [Range(1, int.MaxValue)]
    [Display(Name = "座位数")]
    public int SeatCapacity { get; set; }

    [Display(Name = "里程数")]
    public int Mileage { get; set; } = 0;  // 默认值

    [MaxLength(10)]
    [Display(Name = "状态")]
    public string Status { get; set; } = "可用";  // 默认值

    [MaxLength(20)]
    [Display(Name = "车主ID")]
    public string OwnerId { get; set; }

    [Display(Name = "是否删除")]
    public bool IsDeleted { get; set; }
}