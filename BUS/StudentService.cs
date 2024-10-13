using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class StudentService
    {
        public List<Student> GetALL()
        {
            Model1 db = new Model1();
            return db.Students.ToList();
        }

        public bool AddStudent(string mssv, string tenSinhVien, int facultyId, float diemTB)
        {
            Model1 db = new Model1();


            try
            {
                var exisStudent = db.Students.FirstOrDefault(s => s.mssv == mssv);
                if (exisStudent != null)
                {
                    return false;
                }

                var student = new Student
                {
                    mssv = mssv,
                    tenSinhVien = tenSinhVien,
                    diemTB = diemTB,
                    facultyId = facultyId

                };

                db.Students.Add(student);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Student> GetAllStudentsWithNoMajor()
        {
            using (var db = new Model1())
            {
                return db.Students.Where(s => s.Major.majorName == null).ToList();
            }
        }


        public List<Faculty> GetAllFaculties()
        {
            using (var db = new Model1())
            {
                return db.Faculties.ToList();
            }
        }



        public void DeleteStudent(string mssv)
        {
            using (var db = new Model1())
            {

                var student = db.Students.FirstOrDefault(s => s.mssv == mssv);

                if (student == null)
                {
                    throw new Exception("Sinh viên không tồn tại.");
                }


                db.Students.Remove(student);
                db.SaveChanges();
            }
        }
        public bool UpdateStudentImage(string mssv, string imagePath)
        {
            try
            {
                using (var db = new Model1())
                {
                    var student = db.Students.FirstOrDefault(s => s.mssv == mssv);
                    if (student != null)
                    {
                        student.imagePath = imagePath;
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lưu ảnh: " + ex.Message);
            }

        }

    }
}
