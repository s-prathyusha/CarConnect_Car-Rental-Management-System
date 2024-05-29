using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.exception
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message)
        {
        }
    }

    public class ReservationException : Exception
    {
        public ReservationException(string message) : base(message)
        {
        }
    }

    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException(string message) : base(message)
        {
        }
    }

    public class AdminNotFoundException : Exception
    {
        public AdminNotFoundException(string message) : base(message)
        {
        }
    }

    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : base(message)
        {
        }
    }

    public class DatabaseConnectionException : Exception
    {
        public DatabaseConnectionException(string message) : base(message)
        {
        }
    }

    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(string message) : base(message)
        {

        }
    }
}
