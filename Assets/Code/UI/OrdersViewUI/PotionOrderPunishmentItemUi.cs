﻿using Code.Logic.Orders;
using TMPro;
using UnityEngine;

namespace Code.UI.OrdersViewUI
{
    public class PotionOrderPunishmentItemUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _reputationAmountText;

        public void SetPunishment(PotionOrderPunishment punishment)
        {
            _reputationAmountText.text = punishment.ReputationAmount.ToString();
        }
    }
}