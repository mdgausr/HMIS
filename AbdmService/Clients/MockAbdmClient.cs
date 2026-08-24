public class MockAbdmClient : IAbdmClient
{
    public Task<AbdmVerifyResult> VerifyHealthIdAsync(string healthId)
    {
        var last = healthId?.Length > 0 ? healthId[^1] : '0';
        var success = char.IsDigit(last) && ((last - '0') % 2 == 0);
        if (success) return Task.FromResult(new AbdmVerifyResult(true, "Verified (mock)", "John Doe"));
        return Task.FromResult(new AbdmVerifyResult(false, "Not found (mock)", null));
    }
}
