using Dapper;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreatePatientDto dto);
}

public class PatientRepository : IPatientRepository
{
    private readonly IDbConnectionFactory _db;
    public PatientRepository(IDbConnectionFactory db) => _db = db;

    public async Task<Patient?> GetByIdAsync(int id)
    {
        using var conn = _db.Create();
        return await conn.QueryFirstOrDefaultAsync<Patient>("SELECT id, first_name as FirstName, last_name as LastName, gender, dob FROM patients WHERE id=@id", new { id });
    }

    public async Task<int> CreateAsync(CreatePatientDto dto)
    {
        using var conn = _db.Create();
        var sql = @"INSERT INTO patients (first_name, last_name, gender, dob) VALUES (@FirstName, @LastName, @Gender, @Dob) RETURNING id";
        return await conn.ExecuteScalarAsync<int>(sql, dto);
    }
}

public class Patient { public int Id { get; set; } public string FirstName { get; set; } = ""; public string LastName { get; set; } = ""; public string Gender { get; set; } = ""; public DateTime Dob { get; set; } }
