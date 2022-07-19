using GestiuneBiblioteca;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace GestiuneBiblioteca_Tests
{
    [TestClass]
    public class Bootstrapper_UnitTest
    {
        private Bootstrapper bootstrapper;
        private static Book book;

        [ClassInitialize]
        public static void ClassSetUp(TestContext context)
        {
            book = new Book("test", "test", 1);
        }

        [TestInitialize]
        public void TestSetUp()
        {
            bootstrapper = new Bootstrapper();
        }

        [TestMethod]
        public void AddBook_WhenCalled_AddsObjectToList()
        {           
            bootstrapper.AddBook("a", "a", 1);

            Assert.AreEqual(1, bootstrapper.availableBooks.Count);
        }

        [TestMethod]
        public void GetAllBooks_WhenCalled_ReturnsTrue()
        {         
            bootstrapper.availableBooks.Add(book);

            bool booksAreAvailable = bootstrapper.GetAllBooks(bootstrapper.availableBooks);

            Assert.AreEqual(true, booksAreAvailable);
        }

        [TestMethod]
        public void GetAllBooks_WhenCalled_ReturnsFalse()
        {
            bool booksAreAvailable = bootstrapper.GetAllBooks(bootstrapper.availableBooks);

            Assert.AreEqual(false, booksAreAvailable);
        }

        [TestMethod]
        public void GetNumberOfCopies_WhenCalled_ReturnsCode400()
        {
            var result = bootstrapper.GetNumberOfCopies("0");

            Assert.AreEqual(400, result.Code);
        }

        [TestMethod]
        public void GetNumberOfCopies_WhenCalled_ReturnsCode200()
        {
            
            bootstrapper.availableBooks.Add(book);

            var result = bootstrapper.GetNumberOfCopies("0");

            Assert.AreEqual(200, result.Code);
            Assert.AreEqual("Number of copies by edition: 1 \nNumber of copies by name: 1", result.Message);
        }

        [TestMethod]
        public void BorrowBook_WhenCalled_AddsBookToBorrowedList()
        {
            bootstrapper.availableBooks.Add(book);

            var result = bootstrapper.BorrowBook("0", "test");

            Assert.AreEqual(1, bootstrapper.borrowedBooks.Count);
        }

        [TestMethod]
        public void BorrowBook_WhenCalled_RemovesBookFromAvailableList()
        {
            bootstrapper.availableBooks.Add(book);

            var result = bootstrapper.BorrowBook("0", "test");

            Assert.AreEqual(0, bootstrapper.availableBooks.Count);
        }

        [TestMethod]
        public void BorrowBook_WhenCalled_Returns200()
        {
            bootstrapper.availableBooks.Add(book);

            var result = bootstrapper.BorrowBook("0", "test");

            Assert.AreEqual(200, result.Code);
            Assert.AreEqual("The book test is now borrowed", result.Message);
        }

        [TestMethod]
        public void BorrowBook_WhenCalled_ReturnsCode400()
        { 
            var result = bootstrapper.BorrowBook("1", "test");

            Assert.AreEqual(400, result.Code);
        }

        [TestMethod]
        public void ReturnBook_WhenCalled_AddsBookToAvailableList()
        {
            bootstrapper.borrowedBooks.Add(book);

            var result = bootstrapper.ReturnBook("0");

            Assert.AreEqual(1, bootstrapper.availableBooks.Count);
        }

        [TestMethod]
        public void ReturnBook_WhenCalled_RemovesBookFromBorrowedList()
        {
            bootstrapper.borrowedBooks.Add(book);

            var result = bootstrapper.ReturnBook("0");

            Assert.AreEqual(0, bootstrapper.borrowedBooks.Count);
        }

        [TestMethod]
        public void ReturnBook_WhenCalled_Returns200()
        {
            bootstrapper.borrowedBooks.Add(book);

            var result = bootstrapper.ReturnBook("0");

            Assert.AreEqual(200, result.Code);
        }

        [TestMethod]
        public void ReturnBook_WhenCalled_ReturnsCode400()
        {
            var result = bootstrapper.ReturnBook("1");

            Assert.AreEqual(400, result.Code);
        }

        [TestMethod]
        public void CalculateTotalPrice_WhenCalled_ReturnsWithPenalty()
        {
            book.BorrowTime = DateTime.Now.AddSeconds(-30);
            var result = bootstrapper.CalculateTotalPrice(book.BorrowTime, 10);

            Assert.AreEqual(150.1, result);
        }

        [TestMethod]
        public void CalculateTotalPrice_WhenCalled_ReturnsWithoutPenalty()
        {
            book.BorrowTime = DateTime.Now.AddSeconds(-28);
            var result = bootstrapper.CalculateTotalPrice(book.BorrowTime, 10);

            Assert.AreEqual(140, result);
        }
    }
}