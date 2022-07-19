using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestiuneBiblioteca
{
    public class Book
    {
        static int idIndex = 0;
        public int Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public int Price { get; set; }
        public DateTime BorrowTime { get; set; }
        public string BorrowerName { get; set; }

        public Book(string name, string iSBN, int price)
        {
            Id = idIndex ++;
            Name = name;
            ISBN = iSBN;
            Price = price;
            BorrowerName = "None yet";
        }

    }
}
