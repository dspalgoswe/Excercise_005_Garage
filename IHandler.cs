public interface IHandler
{
    void AddGarage(Garage<IVehicle> garage);
    void ParkVehicle(IVehicle vehicle, Garage<IVehicle> garage);
    void RetrieveVehicle(string registrationNumber, Garage<IVehicle> garage);
    void SearchVehicles(string criteria, Garage<IVehicle> garage);
}
