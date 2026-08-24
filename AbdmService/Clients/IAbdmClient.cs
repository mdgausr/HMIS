public interface IAbdmClient
{
    Task<AbdmVerifyResult> VerifyHealthIdAsync(string healthId);
}

public record AbdmVerifyResult(bool Success, string Message, string? AbhaHolderName);
