using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class PublisherController : Controller
    {
        private HENRYEntities db = new HENRYEntities();
        // GET: Publisher
        public ActionResult Publisher()
        {
            // #4 LINQ Statement
            var data = from p in db.PUBLISHERs select new {p.PUBLISHER_CODE, p.PUBLISHER_NAME};
            int size = 0;
            int counter = 0;

            foreach (var publisher in data)
            {
                size++;
            }

            string[] publisherNames = new string[size];
            string[] publisherCodes = new string[size];
            
            foreach (var publisher in data)
            {
                publisherNames[counter] = publisher.PUBLISHER_NAME;
                publisherCodes[counter] = publisher.PUBLISHER_CODE;
                counter++;
            }

            ViewBag.publisherNames = publisherNames;
            ViewBag.publisherCodes = publisherCodes;
            return View();
        }

        [HttpPost]
        public ActionResult ByPublisherDisplay(OptionSelected model)
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

            var allBooks = db.BOOKs.ToList();

            List<BOOK> booksToDisplay = new List<BOOK>();

            foreach (var book in allBooks)
            {
                BOOK myBook = new BOOK();

                if (model.optionSelected == book.PUBLISHER_CODE)
                {
                    myBook.BOOK_CODE = book.BOOK_CODE;
                    myBook.PAPERBACK = book.PAPERBACK;
                    myBook.PRICE = book.PRICE;
                    myBook.TYPE = book.TYPE;
                    myBook.TITLE = book.TITLE;
                }

                booksToDisplay.Add(myBook);
            }

            // #5 LINQ Statement
            var data = from p in db.PUBLISHERs select new {p.PUBLISHER_CODE, p.PUBLISHER_NAME};
            int size = 0;
            int counter = 0;

            foreach (var publisher in data)
            {
                size++;
            }

            string[] publisherNames = new string[size];
            string[] publisherCodes = new string[size];

            foreach (var publisher in data)
            {
                publisherNames[counter] = publisher.PUBLISHER_NAME;
                publisherCodes[counter] = publisher.PUBLISHER_CODE;
                counter++;
            }

            ViewBag.publisherNames = publisherNames;
            ViewBag.publisherCodes = publisherCodes;
            ViewBag.booksToDisplay = booksToDisplay;
            return View();
        }
    }
}