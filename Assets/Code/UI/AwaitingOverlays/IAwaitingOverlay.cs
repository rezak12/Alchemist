namespace Code.UI.AwaitingOverlays
{
    public interface IAwaitingOverlay
    {
        public void Show(string message = "");
        public void Hide();
    }
}