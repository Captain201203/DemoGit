using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class FacultyService
    {
        public List<Faculty> GetAllFaculty()
        {
            using(var db = new Model1())
            {
                return db.Faculties.ToList();
            }
        }
    }
}
