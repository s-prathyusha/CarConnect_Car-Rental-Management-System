using Car_Connect.entity_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.dao
{
    internal interface IReservationService
    {
        Reservation GetReservationById(int reservationId);
        List<Reservation> GetReservationsByCustomerId(int customerId);
        void CreateReservation(Reservation reservationData);
        void UpdateReservation(Reservation reservationData);
        void CancelReservation(int reservationId);
    }
}
