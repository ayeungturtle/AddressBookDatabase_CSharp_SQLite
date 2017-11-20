using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookACK
{
    class Program
    {
        static void Main(string[] args)
        {
            //create connection to database
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=c:\users\andrew\source\repos\PhoneBookACK\PhoneBookACK\Database1.mdf;Integrated Security=True");
            connection.Open();
            //Typing out menu of choices
            while (true)
            {
                Console.WriteLine("Would you like to 1) add a contact 2) update a contact 3) delete a contact 4) view the entire phonebook 5) search for a contact or 6) exit.  Please type a number 1-6.");
                string userResponse = Console.ReadLine();

                if (userResponse == "1")
                {
                    //adding contact   *** tested and confirmed functional
                    Console.WriteLine("First name?");
                    string userFirstName = Console.ReadLine();
                    Console.WriteLine("Last name?");
                    string userLastName = Console.ReadLine();
                    Console.WriteLine("Phone number?");
                    string userPhoneNumber = Console.ReadLine();
                    Console.WriteLine("Email?");
                    string userEmail = Console.ReadLine();

                    SqlCommand addContact = new SqlCommand($"INSERT INTO PhoneBook (firstName,lastName,phoneNumber,EMAIL) VALUES ('{userFirstName}', '{userLastName}', '{userPhoneNumber}', '{userEmail}')", connection);
                    addContact.ExecuteNonQuery();
                }

                if (userResponse == "2")
                {
                    //updating contact
                    string userAnswer = "u";
                    while (userAnswer == "u")
                    { 
                        Console.WriteLine("Enter the ID of the contact to update. If unsure, press u.");
                        userAnswer = Console.ReadLine();
                        if (userAnswer == "u")
                        {
                            Search(connection);
                        }
                    }
                    Console.WriteLine("What is the new phone number?");
                    string newNumber = Console.ReadLine();

                    SqlCommand updateNumber = new SqlCommand($"UPDATE phoneBook SET phoneNumber = '{newNumber}' WHERE ID = '{userAnswer}'", connection);
                    updateNumber.ExecuteNonQuery();
                }

                if (userResponse == "3")
                {
                    //deleting contact
                    string userAnswer = "u";
                    while (userAnswer == "u")
                    {
                        Console.WriteLine("Enter the ID of the contact to delete. If unsure of ID, press u.");
                        userAnswer = Console.ReadLine();
                        if (userAnswer == "u")
                        {
                            Search(connection);
                        }
                    }
                    SqlCommand deleteEntry = new SqlCommand($"DELETE FROM phoneBook WHERE id = '{userAnswer}'", connection);
                    deleteEntry.ExecuteNonQuery();
                }

                if (userResponse == "4")
                {
                    //show entire phonebook
                    SqlCommand returnAll = new SqlCommand("SELect * frOM pHONEBOOK", connection);
                    SqlDataReader everything = returnAll.ExecuteReader();
                    if (everything.HasRows)
                    {
                        while(everything.Read())
                        {
                            Console.WriteLine($"ID: { everything["ID"]} \t { everything["firstName"]} {everything["LastName"]} \t {everything["phoneNumber"]} \t {everything["email"]}");
                        }
                    }
                    everything.Close();
                }

                if (userResponse == "5")
                {
                    //search phonebook
                    Search(connection);
                }
                if (userResponse == "6")
                {
                    break;
                }
            }
            connection.Close();
        }

        public static void Search(SqlConnection connection)
        {
            Console.WriteLine("What is the last name of the contact you would like to retrieve?  If unsure of last name, type the first letters.");
            string userSearch = Console.ReadLine();

            SqlCommand search = new SqlCommand($"SELECT * FROM phoneBook WHERE lastName like '{userSearch}%'", connection);
            SqlDataReader searchResult = search.ExecuteReader();

            if (searchResult.HasRows)
            {
                while (searchResult.Read())
                {
                    Console.WriteLine($"ID: {searchResult["ID"]} ---- {searchResult["firstName"]}" +
                        $" {searchResult["LastName"]}, {searchResult["phoneNumber"]}, {searchResult["email"]}");
                }
            }
            searchResult.Close();
        }
    }
}
