using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using BankSoftware.Models;
using System.Web;

namespace BankSoftware
{
    public class UserAccountRepo
    {
        public int Add(UserAccountModel model, string loggedInUser)
        {
            using (var context = new BankEntities())
            {

                UserAccount s = new UserAccount();


                s.UA_NO = model.UA_NO;
                //s.UA_Holder = model.UA_Holder;
                s.UA_Holder = loggedInUser;


                s.Balance = model.Balance;



                context.UserAccount.Add(s);
                context.SaveChanges();

                return s.UA_ID;


            }
        }

        public List<UserAccountModel> GetAllData(string loggedInUser)
        {

            using (var context = new BankEntities())
            {
                var result = context.UserAccount
                    .Where(x => x.UA_Holder == loggedInUser)
                    .Select(x => new UserAccountModel()
                    {
                        UA_ID = x.UA_ID,
                        UA_NO = x.UA_NO,
                        UA_Holder = x.UA_Holder,
                        Balance = x.Balance,
                    })
                    .ToList();

                return result;
            }
        }



        public UserAccountModel GetDetails(int id)
        {
            using (var context = new BankEntities())
            {
                var result = context.UserAccount.Where(x => x.UA_ID == id).Select(x => new UserAccountModel()
                {
                    UA_ID = x.UA_ID,
                    UA_NO = x.UA_NO,
                    UA_Holder = x.UA_Holder,
                    Balance = x.Balance
                }
                ).FirstOrDefault();

                return result;
            }
        }

        public bool UpdateData(int id, UserAccountModel model)
        {
            using (var context = new BankEntities())
            {
                var M = context.UserAccount.FirstOrDefault(x => x.UA_ID == id);
                if (M != null)
                {

                    M.UA_NO = model.UA_NO;
                    M.UA_Holder = model.UA_Holder;
                    M.Balance = model.Balance;

                }

                context.SaveChanges();

                return true;
            }
        }

        public bool DeleteData(int id)
        {
            using (var context = new BankEntities())
            {
                var M = context.UserAccount.FirstOrDefault(x => x.UA_ID == id);
                if (M != null)
                {
                    context.UserAccount.Remove(M);
                    context.SaveChanges();
                    return true;
                }

                return false;
            }
        }


        public bool TransferFunds(string senderAccountNumber, string receiverAccountNumber, decimal amount)
        {
            using (var context = new BankEntities())
            {

                var senderAccount = context.UserAccount.FirstOrDefault(x => x.UA_NO == senderAccountNumber);
                var receiverAccount = context.UserAccount.FirstOrDefault(x => x.UA_NO == receiverAccountNumber);

                if (senderAccount != null && receiverAccount != null)
                {

                    if (decimal.TryParse(senderAccount.Balance, out decimal senderBalance) &&
                        decimal.TryParse(receiverAccount.Balance, out decimal receiverBalance))
                    {
                        if (senderBalance >= amount)
                        {

                            senderBalance -= amount;


                            receiverBalance += amount;


                            senderAccount.Balance = senderBalance.ToString();
                            receiverAccount.Balance = receiverBalance.ToString();


                            context.SaveChanges();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool WithdrawFunds(string accountNumber, decimal amount)
           {
               using (var context = new BankEntities())
               {
                   var account = context.UserAccount.FirstOrDefault(x => x.UA_NO == accountNumber);

                   if (account != null && decimal.TryParse(account.Balance, out decimal currentBalance))
                   {
                       if (currentBalance >= amount)
                       {
                           currentBalance -= amount;
                           account.Balance = currentBalance.ToString();
                           context.SaveChanges();
                           return true;
                       }
                       else
                       {
                           return false;
                       }
                   }
                   else
                   {
                       return false;
                   }
               }
           }
       
           public bool DepositFunds(string accountNumber, decimal amount)
               {
                   using (var context = new BankEntities())
                   {
                       var account = context.UserAccount.FirstOrDefault(x => x.UA_NO == accountNumber);

                       if (account != null && decimal.TryParse(account.Balance, out decimal currentBalance))
                       {
                           currentBalance += amount;
                           account.Balance = currentBalance.ToString();
                           context.SaveChanges();
                           return true;
                       }
                       else
                       {
                           return false;
                       }
                   }
               }

             
        /*
        public bool DepositFunds(string accountNumber, decimal amount)
                {
                    using (var context = new BankEntities())
                    {
                        var account = context.UserAccount.FirstOrDefault(x => x.UA_NO == accountNumber);

                        if (account != null && decimal.TryParse(account.Balance, out decimal currentBalance))
                        {
                            currentBalance += amount;
                            account.Balance = currentBalance.ToString();

                            context.SaveChanges();

                            // Record the deposit transaction
                            RecordTransaction(accountNumber, "Deposit", amount);

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
          */

   
        public bool PayUtilityBill(string accountNumber, string billType, decimal billAmount)
        {
            using (var context = new BankEntities())
            {
                var account = context.UserAccount.FirstOrDefault(x => x.UA_NO == accountNumber);

                if (account != null && decimal.TryParse(account.Balance, out decimal currentBalance))
                {
                    if (currentBalance >= billAmount)
                    {
                        currentBalance -= billAmount;
                        account.Balance = currentBalance.ToString();


                        account.UtilityBillType = billType;
                        account.UtilityBillAmount = billAmount;

                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
     
         /* 
        public bool PayUtilityBill(string accountNumber, string billType, decimal billAmount)
        {
            using (var context = new BankEntities())
            {
                var account = context.UserAccount.FirstOrDefault(x => x.UA_NO == accountNumber);

                if (account != null && decimal.TryParse(account.Balance, out decimal currentBalance))
                {
                    if (currentBalance >= billAmount)
                    {
                        currentBalance -= billAmount;
                        account.Balance = currentBalance.ToString();

                        // Update the utility bill details
                        account.UtilityBillType = billType;
                        account.UtilityBillAmount = billAmount;

                        context.SaveChanges();

                        // Record the bill payment transaction
                        RecordTransaction(accountNumber, "Bill Payment", billAmount);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
         */

        /*
               public bool RecordTransaction(string accountNumber, string transactionType, decimal amount)
               {
                   try
                   {
                       using (var context = new BankEntities())
                       {
                           var transaction = new TransactionModel
                           {
                               AccountNumber = accountNumber,
                               TransactionType = transactionType,
                               Amount = amount,
                               TransactionDate = DateTime.Now
                           };

                           context.TransactionHistory.Add(transaction);
                           context.SaveChanges();
                       }
                       return true;
                   }
                   catch (Exception ex)
                   {

                       return false;
                   }
               }

               public List<TransactionModel> GetAllTransactions()
               {
                   using (var context = new BankEntities())
                   {
                       return context.TransactionHistory.ToList(); 
                   }
               }
                */
    }
}