using Dapper;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task CreateAsync(User user);
}

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _dbFactory;
    public UserRepository(IDbConnectionFactory dbFactory) => _dbFactory = dbFactory;

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var conn = _dbFactory.Create();
        var sql = "SELECT id, username, password_hash as PasswordHash, role FROM users WHERE username = @u LIMIT 1";
        return await conn.QueryFirstOrDefaultAsync<User>(sql, new { u = username });
    }

    public async Task CreateAsync(User user)
    {
        using var conn = _dbFactory.Create();
        var sql = "INSERT INTO users (username, password_hash, role) VALUES (@Username, @PasswordHash, @Role)";
        await conn.ExecuteAsync(sql, user);
    }
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = "User";
}
