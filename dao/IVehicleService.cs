using Car_Connect.entity_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.dao
{
    public interface IVehicleService
    {
        Vehicle GetVehicleById(int vehicleId);
        List<Vehicle> GetAvailableVehicles();
        List<Vehicle> GetAllVehicles();
        void AddVehicle(Vehicle vehicleData);
        void UpdateVehicle(Vehicle vehicleData);
        void RemoveVehicle(int vehicleId);
    }
}
