using GestiuneBiblioteca;

var bootstrapper = new Bootstrapper();

while (true)
{
    Console.WriteLine("\nPlease select one of the following options");
    Console.WriteLine("1. Add a book");
    Console.WriteLine("2. Get the list of all available books");
    Console.WriteLine("3. Get the number of available copies of a book");
    Console.WriteLine("4. Borrow a book");
    Console.WriteLine("5. Return a book");
    Console.WriteLine("6. Exit app");

    Console.Write("Enter your input: ");

    string option = null;
    option = ValidateInput(option);

    bool output;
    ResponseObject response;
    string id;

    switch (option)
    {
        case "1":
            Console.Clear();
            Console.WriteLine("\nInsert new book");

            string name = null;
            string isbn = null;
            string price = null;

            Console.Write("Name:");
            name = ValidateInput(name);

            Console.Write("ISBN:");
            isbn = ValidateInput(isbn);

            Console.Write("Price:");
            price = ValidateInput(price);
            int result = int.TryParse(price, out result) ? result : 0;

            if (result == 0)
            {
                while (result == 0)
                {
                    price = null;
                    Console.Write("Please enter a valid price: ");
                    price = ValidateInput(price);
                    result = int.TryParse(price, out result) ? result : 0;
                }
            }

            bootstrapper.AddBook(name, isbn, Convert.ToInt32(result));
            break;
        case "2":
            Console.Clear();
            bootstrapper.GetAllBooks(bootstrapper.availableBooks);
            break;
        case "3":
            Console.Clear();
            output = bootstrapper.GetAllBooks(bootstrapper.availableBooks);
            if (!output)
                break;

            Console.Write("\nPlease enter the id of a book in order to get the number of available copies:");
            id = null;
            id = ValidateInput(id);

            response = bootstrapper.GetNumberOfCopies(id);
            Console.WriteLine(response.Message);

            break;
        case "4":
            Console.Clear();
            output = bootstrapper.GetAllBooks(bootstrapper.availableBooks);
            if (!output)
                break;

            Console.Write("\nPlease enter the id of book to borrow: ");
            id = null;
            id = ValidateInput(id);

            Console.Write("Please enter customer's name: ");
            string borrowerName = null;
            borrowerName = ValidateInput(borrowerName);

            response = bootstrapper.BorrowBook(id, borrowerName);
            Console.WriteLine(response.Message);

            break;
        case "5":
            Console.Clear();
            output = bootstrapper.GetAllBooks(bootstrapper.borrowedBooks);
            if (!output)
                break;

            Console.Write("\nPlease enter the id of book to return:");
            id = null;
            id = ValidateInput(id);

            response = bootstrapper.ReturnBook(id);
            Console.WriteLine(response.Message);

            break;
        case "6":
            Environment.Exit(0);
            break;
        default:
            Console.Clear();
            Console.WriteLine("Please select a valid option");
            break;
    }
}

string ValidateInput(string input)
{

    while (String.IsNullOrEmpty(input))
    {
        input = Console.ReadLine();

        if (!String.IsNullOrEmpty(input))
            return input;

        Console.Write("Please enter a valid input:");
    }

    return input;
}



