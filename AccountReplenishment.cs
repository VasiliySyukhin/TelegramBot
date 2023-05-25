using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiwi.BillPayments.Client;
using Qiwi.BillPayments.Model;
using Qiwi.BillPayments.Model.In;
using Microsoft.Win32;
using Telegram_Bot_Project;

namespace Telegram_Bot_Project
{
    public class AccountReplenishment
    {
        private static string UserBillId { get; set; }
        


        public static string CreateForm(decimal sum)
        {
            BillPaymentsClient clientQiwi = BillPaymentsClientFactory.Create(secretKey: "eyJ2ZXJzaW9uIjoiUDJQIiwiZGF0YSI6eyJwYXlpbl9tZXJjaGFudF9zaXRlX3VpZCI6Im53MW9oai0wMCIsInVzZXJfaWQiOiI3OTAyNzAzMTQzMCIsInNlY3JldCI6IjlmZDA4NGYzZDJmNWNmYWZiNTNkZDkxNmMxYWQ3NmRiM2Y3OTY5NWU1MjBhNzRmMTkxOTBhYjAyNGE0MjhiMjkifX0=");
            //Qiwi.BillPayments.Model.Out.BillResponse 
            var newPayment = clientQiwi.CreateBill(
                info: new CreateBillInfo
                {
                    BillId = Guid.NewGuid().ToString(),
                    Amount = new MoneyAmount
                    {
                        ValueDecimal = sum,
                        CurrencyEnum = CurrencyEnum.Rub
                    },
                    Comment = "comment",
                    ExpirationDateTime = DateTime.Now.AddDays(5),
                    Customer = new Customer
                    {
                        Email = "example@mail.org",
                        Account = Guid.NewGuid().ToString(),
                        Phone = ""
                    }
                },
                customFields: new CustomFields
                {
                    ThemeCode = "кодСтиля"
                }
            );
            AccountReplenishment.UserBillId = newPayment.BillId.ToString();
            //var infopayment = clientQiwi.GetBillInfo(newPayment.BillId);
            string url = newPayment.PayUrl.ToString();
            return url;
        }
        public async Task<bool> UpdatePayments(string[] idAcc)
        {
            BillPaymentsClient clientQiwi = BillPaymentsClientFactory.Create(secretKey: "eyJ2ZXJzaW9uIjoiUDJQIiwiZGF0YSI6eyJwYXlpbl9tZXJjaGFudF9zaXRlX3VpZCI6Im53MW9oai0wMCIsInVzZXJfaWQiOiI3OTAyNzAzMTQzMCIsInNlY3JldCI6IjlmZDA4NGYzZDJmNWNmYWZiNTNkZDkxNmMxYWQ3NmRiM2Y3OTY5NWU1MjBhNzRmMTkxOTBhYjAyNGE0MjhiMjkifX0=");
            var localInfoUserBill = AccountReplenishment.UserBillId;
            int i = 0;
            string Status = "WAIT"; // можно любой текст
            while (Status != "PAID")
            {
                Status = clientQiwi.GetBillInfo(localInfoUserBill).Status.ValueString;
                i++;
                if (i == 600)
                {
                   
                    clientQiwi.CancelBill(billId: localInfoUserBill);
                    return false;
                }
                var taskdealy = Task.Delay(1000);
                await taskdealy;
            }

        

            return true;



        }
    }
   

}
