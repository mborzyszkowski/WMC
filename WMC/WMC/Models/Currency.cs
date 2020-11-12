using System;
using System.Collections.Generic;
using System.Text;

namespace WMC.Models
{
    public class Currency
    {
        public enum CurrencyType
        {
            PLN, USD
        }

        public CurrencyType Type { get; set; }
    }
}
