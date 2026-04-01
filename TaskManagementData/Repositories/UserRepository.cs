using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementData.Contexts;
using TaskManagemetCore.Interfaces;
using TaskManagemetCore.Models;

namespace TaskManagementData.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User? GetByUsername(string username)
        {
            return _context.Users
                .FirstOrDefault(u => u.Username == username);
        }

        public User? GetById(int id)
        {
            return _context.Users
                .FirstOrDefault(u => u.Id == id);
        }

        public User? GetByEmail(string email)
        {
            return _context.Users
                .FirstOrDefault(u => u.Email == email);
        }

        public List<User> GetAll()
        {
            return _context.Users
                .AsNoTracking()
                .ToList();
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        object IUserRepository.GetByEmail(string email)
        {
            return GetByEmail(email);
        }
    }
}
