namespace Code.Infrastructure.Services.RandomServices
{
    public interface IRandomService
    {
        int Next(int min, int max);
        float Next(float min, float max);
    }
}