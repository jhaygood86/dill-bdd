using System;
using System.Collections.Generic;
using System.Text;

namespace Relish.Tests.Examples
{
    public class Jeff
    {
        public bool HasFaultyMicrowave { get; set; } 
        public bool HasReceipt { get; set; }
        public decimal Money { get; set; }

        private decimal _microwavePurchasePrice = 0.00m;

        public Jeff()
        {
            Money = 100.00m;
        }

        public void BuyFaultyMicrowave(decimal purchasePrice)
        {
            _microwavePurchasePrice = purchasePrice;
            HasFaultyMicrowave = true;
            Money -= purchasePrice;
        }

        public void ReturnMicrowave()
        {
            if (HasReceipt && HasFaultyMicrowave)
            {
                HasFaultyMicrowave = false;
                Money += _microwavePurchasePrice;
            }
        }
    }
}
