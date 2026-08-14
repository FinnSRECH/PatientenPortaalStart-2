using Hospital.Domain.Enums;
using Hospital.Domain.Models;

namespace Hospital.PatientPortal.Services;

public class HospitalDataService
{
	private readonly List<Patient> _patients = new();
	private readonly List<Complaint> _complaints = new();
	private readonly List<Consultation> _consultations = new();
	private readonly List<Treatment> _treatments = new();
	private readonly List<Evaluation> _evaluations = new();
	private readonly List<PatientDocument> _documents = new();

	private int _patientId = 1;
	private int _complaintId = 1;
	private int _documentId = 1;

	public HospitalDataService(PasswordService passwords)
	{
		var credentials = passwords.HashPassword("Welkom123!");

		_patients.Add(new Patient
		{
			Id = _patientId++,
			FirstName = "Jan",
			LastName = "de Vries",
			Email = "jan@demo.nl",
			PhoneNumber = "0612345678",
			DateOfBirth = new DateOnly(1988, 4, 12),
			Address = "Stationsstraat 12, Groningen",
			PasswordHash = credentials.Hash,
			PasswordSalt = credentials.Salt
		});

		_treatments.Add(new Treatment
		{
			Id = 1,
			PatientId = 1,
			Name = "Behandeling knieklachten",
			Description = "Onderzoek en behandeling van aanhoudende pijn aan de rechterknie.",
			SurgeonName = "Dr. Jansen",
			NurseName = "Sophie de Boer",
			StartDate = DateTime.Today.AddDays(-7),
			Status = TreatmentStatus.Active
		});

		_consultations.Add(new Consultation
		{
			Id = 1,
			PatientId = 1,
			TreatmentId = 1,
			StartTime = DateTime.Today.AddDays(-5).AddHours(14),
			DoctorName = "Dr. Jansen",
			Room = "B2.14",
			Reason = "Eerste onderzoek knieklachten",
			Status = AppointmentStatus.Completed,
			MedicalReport = "De knie is onderzocht. Er is voorlopig geen spoedindicatie. Een vervolgconsult is gepland."
		});

		_consultations.Add(new Consultation
		{
			Id = 2,
			PatientId = 1,
			TreatmentId = 1,
			StartTime = DateTime.Today.AddDays(5).AddHours(10).AddMinutes(30),
			DoctorName = "Dr. Jansen",
			Room = "B2.14",
			Reason = "Controle knieklachten",
			Status = AppointmentStatus.Planned
		});

		_evaluations.Add(new Evaluation
		{
			Id = 1,
			PatientId = 1,
			ConsultationId = 1,
			CreatedAt = DateTime.Today.AddDays(-5).AddHours(15),
			CareProviderName = "Dr. Jansen",
			Title = "Evaluatie eerste consultatie",
			Content = "De klachten worden verder opgevolgd. Tijdens het vervolgconsult bespreken we het herstel en eventuele vervolgstappen."
		});

		_documents.Add(new PatientDocument
		{
			Id = _documentId++,
			PatientId = 1,
			FileName = "verwijsbrief-huisarts.pdf",
			Description = "Verwijsbrief van de huisarts voor de knieklachten.",
			ContentType = "application/pdf",
			FilePath = "",
			UploadedAt = DateTime.Now.AddDays(-10),
			UploadedBy = "Huisarts",
			UploadedByHealthcareProvider = true
		});

		_documents.Add(new PatientDocument
		{
			Id = _documentId++,
			PatientId = 1,
			FileName = "medicatieoverzicht.pdf",
			Description = "Actueel medicatieoverzicht.",
			ContentType = "application/pdf",
			FilePath = "",
			UploadedAt = DateTime.Now.AddDays(-5),
			UploadedBy = "Jan de Vries",
			UploadedByHealthcareProvider = false
		});
	}

	public Patient? GetPatient(int id)
	{
		return _patients.FirstOrDefault(p => p.Id == id);
	}

	public Patient? FindPatientByEmail(string email)
	{
		return _patients.FirstOrDefault(
			p => p.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
	}

	public Treatment? GetActiveTreatment(int patientId)
	{
		return _treatments.FirstOrDefault(
			t => t.PatientId == patientId &&
				 t.Status == TreatmentStatus.Active);
	}

	public Treatment? GetTreatment(int treatmentId, int patientId)
	{
		return _treatments.FirstOrDefault(
			t => t.Id == treatmentId &&
				 t.PatientId == patientId);
	}

	public IReadOnlyList<Treatment> GetTreatments(int patientId)
	{
		return _treatments
			.Where(t => t.PatientId == patientId)
			.OrderByDescending(t => t.StartDate)
			.ToList();
	}

	public IReadOnlyList<Consultation> GetConsultations(int patientId)
	{
		return _consultations
			.Where(c => c.PatientId == patientId)
			.OrderBy(c => c.StartTime)
			.ToList();
	}

	public IReadOnlyList<Consultation> GetConsultationsForTreatment(
		int treatmentId,
		int patientId)
	{
		return _consultations
			.Where(c => c.TreatmentId == treatmentId &&
						c.PatientId == patientId)
			.OrderByDescending(c => c.StartTime)
			.ToList();
	}

	public IReadOnlyList<Consultation> GetConsultationHistory(int patientId)
	{
		return _consultations
			.Where(c => c.PatientId == patientId &&
						c.Status == AppointmentStatus.Completed)
			.OrderByDescending(c => c.StartTime)
			.ToList();
	}

	public IReadOnlyList<Evaluation> GetEvaluations(int patientId)
	{
		return _evaluations
			.Where(e => e.PatientId == patientId)
			.OrderByDescending(e => e.CreatedAt)
			.ToList();
	}

	public IReadOnlyList<Complaint> GetComplaints(int patientId)
	{
		return _complaints
			.Where(c => c.PatientId == patientId)
			.OrderByDescending(c => c.CreatedAt)
			.ToList();
	}

	public IReadOnlyList<PatientDocument> GetDocuments(int patientId)
	{
		return _documents
			.Where(d => d.PatientId == patientId)
			.OrderByDescending(d => d.UploadedAt)
			.ToList();
	}

	public Patient AddPatient(Patient patient)
	{
		patient.Id = _patientId++;
		_patients.Add(patient);
		return patient;
	}

	public void AddComplaint(Complaint complaint)
	{
		complaint.Id = _complaintId++;
		complaint.CreatedAt = DateTime.Now;
		_complaints.Add(complaint);
	}

	public void AddDocument(PatientDocument document)
	{
		document.Id = _documentId++;
		document.UploadedAt = DateTime.Now;
		_documents.Add(document);
	}
public void UpdatePatient(Patient patient)
	{
		var existing = GetPatient(patient.Id);

		if (existing is null)
		{
			return;
		}

		existing.FirstName = patient.FirstName;
		existing.LastName = patient.LastName;
		existing.PhoneNumber = patient.PhoneNumber;
		existing.Address = patient.Address;
	}
}
