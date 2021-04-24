using System;

namespace Лобараторная_работа_DAPPER
{
    /// <summary>
    /// Класс PaymentStatuses
    /// </summary>
    public class PaymentStatuses
    {
        public int ID { get; set; }
        public int PaymentID { get; set; }
        public int StatusID { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
