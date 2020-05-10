using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class HomeController : Controller
    {
        private HENRYEntities db = new HENRYEntities(); 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Author()
        {
            // #3 LINQ Statement
            var data = from author in db.AUTHORs select new { author.AUTHOR_FIRST, author.AUTHOR_LAST, author.AUTHOR_NUM };
            List<string> firstNames = new List<string>();
            string[] lastNames = new string[25];
            string[] authorNum = new string[25];
            int i = 0;

            foreach (var author in data)
            {
                firstNames.Add(author.AUTHOR_FIRST);
                lastNames[i] = author.AUTHOR_LAST;
                authorNum[i] = author.AUTHOR_NUM.ToString();
                i++;
            }

            ViewBag.firstNames = firstNames;
            ViewBag.lastNames = lastNames;
            ViewBag.authorNum = authorNum;
            return View();
        }
        [HttpPost]
        public ActionResult ByAuthorDisplay(OptionSelected model)
        {
            // Data for drop-down list
            var data = from author in db.AUTHORs select new { author.AUTHOR_FIRST, author.AUTHOR_LAST, author.AUTHOR_NUM};

            // Books data

            int authorNumberSelected = 0;

            if (model.optionSelected != null)
            {
                authorNumberSelected = Int32.Parse(model.optionSelected);
            }
            else
            {
                authorNumberSelected = 0;
            }
            
            var allBooks = db.BOOKs.ToList();

            // #2 LINQ Statement
            var bookCodes = db.WROTEs.Where(a => a.AUTHOR_NUM == authorNumberSelected).Select(id => id.BOOK_CODE);
            List<BOOK> booksToDisplay = new List<BOOK>();

            foreach (var code in bookCodes)
            {
                BOOK myBook = new BOOK();

                foreach (var book in allBooks)
                {
                    if (code == book.BOOK_CODE)
                    {
                        myBook.BOOK_CODE = book.BOOK_CODE;
                        myBook.PAPERBACK = book.PAPERBACK;
                        myBook.PRICE = book.PRICE;
                        myBook.PUBLISHER_CODE = book.PUBLISHER_CODE;
                        myBook.TYPE = book.TYPE;
                        myBook.TITLE = book.TITLE;
                    }
                }

                booksToDisplay.Add(myBook);
            }

            List<string> firstNames = new List<string>();
            string[] lastNames = new string[25];
            string[] authorNum = new string[25];
            int i = 0;

            foreach (var author in data)
            {
                firstNames.Add(author.AUTHOR_FIRST);
                lastNames[i] = author.AUTHOR_LAST;
                authorNum[i] = author.AUTHOR_NUM.ToString();
                i++;
            }

            ViewBag.firstNames = firstNames;
            ViewBag.lastNames = lastNames;
            ViewBag.authorNum = authorNum;
            ViewBag.booksToDisplay = booksToDisplay;
            return View();
        }

        public ActionResult Inventory()
        {
            // #1 LINQ statement
            var ids = db.BOOKs.Select(id => id.BOOK_CODE);
            List<String> listIDs = new List<string>();

            foreach (var id in ids)
            {
                listIDs.Add(id);
            }

            ViewBag.ids = listIDs;
            return View();
        }

        public ActionResult Book(string bookCode)
        {
            var optionSelected = bookCode;

            var allBooks = db.BOOKs.ToList();
            var wrotes = db.WROTEs.ToList();
            var allAuthors = db.AUTHORs.ToList();
            var allPublishers = db.PUBLISHERs.ToList();
            var inventory = db.INVENTORies.ToList();
            var allBranches = db.BRANCHes.ToList();

            List<InventoryBookDisplay> returnLocationDisplay = new List<InventoryBookDisplay>();

            BOOK returnBook = new BOOK();

            foreach (var book in allBooks)
            {
                if (book.BOOK_CODE == optionSelected)
                {
                    returnBook.TITLE = book.TITLE;
                    returnBook.TYPE = book.TYPE;
                    returnBook.PRICE = book.PRICE;
                    returnBook.PAPERBACK = book.PAPERBACK;
                    returnBook.BOOK_CODE = book.BOOK_CODE;
                    returnBook.PUBLISHER_CODE = book.PUBLISHER_CODE;
                }
            }

            foreach (var record in wrotes)
            {
                if (record.BOOK_CODE == optionSelected)
                {
                    foreach (var author in allAuthors)
                    {
                        if (author.AUTHOR_NUM == record.AUTHOR_NUM)
                        {
                            ViewBag.WrittenBy = author.AUTHOR_FIRST + " " + author.AUTHOR_LAST;             
                        }
                    }
                }
            }

            foreach (var publisher in allPublishers)
            {
                if (publisher.PUBLISHER_CODE == returnBook.PUBLISHER_CODE)
                {
                    ViewBag.Publisher = publisher.PUBLISHER_NAME;
                }
            }

            List<string> branchesIDs = new List<string>();

            foreach (var record in inventory)
            {
                if (record.BOOK_CODE == optionSelected)
                {
                    branchesIDs.Add(record.BRANCH_NUM.ToString());
                }
            }

            InventoryBookDisplay temporaBookDisplay = new InventoryBookDisplay();

            foreach (var branch in allBranches)
            {
                foreach (var element in branchesIDs)
                {
                    if (branch.BRANCH_NUM.ToString() == element)
                    {
                        temporaBookDisplay = new InventoryBookDisplay();

                        temporaBookDisplay.name = branch.BRANCH_NAME;

                        foreach (var record in inventory)
                        {
                            if (record.BRANCH_NUM.ToString() == element && record.BOOK_CODE == optionSelected)
                            {
                                
                                temporaBookDisplay.quantity = record.ON_HAND.ToString();
                                temporaBookDisplay.routeReference = record.BRANCH_NUM.ToString();
                                returnLocationDisplay.Add(temporaBookDisplay);
                            }
                        }
                    }
                }
            }

            ViewBag.returnBook = returnBook;
            ViewBag.returnLocationDisplay = returnLocationDisplay;

            return View();
        }
    }
}