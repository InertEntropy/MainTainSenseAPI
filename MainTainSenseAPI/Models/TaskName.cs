﻿namespace MainTainSenseAPI.Models
{
    public class TaskName : BaseModel
    {
        public int Id { get; set; } //Primary Key
        public int RoleId { get; set; } // Foreign Key
        public AppRole? Role { get; set; }
    }
}