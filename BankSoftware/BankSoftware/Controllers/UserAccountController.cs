using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using BankSoftware.Models;
using System.IO;

namespace BankSoftware.Controllers
{
    public class UserAccountController : Controller
    {

        UserAccountRepo repo;

        public UserAccountController()
        {
            this.repo = new UserAccountRepo();

        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserAccountModel UA)
        {

            if (ModelState.IsValid)
            {
                var loggedInUser = User.Identity.Name;

                var count = repo.Add(UA, loggedInUser);

                if (count > 0)
                {
                    ViewBag.Okay = "Data Added";
                }
            }
            return View();
        }

        public ActionResult GetAll()
        {
            var loggedInUser = User.Identity.Name;
            var data = repo.GetAllData(loggedInUser);
            return View(data);

        }


        public ActionResult Delete(int id)
        {
            var data = repo.DeleteData(id);
            return RedirectToAction("GetAll");
        }

        public ActionResult Transfer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Transfer(string senderAccountId, string receiverAccountId, decimal amount)
        {
            bool transferResult = repo.TransferFunds(senderAccountId, receiverAccountId, amount);
            var loggedInUser = User.Identity.Name;

            if (transferResult)
            {
                ViewBag.SuccessMessage = $"Successfully transferred {amount:C} from account {senderAccountId} to account {receiverAccountId}";
                logToFile($"User- ({loggedInUser}) Successfully transferred {amount:C} from account {senderAccountId} to account {receiverAccountId}");
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to transfer funds. Please check sender's balance or account details.";
                logToFile($"User- ({loggedInUser}) Failed to transfer funds. Please check sender's balance or account details.");

            }

            return RedirectToAction("GetAll", "UserAccount");

        }



        public ActionResult Withdraw()
        {
            return View();
        }

        private void logToFile(string logMessage)
        {
            string LogFilePath = "C:/Users/Rafi/Documents/New folder/BankSoftware/transaction.txt";

            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now} - {logMessage}");
            }

        }


        [HttpPost]
        public ActionResult Withdraw(string accountNumber, decimal amount)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = User.Identity.Name;
                bool withdrawalResult = repo.WithdrawFunds(accountNumber, amount);

                if (withdrawalResult)
                {
                    ViewBag.SuccessMessage = $"Successfully withdrawn {amount:C} from account {accountNumber}";
                    logToFile($"User- ({loggedInUser}) Successfully withdrawn {amount:C} from account {accountNumber}");
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to withdraw funds. Please check account number or available balance.";
                    logToFile("Failed to withdraw funds. Please check account number or available balance.");
                    logToFile($"User- ({loggedInUser}) Failed to withdraw funds. Please check account number or available balance.");
                }
            }
            return View();
        }

       

    public ActionResult Deposit()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Deposit(string accountNumber, decimal amount)
        {
            if (ModelState.IsValid)
            {
                bool depositResult = repo.DepositFunds(accountNumber, amount);
                var loggedInUser = User.Identity.Name;
                if (depositResult)
                {
                    ViewBag.SuccessMessage = $"Successfully deposited {amount:C} into account {accountNumber}";
                    logToFile($"User- ({loggedInUser}) Successfully deposited {amount:C} into account {accountNumber}");

                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to deposit funds. Please check account number or amount.";
                    logToFile($"Failed to deposit funds. Please check account number or amount.");
                    logToFile($"User- ({loggedInUser}) Failed to deposit funds. Please check account number or amount.");
                }
            }
            return View();
        }


        public ActionResult PayUtilityBill()
        {
            var model = new UserAccountModel.UtilityBillPaymentModel();


            model.BillTypeList = new List<SelectListItem>
    {
        new SelectListItem { Value = "Gas", Text = "Gas" },
        new SelectListItem { Value = "Electricity", Text = "Electricity" },
        new SelectListItem { Value = "Water", Text = "Water" }

    };

            return View(model);
        }

        [HttpPost]
        public ActionResult PayUtilityBill(UserAccountModel.UtilityBillPaymentModel model)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = User.Identity.Name;
                bool paymentResult = repo.PayUtilityBill(model.AccountNumber, model.BillType, model.BillAmount);

                if (paymentResult)
                {
                    ViewBag.SuccessMessage = $"Successfully paid {model.BillType} bill of {model.BillAmount:C} from account {model.AccountNumber}";
                    logToFile($"User- ({loggedInUser}) Successfully paid {model.BillType} bill of {model.BillAmount:C} from account {model.AccountNumber}");
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to pay the bill. Insufficient funds or account details.";
                  
                    logToFile($"User- ({loggedInUser}) Failed to pay the bill. Insufficient funds or account details.");
                }
            }


            return RedirectToAction("GetAll", "UserAccount");
        }

        /*
        public ActionResult TransactionHistory()
        {
            var transactions = repo.GetAllTransactions();
            return View(transactions);
        }
     */
    }
}