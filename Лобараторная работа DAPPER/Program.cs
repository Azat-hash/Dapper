using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Лобараторная_работа_DAPPER
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString);


            using (connection)
            {
                connection.Open();

                GetTerminals(connection);
                Console.WriteLine("Выберите терминала");
                var terminal = Convert.ToInt32(Console.ReadLine());

                GetOperators(connection);
                Console.WriteLine("Выберите оператора");
                var operators = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Cумма платежа");
                var paySum = Convert.ToInt32(Console.ReadLine());

                const int comissionSum = 5;

                int idPayment =  StatusPaymentsSave(connection, paySum,comissionSum, terminal, operators);

                GetAllPayments(connection);

                Console.WriteLine($"Отправить платеж PaymentID{idPayment} оператору");
                
                connection.Close();

               
                Console.ReadKey();
            }
        }
        
        /// <summary>
        /// Получение операторов
        /// </summary>
        /// <param name="getOperatorStatuses"></param>
        /// <returns></returns>
        private static PaymentsType GetPaymentsType(OperatorStatuses getOperatorStatuses)
        {
            
            PaymentsType paymentsType;

            switch (getOperatorStatuses)
            {
                case OperatorStatuses.Accepted:
                    paymentsType = PaymentsType.Accepted;
                    break;
                case OperatorStatuses.Rejected:
                    paymentsType = PaymentsType.Error;
                    break;

                default:
                    paymentsType = PaymentsType.Confirmed;
                    break;
            }


            return (paymentsType);
        }
        /// <summary>
        /// Запросы
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paymentSum"></param>
        /// <param name="CommissionSum"></param>
        /// <param name="TerminalID"></param>
        /// <param name="OperatorID"></param>
        public static int StatusPaymentsSave(SqlConnection connection,decimal paymentSum ,decimal CommissionSum, int TerminalID, int OperatorID)
        {
            //using (SqlTransaction transaction = connection.BeginTransaction())
            
                try
                {
                    var query =
                        @"INSERT INTO dbo.Payments(PaymentSum, ComissionSum, TransactionDate, TerminalID, OperatorID)
                          VALUES
                         (@PaymentSum, @ComissionSum, @TransactionDate, @TerminalID, @OperatorID)";

                    connection.Execute(query, new
                    {
                        PaymentSum = paymentSum,
                        ComissionSum = CommissionSum,
                        TransactionDate = DateTime.Now,
                        TerminalID = TerminalID,
                        OperatorID = OperatorID
                    });

                     query = "select SCOPE_IDENTITY()";

                    var paymentID = connection.QueryFirstOrDefault<int>(query);

                     query =
                      @"INSERT INTO dbo.Payments(PaymentID, StatusID, CreateDate)
                          VALUES (@PaymentID, @StatusID, @CreateDate)";

                    connection.Execute(query, new
                    {
                        PaymentID = paymentID,
                        StatusID = (int)PaymentsType.Accepted,
                        CreateDate = DateTime.Now,
                    });

                return paymentID;
                }
                
                catch (Exception e)
                {
                    
                    Console.WriteLine($"Во время сохранение произошла ошибка ERROR {e.Message}:{ e.StackTrace}");
                }
            return 0;
        }
     
        /// <summary>
        /// Запрос на таблицу терминал
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static void GetTerminals(SqlConnection connection)
        {
            var query = "select * from Terminals";
            var terminals = connection.Query<Terminals>(query).ToList();
            foreach(var terminalse in terminals)
            {
                Console.WriteLine($"ID терминала:{terminalse.TerminalID} ---Название:{terminalse.Name}");
            }
        }
        /// <summary>
        /// Запрос на таблицу оператор
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static void GetOperators(SqlConnection connection)
        {
            var query = "select * from Operators";
            var operators = connection.Query<Operators>(query).ToList();
            foreach(var operatorr in operators)
            {
                Console.WriteLine($"ID оператора:{operatorr.OperatorID} ---Название:{operatorr.Name}");
            }
        }
        /// <summary>
        /// Запрос на Payments
        /// </summary>
        /// <param name="connection"></param>
        public static void GetAllPayments(SqlConnection connection)
        {
            var query = "select * from Payments";
            var payments = connection.Query<Payments>(query).ToList();
            foreach (var pay in payments)
            {
                Console.WriteLine($"ID payment:{pay.PaymentID} ---Комиссия:{pay.ComissionSum}--Cумма:{pay.PaymentSum}");
            }
        }
    }   
 }

