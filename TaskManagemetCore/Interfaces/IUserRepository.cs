using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Models;

namespace TaskManagemetCore.Interfaces
{
    public interface IUserRepository
    {
        User? GetByUsername(string username);

        User? GetById(int id);

        List<User> GetAll();

        void Add(User user);

        void Update(User user);
        object GetByEmail(string email);
    }
}
