// 定义枚举

namespace VehicleSystem.Core.Enums;

// 车辆状态枚举
public enum VehicleStatus
{
    在库 = 0,   // 数据库存 0
    已出租 = 1, // 数据库存 1
    维修中 = 2, // 数据库存 2
    已报废 = 3  // 数据库存 3
}

// 删除结果枚举
public enum SoftDeleteResult
{
    Success,
    NotFound,
    AlreadyDeleted,
    Error
}