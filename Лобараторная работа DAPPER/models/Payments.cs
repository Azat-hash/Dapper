using System;

namespace Лобараторная_работа_DAPPER
{
    /// <summary>
    /// Класс Payments
    /// </summary>
    public class Payments
    {
        public int PaymentID { get; set; }
        public decimal PaymentSum { get; set; }
        public decimal ComissionSum { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TerminalID { get; set; }
        public int OperatorID { get; set; }
        
    }
}
