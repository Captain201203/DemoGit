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
using System.Xml.Linq;

namespace buoi_6___mo_hinh_3_lop
{
    public partial class Form1 : Form
    {
        private readonly StudentService studentService = new StudentService();
        public Form1()
        {
            InitializeComponent();
        }

        public void Add()
        {

            string mssv = txbId.Text.Trim();
            string tenSinhVien = txbName.Text.Trim();


            if (!float.TryParse(txbAverage.Text, out float diemTB))
            {
                MessageBox.Show("Điểm trung bình phải là số.");
                return;
            }


            if (cbbFaculty.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn khoa.");
                return;
            }


            int facultyId = (int)cbbFaculty.SelectedValue;

            if (string.IsNullOrWhiteSpace(mssv) || string.IsNullOrWhiteSpace(tenSinhVien))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }


            var studentService = new StudentService();


            bool isAdded = studentService.AddStudent(mssv, tenSinhVien, facultyId, diemTB);


            if (isAdded)
            {

                txbId.Clear();
                txbName.Clear();
                txbAverage.Clear();

                MessageBox.Show("Thêm sinh viên thành công");
            }
            else
            {

                MessageBox.Show("Thêm sinh viên không thành công. Vui lòng kiểm tra lại.");
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {

            using (var db = new Model1())
            {
                List<Student> students;

                if (checkBox1.Checked)
                {
                    
                    students = db.Students.Where(s => s.Major == null).ToList();
                }
                else
                {
                   
                    students = db.Students.ToList();
                }

                var fillStudent = students.Select(s => new
                {
                    mssv = s.mssv,
                    tenSinhVien = s.tenSinhVien,
                    facultyName = s.Faculty.facultyName, // Tên khoa
                    diemTB = s.diemTB,
                    majorName = s.Major != null ? s.Major.majorName : "Chưa có chuyên ngành" // Kiểm tra Major có null không
                }).ToList();

                // Gán dữ liệu vào DataGridView
                dataGridView1.DataSource = fillStudent;

                // Tùy chỉnh tiêu đề các cột
                dataGridView1.Columns["mssv"].HeaderText = "MSSV";
                dataGridView1.Columns["tenSinhVien"].HeaderText = "Tên Sinh Viên";
                dataGridView1.Columns["diemTB"].HeaderText = "Điểm TB";
                dataGridView1.Columns["facultyName"].HeaderText = "Tên Khoa"; // Tiêu đề cho cột tên khoa
                dataGridView1.Columns["majorName"].HeaderText = "Tên Chuyên Ngành"; // Tiêu đề cho cột tên chuyên ngành
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add();
            Form1_Load(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                string mssv = dataGridView1.SelectedRows[0].Cells["mssv"].Value.ToString();


                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?",
                                                    "Xác nhận xóa",
                                                    MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {

                    studentService.DeleteStudent(mssv);

                    MessageBox.Show("Xóa sinh viên thành công");


                    Form1_Load(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa.");
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Chọn Ảnh Sinh Viên";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Hiển thị ảnh trong PictureBox
                    pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                    pictureBox1.Tag = openFileDialog.FileName; // Lưu đường dẫn ảnh vào Tag
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (Application.OpenForms["Form2"] == null)
            {
                // Tạo một instance của FormRegisterMajor
                Form2 registerMajorForm = new Form2();

                // Hiển thị form đăng ký chuyên ngành
                registerMajorForm.Show();
            }
            else
            {
                // Đưa form đã mở ra trước màn hình
                Application.OpenForms["Form2"].BringToFront();
            }
        }
   

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            string mssv = txbId.Text;
            string imagePath = txbImagePath.Text; // Đường dẫn ảnh đã chọn

            // Gọi phương thức lưu đường dẫn ảnh
            bool result = studentService.UpdateStudentImage(mssv, imagePath);

            if (result)
            {
                MessageBox.Show("Đã lưu ảnh thành công!");
            }
            else
            {
                MessageBox.Show("Lỗi khi lưu ảnh!");
            }
        }
    }
}

