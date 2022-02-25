using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PatientRecordApplication
{
    /// <summary>
    /// Class that represents a patient. Holds fields PatientID, Name, and Balance. Loads this data into the patientRecords file
    /// </summary>
    public class Patient
    {
        private int patientID;
        private string name;
        private decimal balance;

        public int PatientID
        {
            set
            {
                patientID = value;
            }

            get
            {
                return patientID;
            }
        }

        public string Name
        {
            set
            {
                name = value;
            }

            get
            {
                return name;
            }
        }

        public decimal Balance
        {
            set
            {
                balance = value;
            }

            get
            {
                return balance;
            }
        }

        public void enterInfo()
        {
               int numPatients;
               Console.Write("How many patients would you like to add: ");
               numPatients = int.Parse(Console.ReadLine());
               Patient[] patientObj = new Patient[numPatients];
               int i = 0;
               while (i < numPatients)
               {
                patientObj[i] = new Patient();
                   Console.Write($"Enter info for patient {i + 1}: ");
                   Console.Write("\nEnter patient name: ");
                   patientObj[i].Name = Console.ReadLine();
                   Console.Write("Enter patient ID: ");
                   patientObj[i].PatientID = int.Parse(Console.ReadLine());
                   Console.Write("Enter patient balance: ");
                   patientObj[i].Balance = decimal.Parse(Console.ReadLine());
                   ++i;
                   Console.Write("\n\n");
               }

            writeToFile(patientObj, numPatients);
        }

        public void writeToFile(Patient[] patientArr, int numPatients)
        {
             FileStream fstream = new FileStream("PatientRecord.txt", FileMode.Create, FileAccess.Write);
             StreamWriter streamWriter = new StreamWriter(fstream);
             int i = 0;
             while (i < numPatients)
                {
                    streamWriter.WriteLine(patientArr[i].PatientID + "," + patientArr[i].Name + "," + patientArr[i].Balance);
                    i++;
                }

             streamWriter.Close();
             fstream.Close();
                    
        }
    }

    /// <summary>
    /// Class that reads the data from the patientRecords.txt file and displays the data to the screen
    /// </summary>
    public class DisplayInfo
    {
        public void displayRecords()
        {
            bool continueLoop = true;
            do
            {
                try
                {
                    FileStream infile = new FileStream("PatientRecord.txt", FileMode.Open, FileAccess.Read);
                    StreamReader streamReader = new StreamReader(infile);
                    string line;
                    string[] splitLine;

                    line = streamReader.ReadLine();
                    while (line != null)
                    {
                        splitLine = line.Split(',');
                        Console.WriteLine($"Patient ID: {splitLine[0]}");
                        Console.WriteLine($"Patient Name: {splitLine[1]}");
                        Console.WriteLine($"Patient Balance: {splitLine[2]:C}\n");
                        line = streamReader.ReadLine();
                    }

                    streamReader.Close();
                    infile.Close();
                    continueLoop = false;
                }

                catch (FileNotFoundException fileNotFoundException)
                {
                    Console.WriteLine(fileNotFoundException.Message);
                    Console.WriteLine("The file you are trying to access does not exist");
                }

                finally
                {
                    // file closing cannot be moved here because the file needs to be declared within the try block because that is what we are 
                    // "trying". Variables declared within the try block cannot be accessed in the finally block.
                }
            } while (continueLoop);
        }
    }

    /// <summary>
    /// Class that allows the user to search a patient by ID number and displays the patient's record to the screen
    /// </summary>
    public class SearchByID
    {
        public void searchByIDNum()
        {
            FileStream infile = new FileStream("PatientRecord.txt", FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(infile);
            string line;
            string[] splitLine;
            string ID;

            Console.Write("Enter an ID number of a patient: ");
            ID = Console.ReadLine();
            bool found = false;

           
            line = streamReader.ReadLine();

            while (line != null)
            {

                splitLine = line.Split(',');
                if (splitLine[0] == ID)
                {
                    found = true;
                    Console.WriteLine($"Patient ID: {ID}");
                    Console.WriteLine($"Patient Name: {splitLine[1]}");
                    Console.WriteLine($"Patient Balance: {splitLine[2]:C}");
                    goto AfterLoop;
                }

                line = streamReader.ReadLine();

            }

            if (found == false)
            {
                Console.WriteLine($"No record with ID {ID} was found");
            }

          AfterLoop:
          Console.WriteLine();
        }
    }

    /// <summary>
    /// Class that allows the user to filter records by balance. Displays all records with a balance less than or equal to a user specified amount
    /// </summary>
    public class FindByMinBalance
    {
        public void MinBalance()
        {

            bool continueLoop = true;
            decimal minBalance;
            do
            {
                try
                {
                    //decimal minBalance;
                    Console.Write("Enter a minimum balance due: ");
                    minBalance = decimal.Parse(Console.ReadLine());
                    if (minBalance < 0)
                    {
                        throw new NegNumberException();
                    }
                    FileStream infile = new FileStream("PatientRecord.txt", FileMode.Open, FileAccess.Read);
                    StreamReader streamReader = new StreamReader(infile);
                    string line;
                    string[] splitLine;
                    bool found = false; 

                    Console.WriteLine($"\nBalances below {minBalance}: ");
                    line = streamReader.ReadLine();
                    while (line != null)
                    {
                        splitLine = line.Split(',');
                        if (Convert.ToDecimal(splitLine[2]) <= minBalance)
                        {
                            found = true;
                            Console.WriteLine($"Record for patient \"{splitLine[1]}\": ");
                            Console.WriteLine($"\nPatient ID: {splitLine[0]}");
                            Console.WriteLine($"Patient Name: {splitLine[1]}");
                            Console.WriteLine($"Patient Balance: {splitLine[2]:C}\n");
                        }

                        line = streamReader.ReadLine();
                    }

                    if (!found)
                    {
                        Console.WriteLine($"No records with a balance below {minBalance}");
                    }

                    continueLoop = false;
                }

                catch (FormatException formatException)
                {
                    Console.WriteLine($"\n{formatException.Message}");
                    Console.WriteLine("You must enter a decimal value. Try again");
                }

                catch (NegNumberException negNumExc)
                {
                    Console.WriteLine(negNumExc.Message);
                    Console.WriteLine("The balance cannot be negative");
                }

                finally
                {
                    minBalance = 0;
                    // nothing needs to be reallocated here
                }

            } while (continueLoop);

        }
    }

    /// <summary>
    /// User defined exception to ensure no negative numbers are entered for balances.
    /// </summary>
    public class NegNumberException : Exception
    {
        public NegNumberException() : base("Need's positive number")
        {

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Patient patient = new Patient();
            DisplayInfo display = new DisplayInfo();
            SearchByID searchID = new SearchByID();
            FindByMinBalance searchByBalance = new FindByMinBalance();
            patient.enterInfo();
            display.displayRecords();
            searchID.searchByIDNum();
            searchByBalance.MinBalance();
            Console.Write("Press any key to exit");
            Console.ReadLine();
        }
    }
}
