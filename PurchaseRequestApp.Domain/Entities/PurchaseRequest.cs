namespace PurchaseRequestApp.Domain.Entities
{
public class PurchaseRequest
{
    public Guid Id { get; set; }
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime DocumentDate { get; set; } = DateTime.Now;
    public Guid CompanyId { get; set; }
    public string IndentType { get; set; } = string.Empty; // "Capital" or "Revenue"
    public bool IsReserved { get; set; }
    public Guid DepartmentId { get; set; }
    public string ChargeType { get; set; } = string.Empty; // "Chargeable" / "Non Chargeable"
    public string RequestedBy { get; set; } = string.Empty;
    public string IndentTags { get; set; } = string.Empty; // comma-separated tags
    public string? Remarks { get; set; }

    public CompanyMaster? Company { get; set; }
    public DepartmentMaster? Department { get; set; }
    public ICollection<PurchaseRequestItem> Items { get; set; } = new List<PurchaseRequestItem>();
}
}