using System.Collections;
// Garage
public class Garage<T> : IEnumerable<T> where T : IVehicle
{
    private T[] vehicles; // Samlar fordon i array
    private int capacity;
    private int currentCount;

    public bool IsFull => currentCount >= capacity;

    public Garage(int capacity)
    {
       // if (capacity <= 1) throw new ArgumentException(nameof(capacity));
        this.capacity = capacity;
        vehicles = new T[capacity]; // Skapa array av typen T
        currentCount = 0;
    }

    public bool Park(T vehicle)
    {
        if (IsFull)
            return false;

      

        vehicles[currentCount++] = vehicle; // Parkera och öka index
        return true;
    }

    public bool Retrieve(string registrationNumber)
    {
        var vehicle = vehicles.FirstOrDefault(v => v != null && v.RegistrationNumber.Equals(registrationNumber, StringComparison.OrdinalIgnoreCase));
        if (vehicle != null)
        {
            vehicles[Array.IndexOf(vehicles, vehicle)] = default; // Sätt default i array
            currentCount--;
            return true;
        }
        return false;
    }

    public IEnumerable<T> GetAllVehicles()
    {
        return vehicles.Where(v => v != null); // Retur, parkerade fordon
    }

    public IEnumerable<T> SearchVehicles(string color, string type, int? numberofwheels)
    {
        IEnumerable<T> result = vehicles.Where(v => v != null);

        if (!string.IsNullOrEmpty(color))
        {
            result = result.Where(v => v.Color == color);
        } 
        
        if (!string.IsNullOrEmpty(type))
        {
            result = result.Where(v => v.GetType().Name == type);
        }  
        
        if (numberofwheels.HasValue)
        {
            result = result.Where(v => v.NumberOfWheels == numberofwheels);
        }

        return result.ToList();


        //return vehicles.Where(v => v != null && (
        //    v.Manufacturer.Contains(criteria, StringComparison.OrdinalIgnoreCase) ||
        //    v.Color.IndexOf(criteria, StringComparison.OrdinalIgnoreCase) >= 0 ||
        //    v.RegistrationNumber.IndexOf(criteria, StringComparison.OrdinalIgnoreCase) >= 0));
    }

    public void ListVehicleTypes()
    {
        var vehicleCounts = new Dictionary<string, int>();

        // Räkna fordon av varje typ
        foreach (var vehicle in vehicles)
        {
            if (vehicle != null)
            {
                string vehicleType = vehicle.GetType().Name; // Hämtar typer
                if (vehicleCounts.ContainsKey(vehicleType))
                {
                    vehicleCounts[vehicleType]++;
                }
                else
                {
                    vehicleCounts[vehicleType] = 1;
                }
            }
        }

        // Skriv ut antalet fordon av varje typ
        Console.WriteLine("Fordonstyper och antal:");
        foreach (var kvp in vehicleCounts)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < currentCount; i++)
        {
            yield return vehicles[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => currentCount; // Retur, antal fordon
}
