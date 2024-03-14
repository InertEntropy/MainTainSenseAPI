using MainTainSenseAPI.Contracts;

namespace MainTainSenseAPI.Models;

public partial class PmChecklist : IEntityWithId
{
    public int Id { get; set; }

    public int ChecklistId { get; set; }

    public int? PmId { get; set; }

    public virtual PreventiveMaintenance? Pm { get; set; }
}
