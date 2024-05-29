using Car_Connect.entity_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.dao
{
    internal interface IAdminService
    {
        Admin GetAdminById(int adminId);
        Admin GetAdminByUsername(string username);
        void RegisterAdmin(Admin adminData);
        void UpdateAdmin(Admin adminData);
        void DeleteAdmin(int adminId);
    }
}
