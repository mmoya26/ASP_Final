using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class LocationController : Controller
    {
        private HENRYEntities db = new HENRYEntities();

        // GET: Location
        public ActionResult Location()
        {
            // #4 LINQ Statement
            var data = from b in db.BRANCHes select new { b.BRANCH_NAME, b.BRANCH_NUM};
            int size = 0;
            int counter = 0;

            foreach (var publisher in data)
            {
                size++;
            }

            string[] branchNames = new string[size];
            string[] branchNumbers = new string[size];

            foreach (var branch in data)
            {
                branchNames[counter] = branch.BRANCH_NAME;
                branchNumbers[counter] = branch.BRANCH_NUM.ToString();
                counter++;
            }

            ViewBag.branchNames = branchNames;
            ViewBag.branchNumbers = branchNumbers;
            return View();
        }

        public ActionResult InventoryLocation(string optionSelected)
        {
            var branches = db.BRANCHes.ToList();

            BRANCH returnBranch = new BRANCH();

            foreach (var branch in branches)
            {
                if (branch.BRANCH_NUM.ToString() == optionSelected)
                {
                    returnBranch.BRANCH_NAME = branch.BRANCH_NAME;
                    returnBranch.BRANCH_NUM = branch.BRANCH_NUM;
                    returnBranch.BRANCH_LOCATION = branch.BRANCH_LOCATION;
                    returnBranch.NUM_EMPLOYEES = branch.NUM_EMPLOYEES;
                }
            }

            // List procedure for locations
            int size = 0;
            int counter = 0;
            foreach (var branch in branches)
            {
                size++;
            }

            string[] branchNames = new string[size];
            string[] branchNumbers = new string[size];

            foreach (var branch in branches)
            {
                branchNames[counter] = branch.BRANCH_NAME;
                branchNumbers[counter] = branch.BRANCH_NUM.ToString();
                counter++;
            }

            // Procedure for books to display
            var inventories = db.INVENTORies.ToList();
            var allBooks = db.BOOKs.ToList();

            var inventoryCounter = 0;
            var booksToDisplaySize = 0;

            List<string> bookCodes = new List<string>();

            foreach (var record in inventories)
            {
                if (record.BRANCH_NUM.ToString() == optionSelected)
                {
                    bookCodes.Add(record.BOOK_CODE);
                    booksToDisplaySize++;
                }
            }

            string[] bookTitles = new string[booksToDisplaySize];
            string[] bookQuantities = new string[booksToDisplaySize];
            string[] bookCodesReturn = new string[booksToDisplaySize];

            foreach (var bookCode in bookCodes)
            {
                foreach (var book in allBooks)
                {
                    if (book.BOOK_CODE == bookCode)
                    {
                        bookTitles[inventoryCounter] = book.TITLE;
                        bookCodesReturn[inventoryCounter] = book.BOOK_CODE;
                        inventoryCounter++;
                    }
                }
            }

            int secondCounterInventory = 0;

            foreach (var bookCode in bookCodes)
            {
                foreach (var inventory in inventories)
                {
                    if (inventory.BOOK_CODE == bookCode && inventory.BRANCH_NUM.ToString() == optionSelected)
                    {
                        bookQuantities[secondCounterInventory] = inventory.ON_HAND.ToString();
                    }
                }

                secondCounterInventory++;
            }

            ViewBag.branchNames = branchNames;
            ViewBag.branchNumbers = branchNumbers;
            ViewBag.returnBranch = returnBranch;
            ViewBag.bookTitles = bookTitles;
            ViewBag.bookQuantities = bookQuantities;
            ViewBag.bookCodesReturn = bookCodesReturn;
            return View("_ByLocationDisplay");
        }

        [HttpPost]
        public ActionResult _ByLocationDisplay(OptionSelected model)
        {
            string optionSelected;   

            if (model.optionSelected != null)
            {
                optionSelected = model.optionSelected;
            }
            else
            {
                optionSelected = "";
            }

            var branches = db.BRANCHes.ToList();

            BRANCH returnBranch = new BRANCH();

            foreach (var branch in branches)
            {
                if (branch.BRANCH_NUM.ToString() == optionSelected)
                {
                    returnBranch.BRANCH_NAME = branch.BRANCH_NAME;
                    returnBranch.BRANCH_NUM = branch.BRANCH_NUM;
                    returnBranch.BRANCH_LOCATION = branch.BRANCH_LOCATION;
                    returnBranch.NUM_EMPLOYEES = branch.NUM_EMPLOYEES;
                }
            }

            // List procedure for locations
            int size = 0;
            int counter = 0;
            foreach (var branch in branches)
            {
                size++;
            }

            string[] branchNames = new string[size];
            string[] branchNumbers = new string[size];

            foreach (var branch in branches)
            {
                branchNames[counter] = branch.BRANCH_NAME;
                branchNumbers[counter] = branch.BRANCH_NUM.ToString();
                counter++;
            }

            // Procedure for books to display
            var inventories = db.INVENTORies.ToList();
            var allBooks = db.BOOKs.ToList();

            var inventoryCounter = 0;
            var booksToDisplaySize = 0;

            List<string> bookCodes = new List<string>();

            foreach (var record in inventories)
            {
                if (record.BRANCH_NUM.ToString() == optionSelected)
                {
                    bookCodes.Add(record.BOOK_CODE);
                    booksToDisplaySize++;
                }
            }

            string[] bookTitles = new string[booksToDisplaySize];
            string[] bookQuantities = new string[booksToDisplaySize];
            string[] bookCodesReturn = new string[booksToDisplaySize];

            foreach (var bookCode in bookCodes)
            {
                foreach (var book in allBooks)
                {
                    if (book.BOOK_CODE == bookCode)
                    {
                        bookTitles[inventoryCounter] = book.TITLE;
                        bookCodesReturn[inventoryCounter] = book.BOOK_CODE;
                        inventoryCounter++;
                    }
                }
            }

            int secondCounterInventory = 0;

            foreach (var bookCode in bookCodes)
            {
                foreach (var inventory in inventories)
                {
                    if (inventory.BOOK_CODE == bookCode && inventory.BRANCH_NUM.ToString() == optionSelected)
                    {
                        bookQuantities[secondCounterInventory] = inventory.ON_HAND.ToString();
                    }
                }

                secondCounterInventory++;
            }

            ViewBag.branchNames = branchNames;
            ViewBag.branchNumbers = branchNumbers;
            ViewBag.returnBranch = returnBranch;
            ViewBag.bookTitles = bookTitles;
            ViewBag.bookQuantities = bookQuantities;
            ViewBag.bookCodesReturn = bookCodesReturn;
            return View();
        }
    }
}
