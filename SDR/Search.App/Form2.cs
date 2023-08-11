using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace Search.App
{
    public partial class Form2 : Form
    {
        public static string ExamId = "";
        public static string PdfLink = "";
        public static string StudentName = "";
        public static string Grade = "";
        public static string Comments = "";
        public static string pdfName = "";
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            txtName.Text = StudentName;
            txtGrade.Text = Grade;
            txtComments.Text = Comments;
            txtExamNo.Text = ExamId;

            radPdfViewer1.LoadDocument(PdfLink);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save PDF Files";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = false;
            saveFileDialog1.FileName = pdfName;
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "Text files (*.pdf)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                 string pdfSavePath = saveFileDialog1.FileName;
                File.Copy(PdfLink, pdfSavePath);
                MessageBox.Show("Pdf file downloaded and saved successfully.");
            }
        }
    }
}
