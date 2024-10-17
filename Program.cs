
using System;
using System.Collections.Generic;
using System.Linq;

// Basklass, fordon
public abstract class Vehicle : IVehicle
{
    public string RegistrationNumber { get; private set; }
    public string Manufacturer { get; private set; }
    public string Color { get; private set; }
    public int NumberOfWheels { get; private set; }

    protected Vehicle(string registrationNumber, string manufacturer, string color, int numberOfWheels)
    {
        RegistrationNumber = registrationNumber;
        Manufacturer = manufacturer;
        Color = color;
        NumberOfWheels = numberOfWheels;
    }

    public abstract string GetVehicleInfo();
}

// Subklasser av Vehicle
public class Motorcycle : Vehicle
{
    public int EngineCapacity { get; private set; }

    public Motorcycle(string registrationNumber, string manufacturer, string color, int engineCapacity)
        : base(registrationNumber, manufacturer, color, 2)
    {
        EngineCapacity = engineCapacity;
    }

    public override string GetVehicleInfo()
    {
        return $"{Manufacturer} {Color} Motorcykel cylindervolym: {EngineCapacity} cc";
    }
}

public class Car : Vehicle
{
    public string FuelType { get; private set; }

    public Car(string registrationNumber, string manufacturer, string color, string fuelType)
        : base(registrationNumber, manufacturer, color, 4)
    {
        FuelType = fuelType;
    }

    public override string GetVehicleInfo()
    {
        return $"{Manufacturer} {Color} Bil (Bränsle: {FuelType})";
    }
}

public class Bus : Vehicle
{
    public int SeatCount { get; private set; }

    public Bus(string registrationNumber, string manufacturer, string color, int seatCount)
        : base(registrationNumber, manufacturer, color, 6)
    {
        SeatCount = seatCount;
    }

    public override string GetVehicleInfo()
    {
        return $"{Manufacturer} {Color} Buss (Platser: {SeatCount})";
    }
}

public class Boat : Vehicle
{
    public double Length { get; private set; }

    public Boat(string registrationNumber, string manufacturer, string color, double length)
        : base(registrationNumber, manufacturer, color, 0)
    {
        Length = length;
    }

    public override string GetVehicleInfo()
    {
        return $"{Manufacturer} {Color} Båt (Längd: {Length} m)";
    }
}

// GarageHandler
public class GarageHandler : IHandler
{
    private List<Garage<IVehicle>> garages;
    private Garage<IVehicle> currentGarage;

    public GarageHandler()
    {
        garages = new List<Garage<IVehicle>>();
    }

    public bool IsFull => currentGarage.IsFull;

    public void AddGarage(Garage<IVehicle> garage)
    {
        garages.Add(garage);
    }

    public bool ParkVehicle(IVehicle vehicle)
    {
          return  currentGarage.Park(vehicle);
       
    }

    public void ParkVehicle(IVehicle vehicle, Garage<IVehicle> garage)
    {
        throw new NotImplementedException();
    }

    public void RetrieveVehicle(string registrationNumber, Garage<IVehicle> garage)
    {
        if (!garage.Retrieve(registrationNumber))
        {
            Console.WriteLine("Fordonet hittades inte.");
        }
    }

    public void SearchVehicles(string criteria, Garage<IVehicle> garage)
    {
        var matches = garage.SearchVehicles(criteria);
        foreach (var vehicle in matches)
        {
            Console.WriteLine(vehicle.GetVehicleInfo());
        }
    }
}

// UI för garageapplikationen
public class GarageUI : IUI
{
    private GarageHandler garageHandler = new GarageHandler();
    private List<Garage<IVehicle>> garages = new List<Garage<IVehicle>>();

    public void ShowMenu()
    {
        InitializeGarage(); // Initiera garaget med fordon vid start

        while (true)
        {
            Console.WriteLine("Ange en bokstav för alternativ.");
            Console.WriteLine("Arbete med befintligt garage:  G");
            Console.WriteLine("Skapa ett nytt garage:         N");
            Console.WriteLine("Avsluta:                       X");

            string input = Console.ReadLine()?.ToUpper();

            switch (input)
            {
                case "G":
                    ManageExistingGarage();
                    break;
                case "N":
                    CreateNewGarage();
                    break;
                case "X":
                    Console.WriteLine("Programmet stängs.");
                    return; // Avsluta programmet
                default:
                    Console.WriteLine("Ogiltigt val! Försök igen.");
                    break;
            }
        }
    }

