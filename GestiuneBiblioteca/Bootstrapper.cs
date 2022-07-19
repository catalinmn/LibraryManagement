
namespace GestiuneBiblioteca
{
    public class Bootstrapper
    {
        public List<Book> availableBooks;
        public List<Book> borrowedBooks;
        const int penaltyDays = 14;

        public Bootstrapper()
        {
            this.availableBooks = new();
            this.borrowedBooks = new(); 
        }

        public void AddBook(string name, string isbn, int price)
        {
            availableBooks.Add(new Book(name, isbn, price));
        }

        public bool GetAllBooks(List<Book> books)
        {
            if (books.Count == 0)
            {
                Console.WriteLine("You can't perform this actions because there are no books.");
                return false;
            }

            books.ForEach(book => Console.WriteLine($"Id: {book.Id}, Name: {book.Name}, ISBN: {book.ISBN}, Price: {book.Price}, Last Customer's Name: {book.BorrowerName}"));

            return true;
        }

        public ResponseObject GetNumberOfCopies(string id)
        {
            ResponseObject responseObject;

            try
            {
                Book book = availableBooks.Find(book => book.Id == Convert.ToInt32(id));

                if(book == null)
                {
                    responseObject = new ResponseObject()
                    {
                        Code = 400,
                        Message = $"The book doesn't exists or it is not available at this time"
                    };

                    return responseObject;  
                }

                int noOfCopiesByEdition = availableBooks.FindAll(x => x.ISBN == book.ISBN).Count();

                //aici putem presupune faptul ca ne intereseaza o anumita carte, necontand editia (care este data de codul ISBN)
                int noOfCopiesByBookName = availableBooks.FindAll(x => x.Name == book.Name).Count();

                responseObject = new ResponseObject()
                {
                    Code = 200,
                    Message = $"Number of copies by edition: {noOfCopiesByEdition} \nNumber of copies by name: {noOfCopiesByBookName}"
                };

                return responseObject;
            }
            catch (Exception ex)
            {
                responseObject = new ResponseObject()
                {
                    Code = 500,
                    Message = ex.Message
                };

                return responseObject;
            }        
        }

        public ResponseObject BorrowBook(string id, string borrowerName)
        { 
            ResponseObject responseObject;

            try
            {
                Book book = availableBooks.Find(x => x.Id == Convert.ToInt32(id));
                if (book == null)
                {
                    responseObject = new ResponseObject()
                    {
                        Code = 400,
                        Message = "The book doesn't exists or it is not available at this time"
                    };

                    return responseObject;
                }

                book.BorrowerName = borrowerName;

                book.BorrowTime = DateTime.Now;

                borrowedBooks.Add(book);
                availableBooks.Remove(book);

                responseObject = new ResponseObject()
                {
                    Code = 200,
                    Message = $"The book {book.Name} is now borrowed"
                };

                return responseObject;
            }
            catch (Exception ex)
            {
                responseObject = new ResponseObject()
                {
                    Code = 500,
                    Message = ex.Message
                };

                return responseObject;
            }
        }

        public ResponseObject ReturnBook(string id)
        {         
            ResponseObject responseObject;

            try
            {
                Book book = borrowedBooks.Find(x => x.Id == Convert.ToInt32(id));
                if (book == null)
                {
                    responseObject = new ResponseObject()
                    {
                        Code = 400,
                        Message = "The book is not in the list of borrowed books"
                    };

                    return responseObject;
                }

                double totalPrice = CalculateTotalPrice(book.BorrowTime, book.Price);

                availableBooks.Add(book);
                borrowedBooks.Remove(book);

                responseObject = new ResponseObject()
                {
                    Code = 200,
                    Message = $"\nTotal price: {totalPrice}"
                };

                return responseObject;
            }
            catch (Exception ex)
            {
                responseObject = new ResponseObject()
                {
                    Code = 500,
                    Message = ex.Message
                };

                return responseObject;
            }
        }

        public double CalculateTotalPrice(DateTime borrowTime, int bookPrice)
        {
            //consideram faptul ca fiecare 2 secunde reprezinta o zi
            int days = (DateTime.Now - borrowTime).Seconds / 2;

            double totalPrice;

            if (days > penaltyDays)
                totalPrice = bookPrice * days + 0.01 * (days - penaltyDays) * bookPrice;
            else
                totalPrice = bookPrice * days;

            return totalPrice;
        }
    }
}
