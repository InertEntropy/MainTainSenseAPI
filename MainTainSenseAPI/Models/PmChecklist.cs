namespace MainTainSenseAPI.Models;

public partial class PmChecklist
{
    public int ChecklistId { get; set; }

    public int? PmId { get; set; }

    public virtual PreventiveMaintenance? Pm { get; set; }
}