    private void InitializeGarage()
    {
        Garage<IVehicle> existingGarage = new Garage<IVehicle>(10);  // Anger 10 platser

        // Lägg till fordon av olika typer
        existingGarage.Park(new Motorcycle("MCR123", "Harley Davidson", "Svart", 1200));
        existingGarage.Park(new Motorcycle("MCR124", "Yamaha", "Vit", 600));
        existingGarage.Park(new Car("BIL123", "Volvo", "Svart", "Diesel"));
        existingGarage.Park(new Car("BIL124", "Audi", "Silver", "Bensin"));
        existingGarage.Park(new Bus("BUS123", "Scania", "Orange", 50));
        existingGarage.Park(new Bus("BUS124", "Mercedes", "Beige", 35));
        existingGarage.Park(new Boat("BAT123", "Bayliner", "Vit", 7));
        existingGarage.Park(new Boat("BAT124", "Yamaha", "Gul", 5));

        // Lägg garage till garageHandler och garages
        garageHandler.AddGarage(existingGarage);
        garages.Add(existingGarage);
    }

    private void ManageExistingGarage()
    {
        if (garages.Count == 0)
        {
            Console.WriteLine("Inga garage finns. Skapa ett nytt garage först.");
            return;
        }

        while (true)
        {
            Console.WriteLine("Välkommen! Du har följande alternativ:");
            Console.WriteLine("Visa antal fordon i garaget:         1");
            Console.WriteLine("Lägg till ett nytt fordon i garaget: 2");
            Console.WriteLine("Tag bort fordon som lämnat garaget:  3");
            Console.WriteLine("Söka efter fordon i garaget:         4");
            Console.WriteLine("Lista fordonstyper och antal:        5");
            Console.WriteLine("Återgå till huvudmenyn:              H");
            Console.WriteLine("Avsluta:                             X");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ShowVehicleCount();
                    break;
                case "2":
                    AddVehicleToGarage();
                    break;
                case "3":
                    RemoveVehicleFromGarage();
                    break;
                case "4":
                    SearchVehiclesInGarage();
                    break;
                case "5":
                    ListVehicleTypesInGarage(); // Visar fordonstyp
                    break;
                case "H":
                    return; // Återgå till huvudmenyn
                case "X":
                    Console.WriteLine("Programmet stängs.");
                    Environment.Exit(0); // Avsluta programmet
                    break;
                default:
                    Console.WriteLine("Ogiltigt val! Försök igen.");
                    break;
            }
        }
    }

    private void ListVehicleTypesInGarage()
    {
        Console.WriteLine("Välj garage (index: 0 till {0}):", garages.Count - 1);
        if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < garages.Count)
        {
            garages[index].ListVehicleTypes(); // Anropa den nya metoden för att lista fordonstyper
        }
        else
        {
            Console.WriteLine("Ogiltigt garageindex. Försök igen.");
        }
    }

    private void CreateNewGarage()
    {
        Console.WriteLine("Ange antal platser i det nya garaget:");
        if (int.TryParse(Console.ReadLine(), out int capacity) && capacity > 0)
        {
            Garage<IVehicle> newGarage = new Garage<IVehicle>(capacity);
            garages.Add(newGarage);
            garageHandler.AddGarage(newGarage);
            Console.WriteLine($"Ett garage med {capacity} platser har skapats.");
        }
        else
        {
            Console.WriteLine("Ogiltig input. Försök igen.");
        }
    }

    private void ShowVehicleCount()
    {
        Console.WriteLine("Välj garage (index: 0 till {0}):", garages.Count - 1);
        if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < garages.Count)
        {
            var count = garages[index].Count;
            Console.WriteLine($"Antal fordon i garage {index + 1}: {count}");
        }
        else
        {
            Console.WriteLine("Ogiltigt index. Försök igen.");
        }
    }

    private void AddVehicleToGarage()
    {
        if (garageHandler.IsFull)
        {
            Console.WriteLine("garaget är fullt");
            return;
        }

        Console.WriteLine("Ange registreringsnummer:");
        string regNumber = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(regNumber))
        {
            Console.WriteLine("Ogiltigt registreringsnummer. Försök igen.");
            return;
        }

        Console.WriteLine("Ange tillverkare:");
        string manufacturer = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(manufacturer))
        {
            Console.WriteLine("Ogiltig tillverkare. Försök igen.");
            return;
        }

        Console.WriteLine("Ange färg:");
        string color = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(color))
        {
            Console.WriteLine("Ogiltig färg. Försök igen.");
            return;
        }

        Console.WriteLine("Ange typ av fordon (motorcykel, bil, buss, båt):");
        string vehicleType = Console.ReadLine()?.ToLower();

        IVehicle vehicle = null;

        switch (vehicleType)
        {
            case "motorcykel":
                Console.WriteLine("Ange motorcykelns cylindervolym:");
                if (int.TryParse(Console.ReadLine(), out int engineCapacity))
                {
                    vehicle = new Motorcycle(regNumber, manufacturer, color, engineCapacity);
                }
                break;
            case "bil":
                Console.WriteLine("Ange bränsletyp:");
                string fuelType = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(fuelType))
                {
                    vehicle = new Car(regNumber, manufacturer, color, fuelType);
                }
                break;
            case "buss":
                Console.WriteLine("Ange antal säten:");
                if (int.TryParse(Console.ReadLine(), out int seatCount))
                {
                    vehicle = new Bus(regNumber, manufacturer, color, seatCount);
                }
                break;
            case "båt":
                Console.WriteLine("Ange båtens längd:");
                if (double.TryParse(Console.ReadLine(), out double length))
                {
                    vehicle = new Boat(regNumber, manufacturer, color, length);
                }
                break;
            default:
                Console.WriteLine("Ogiltig fordonstyp. Försök igen.");
                return;
        }

        if (vehicle != null)
        {
            Console.WriteLine("Välj garage att parkera fordonet i (index: 0 till {0}):", garages.Count - 1);
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < garages.Count)
            {
               if( garageHandler.ParkVehicle(vehicle))
                Console.WriteLine("Fordonet har lagts till i garaget.");
                else
                {

                }
            }
            else
            {
                Console.WriteLine("Ogiltigt garageindex. Försök igen.");
            }
        }
    }

    private void RemoveVehicleFromGarage()
    {
        Console.WriteLine("Ange registreringsnummer på fordonet som ska tas bort:");
        string regNumber = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(regNumber))
        {
            Console.WriteLine("Ogiltigt registreringsnummer. Försök igen.");
            return;
        }

        Console.WriteLine("Välj garage (index: 0 till {0}):", garages.Count - 1);
        if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < garages.Count)
        {
            garageHandler.RetrieveVehicle(regNumber, garages[index]);
        }
        else
        {
            Console.WriteLine("Ogiltigt garageindex. Försök igen.");
        }
    }

    private void SearchVehiclesInGarage()
    {
        Console.WriteLine("Vill du söka efter reg.nr. (skriv '1') eller attribut (skriv '2')?");
        string choice = Console.ReadLine();

        Console.WriteLine("Välj garage (index: 0 till {0}):", garages.Count - 1);
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 0 || index >= garages.Count)
        {
            Console.WriteLine("Ogiltigt garageindex. Försök igen.");
            return;
        }

        if (choice == "1")
        {
            Console.WriteLine("Ange reg.nr.");
            string regNumber = Console.ReadLine();
            var vehicles = garages[index].Where(v =>
                v.RegistrationNumber.Equals(regNumber, StringComparison.OrdinalIgnoreCase));
            foreach (var vehicle in vehicles)
            {
                Console.WriteLine(vehicle.GetVehicleInfo());
            }
        }
        else if (choice == "2")
        {
            Console.WriteLine("Separera sökorden med kommatecken (t ex tillverkare, färg, reg.nr.):");
            string input = Console.ReadLine();
            string[] criteria = input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim()).ToArray();

            var vehicles = garages[index].GetAllVehicles().Where(v =>
                criteria.Any(c =>
                    v.Manufacturer.IndexOf(c, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    v.Color.IndexOf(c, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    v.RegistrationNumber.IndexOf(c, StringComparison.OrdinalIgnoreCase) >= 0));

            foreach (var vehicle in vehicles)
            {
                Console.WriteLine(vehicle.GetVehicleInfo());
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt val. Försök igen.");
        }
    }
}

// Program
class Program
{
    static void Main(string[] args)
    {
        GarageUI garageUI = new GarageUI();
        garageUI.ShowMenu();
    }
}
