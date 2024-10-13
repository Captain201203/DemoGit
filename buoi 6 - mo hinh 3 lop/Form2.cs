using BUS;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace buoi_6___mo_hinh_3_lop
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }



        private void cbbFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbFaculty.SelectedValue != null)
            {
               
                if (int.TryParse(cbbFaculty.SelectedValue.ToString(), out int facultyId))
                {
                    MajorService majorService = new MajorService();

            
                    List<Major> majors = majorService.GetMajorByFaculty(facultyId);

                 
                    cbbMajor.DataSource = majors;
                    cbbMajor.DisplayMember = "majorName"; 
                    cbbMajor.ValueMember = "majorId"; 
                }
            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {

            FacultyService facultyService = new FacultyService();
            List<Faculty> faculties = facultyService.GetAllFaculty();


            cbbFaculty.DataSource = faculties;
            cbbFaculty.DisplayMember = "facultyName";
            cbbFaculty.ValueMember = "facultyId";


            cbbFaculty.SelectedItem = faculties.FirstOrDefault(f => f.facultyName == "Công Nghệ Thông Tin");


            if (cbbFaculty.SelectedValue != null)
            {
                if (int.TryParse(cbbFaculty.SelectedValue.ToString(), out int facultyId))
                {
                    MajorService majorService = new MajorService();

                    // Lấy danh sách chuyên ngành theo khoa
                    List<Major> majors = majorService.GetMajorByFaculty(facultyId);


                    cbbMajor.DataSource = majors;
                    cbbMajor.DisplayMember = "majorName";
                    cbbMajor.ValueMember = "majorId";
                }
            }
            StudentService studentService = new StudentService();
            List<Student> studentsWithNoMajor = studentService.GetAllStudentsWithNoMajor();

         
            dataGridView1.DataSource = studentsWithNoMajor.Select(s => new
            {
                mssv = s.mssv,
                tenSinhVien = s.tenSinhVien,
                diemTB = s.diemTB,
           
            }).ToList();

            // Đặt tiêu đề cho các cột
            dataGridView1.Columns["mssv"].HeaderText = "MSSV";
            dataGridView1.Columns["tenSinhVien"].HeaderText = "Tên Sinh Viên";
            dataGridView1.Columns["diemTB"].HeaderText = "Điểm TB";
        }
    }
}
