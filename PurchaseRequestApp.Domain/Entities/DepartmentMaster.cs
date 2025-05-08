namespace PurchaseRequestApp.Domain.Entities
{
public class DepartmentMaster
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<PurchaseRequest> Requests { get; set; } = new List<PurchaseRequest>();
}
}