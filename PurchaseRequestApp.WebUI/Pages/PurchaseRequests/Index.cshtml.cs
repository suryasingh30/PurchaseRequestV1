using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PurchaseRequestApp.Domain.Entities;
using PurchaseRequestApp.Infrastructure.Data;

namespace PurchaseRequestApp.WebUI.Pages.PurchaseRequests
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<PurchaseRequest> PurchaseRequests { get; set; } = new();

        public async Task OnGetAsync()
        {
            PurchaseRequests = await _context.PurchaseRequests
                .Include(p => p.Company)
                .Include(p => p.Department)
                .OrderByDescending(p => p.DocumentDate)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var request = await _context.PurchaseRequests.FindAsync(id);
            
            if (request != null)
            {
                _context.PurchaseRequests.Remove(request);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Purchase request deleted successfully";
            }
            
            return RedirectToPage();
        }
    }
}
