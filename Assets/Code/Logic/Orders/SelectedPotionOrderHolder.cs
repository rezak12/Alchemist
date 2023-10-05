namespace Code.Logic.Orders
{
    public class SelectedPotionOrderHolder
    {
        public PotionOrder SelectedOrder { get; private set; }

        public void PutOrder(PotionOrder selectedOrder)
        {
            SelectedOrder = selectedOrder;
        }
    }
}