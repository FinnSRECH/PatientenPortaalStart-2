using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospital.PatientPortal.Pages.Audit;

[Authorize]
public class IndexModel : PageModel
{
	private readonly HospitalDataService _data;

	public IndexModel(HospitalDataService data)
	{
		_data = data;
	}

	public IReadOnlyList<AuditLog> AuditLogs { get; private set; }
		= new List<AuditLog>();

	public void OnGet()
	{
		AuditLogs = _data.GetAuditLogs();
	}
}