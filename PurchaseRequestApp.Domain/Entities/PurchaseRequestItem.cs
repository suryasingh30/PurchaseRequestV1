namespace PurchaseRequestApp.Domain.Entities
{
public class PurchaseRequestItem
{
    public Guid Id { get; set; }
    public Guid PurchaseRequestId { get; set; }
    public Guid ItemId { get; set; }
    public string? TechSpec { get; set; }
    public Guid? MakeId { get; set; }
    public string UOM { get; set; } = string.Empty;
    public decimal Qty { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount => Qty * Rate;
    public DateTime RequiredOn { get; set; }
    public string? ItemRemarks { get; set; }

    public ItemMaster? Item { get; set; }
    public MakeMaster? Make { get; set; }
    public PurchaseRequest? PurchaseRequest { get; set; }
}
}