namespace Code.Logic.Orders
{
    public class SelectedPotionOrderHolder
    {
        public PotionOrder SelectedOrder { get; private set; }

        public void Initialize(PotionOrder selectedOrder)
        {
            SelectedOrder = selectedOrder;
        }
    }
}