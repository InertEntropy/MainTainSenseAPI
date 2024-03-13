using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class WorkOrder
{
    public int WorkOrderId { get; set; }

    [Required]
    public string? WorkOrderDescription { get; set; }

    public string? RequestedBy { get; set; }

    public string? CreatedDate { get; set; }

    public int? AssetId { get; set; }

    public int? PriorityId { get; set; }

    public string? ScheduledDate { get; set; }

    public string? Notes { get; set; }

    public int? AssignedTeamId { get; set; }

    public int? UserId { get; set; }

    public int? StatusId { get; set; }

    public string? CompletionDate { get; set; }

    public string? DueDate { get; set; }

    public string? LastUpdate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Asset? Asset { get; set; }

    public virtual Priority? Priority { get; set; }

    public virtual Status? Status { get; set; }
}
