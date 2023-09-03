namespace Code.Infrastructure.Services.RandomServices
{
    public class UnityRandomService : IRandomService
    {
        public int Next(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public float Next(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}