namespace Code.Infrastructure.Services.ProgressServices
{
    public interface IPersistentProgressService
    {
        int CoinsAmount { get; }
        int ReputationAmount { get; }
    }
}