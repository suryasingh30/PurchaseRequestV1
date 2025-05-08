namespace PurchaseRequestApp.Domain.Entities
{
public class MakeMaster
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<PurchaseRequestItem> Items { get; set; } = new List<PurchaseRequestItem>();
}
}