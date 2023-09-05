using Code.StaticData;
using Code.UI;

namespace Code.Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        public WindowConfig ForWindowByType(WindowType type);
    }
}