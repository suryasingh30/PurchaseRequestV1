using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PurchaseRequestApp.Domain.Entities;
using PurchaseRequestApp.Infrastructure.Data;

namespace PurchaseRequestApp.WebUI.Pages.PurchaseRequests
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PurchaseRequest PurchaseRequest { get; set; } = new();

        [BindProperty]
        public List<PurchaseRequestItem> RequestItems { get; set; } = new();

        public List<CompanyMaster> Companies { get; set; } = new();
        public List<DepartmentMaster> Departments { get; set; } = new();
        public List<ItemMaster> Items { get; set; } = new();
        public List<MakeMaster> Makes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Companies = await _context.CompanyMasters.ToListAsync();
            Departments = await _context.DepartmentMasters.ToListAsync();
            Items = await _context.ItemMasters.ToListAsync();
            Makes = await _context.MakeMasters.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Companies = await _context.CompanyMasters.ToListAsync();
                Departments = await _context.DepartmentMasters.ToListAsync();
                Items = await _context.ItemMasters.ToListAsync();
                Makes = await _context.MakeMasters.ToListAsync();
                return Page();
            }

            PurchaseRequest.Id = Guid.NewGuid();
            PurchaseRequest.DocumentDate = DateTime.Now;
            _context.PurchaseRequests.Add(PurchaseRequest);

            foreach (var item in RequestItems)
            {
                item.Id = Guid.NewGuid();
                item.PurchaseRequestId = PurchaseRequest.Id;
                _context.PurchaseRequestItems.Add(item);
            }

            await _context.SaveChangesAsync();

            if (Request.Form["exportCheckbox"] == "on")
            {
                return ExportToExcel();
            }

            TempData["SuccessMessage"] = "Data saved successfully!";
            return RedirectToPage("/PurchaseRequests/Index");
        }

        private IActionResult ExportToExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var headerWorksheet = workbook.Worksheets.Add("Request Header");
                
                headerWorksheet.Cell(1, 1).Value = "Document No";
                headerWorksheet.Cell(1, 2).Value = PurchaseRequest.DocumentNo;
                
                headerWorksheet.Cell(2, 1).Value = "Document Date";
                headerWorksheet.Cell(2, 2).Value = PurchaseRequest.DocumentDate;
                
                headerWorksheet.Cell(3, 1).Value = "Company";
                headerWorksheet.Cell(3, 2).Value = _context.CompanyMasters.Find(PurchaseRequest.CompanyId)?.Name;
                
                headerWorksheet.Cell(4, 1).Value = "Department";
                headerWorksheet.Cell(4, 2).Value = _context.DepartmentMasters.Find(PurchaseRequest.DepartmentId)?.Name;
                
                headerWorksheet.Cell(5, 1).Value = "Indent Type";
                headerWorksheet.Cell(5, 2).Value = PurchaseRequest.IndentType;
                
                headerWorksheet.Cell(6, 1).Value = "Charge Type";
                headerWorksheet.Cell(6, 2).Value = PurchaseRequest.ChargeType;
                
                headerWorksheet.Cell(7, 1).Value = "Is Reserved";
                headerWorksheet.Cell(7, 2).Value = PurchaseRequest.IsReserved ? "Yes" : "No";
                
                headerWorksheet.Cell(8, 1).Value = "Requested By";
                headerWorksheet.Cell(8, 2).Value = PurchaseRequest.RequestedBy;
                
                headerWorksheet.Cell(9, 1).Value = "Indent Tags";
                headerWorksheet.Cell(9, 2).Value = PurchaseRequest.IndentTags;
                
                headerWorksheet.Cell(10, 1).Value = "Remarks";
                headerWorksheet.Cell(10, 2).Value = PurchaseRequest.Remarks;

                var itemsWorksheet = workbook.Worksheets.Add("Request Items");
                
                itemsWorksheet.Cell(1, 1).Value = "Item";
                itemsWorksheet.Cell(1, 2).Value = "Technical Specification";
                itemsWorksheet.Cell(1, 3).Value = "Make";
                itemsWorksheet.Cell(1, 4).Value = "UOM";
                itemsWorksheet.Cell(1, 5).Value = "Quantity";
                itemsWorksheet.Cell(1, 6).Value = "Rate";
                itemsWorksheet.Cell(1, 7).Value = "Amount";
                itemsWorksheet.Cell(1, 8).Value = "Required On";
                itemsWorksheet.Cell(1, 9).Value = "Remarks";

                for (int i = 0; i < RequestItems.Count; i++)
                {
                    var item = RequestItems[i];
                    var row = i + 2;
                    
                    itemsWorksheet.Cell(row, 1).Value = _context.ItemMasters.Find(item.ItemId)?.ItemName;
                    itemsWorksheet.Cell(row, 2).Value = item.TechSpec; 
                    itemsWorksheet.Cell(row, 3).Value = _context.MakeMasters.Find(item.MakeId)?.Name;
                    itemsWorksheet.Cell(row, 4).Value = item.UOM;
                    itemsWorksheet.Cell(row, 5).Value = item.Qty; 
                    itemsWorksheet.Cell(row, 6).Value = item.Rate;
                    itemsWorksheet.Cell(row, 7).Value = item.Amount;
                    itemsWorksheet.Cell(row, 8).Value = item.RequiredOn;
                    itemsWorksheet.Cell(row, 9).Value = item.ItemRemarks;
                }

                headerWorksheet.Columns().AdjustToContents();
                itemsWorksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"PurchaseRequest_{PurchaseRequest.DocumentNo}_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                    );
                }
            }
        }
    }
}