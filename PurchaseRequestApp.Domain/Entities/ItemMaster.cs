namespace PurchaseRequestApp.Domain.Entities
{
    public class ItemMaster
{
    public Guid Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ItemCode { get; set; } = string.Empty;
    public string UOM { get; set; } = string.Empty;

    public ICollection<PurchaseRequestItem> PurchaseItems { get; set; } = new List<PurchaseRequestItem>();
}
}