
// Interfaces
public interface IVehicle
{
    string RegistrationNumber { get; }
    string Manufacturer { get; }
    string Color { get; }
    int NumberOfWheels { get; }
    string GetVehicleInfo();
}
