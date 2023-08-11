
using iTextSharp.text;
using iTextSharp.text.pdf;
using Search.App.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Search.App
{
    public partial class Form1 : Form
    {
        DataSources DS = new DataSources();
        List<SearchParms> lst = new List<SearchParms>();
        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\tempFiles\";
        string filePath = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        void LoadData()
        {
            var dt = DS.GetSubjects();
            DataRow iRow = dt.NewRow();
            iRow["Id"] = 0;
            iRow["Subject"] = "--Select Subject--";
            dt.Rows.InsertAt(iRow, 0);

            cmbSubjects.DataSource = dt;
            cmbSubjects.DisplayMember = "Subject";
            cmbSubjects.ValueMember = "Id";

            dt = DS.GetProfessors();
            iRow = dt.NewRow();
            iRow["Id"] = 0;
            iRow["name"] = "--Select Professor--";
            dt.Rows.InsertAt(iRow, 0);

            cmbProfessors.DataSource = dt;
            cmbProfessors.DisplayMember = "name";
            cmbProfessors.ValueMember = "Id";

            dt = DS.GetSemesters();
            iRow = dt.NewRow();
            iRow["name"] = "--Select Semester--";
            dt.Rows.InsertAt(iRow, 0);

            cmbSemester.DataSource = dt;
            cmbSemester.DisplayMember = "name";
            cmbSemester.ValueMember = "name";

            dt = DS.GetYears();
            iRow = dt.NewRow();
            iRow["name"] = "--Select Year--";
            dt.Rows.InsertAt(iRow, 0);

            cmbYears.DataSource = dt;
            cmbYears.DisplayMember = "name";
            cmbYears.ValueMember = "name";

            dt = DS.GetStatus();
            iRow = dt.NewRow();
            iRow["name"] = "--Select Status--";
            dt.Rows.InsertAt(iRow, 0);

            cmbStatus.DataSource = dt;
            cmbStatus.DisplayMember = "name";
            cmbStatus.ValueMember = "name";

        }

        private void AddSearchItem(string field, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value != "0")
                {
                    if (!value.Contains("--Select"))
                        lst.Add(new SearchParms { Field = field, Value = value });
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dgExam.Columns.Clear();
            string SearchQuery = "";

            AddSearchItem("examination", txtExamNumber.Text);
            AddSearchItem(GetColumn("year", "", "examination"), cmbYears.SelectedValue.ToString());
            //AddSearchItem("from", dtStart.Value.ToString());
            //AddSearchItem("to", dtEnd.Value.ToString());
            AddSearchItem(GetColumn("professor", "", "examination"), cmbProfessors.SelectedValue.ToString());
            AddSearchItem(GetColumn("semester", "", "examination"), cmbSemester.SelectedValue.ToString());
            AddSearchItem(GetColumn("subject", "", "examination"), cmbSubjects.SelectedValue.ToString());
            AddSearchItem("student", txtStudentId.Text);
            AddSearchItem(GetColumn("status", "", "examination"), cmbStatus.SelectedValue.ToString());

            int Count = lst.Count();
            int ItemCount = 1;
            foreach (var obj in lst)
            {

                if (Count == ItemCount || SearchQuery == "")
                {
                    if (IsColumnEncrypted("answersheets", obj.Field))
                    {
                        if (Count == 1)
                            SearchQuery = "CAST(AES_DECRYPT(" + obj.Field + ", 'D6E2D35C80AA466C9C23A0120874B4B2') AS CHAR) = '" + obj.Value.ToString() + "'";
                        else
                            SearchQuery += "CAST(AES_DECRYPT(" + obj.Field + ", 'D6E2D35C80AA466C9C23A0120874B4B2') AS CHAR) = '" + obj.Value.ToString() + "' AND ";
                    }
                    else
                    {
                        if (Count == 1)
                            SearchQuery = obj.Field + "='" + obj.Value.ToString() + "'";
                        else
                        {
                            if (Count != ItemCount)
                                SearchQuery += obj.Field + "='" + obj.Value.ToString() + "' AND ";
                            else
                                SearchQuery += obj.Field + "='" + obj.Value.ToString() + "'";
                        }
                    }
                }
                else
                {
                    if (IsColumnEncrypted("answersheets", obj.Field))
                    {
                        if (Count != ItemCount)
                            SearchQuery += "CAST(AES_DECRYPT(" + obj.Field + ", 'D6E2D35C80AA466C9C23A0120874B4B2') AS CHAR) = '" + obj.Value.ToString() + "' AND ";
                        else
                            SearchQuery += "CAST(AES_DECRYPT(" + obj.Field + ", 'D6E2D35C80AA466C9C23A0120874B4B2') AS CHAR) = '" + obj.Value.ToString() + "'";
                    }
                    else
                    {
                        if (Count != ItemCount)
                            SearchQuery += obj.Field + "='" + obj.Value.ToString() + "' AND ";
                        else
                            SearchQuery += obj.Field + "='" + obj.Value.ToString() + "'";
                    }
                }

                ItemCount++;
            }

            string SelectQry = "SELECT "
                               + GetColumn("student", "Id") + ","
                               + GetColumn("studentName", "Stu.Name") + ","
                               + GetColumn("grade", "Grade") + ","
                               + GetColumn("notes", "Note") + ","
                               + GetColumn("SysGeneratedName", "pdf") + ","
                               + GetColumn("examination", "Exam No") + ","
                               + GetColumn("Date", "Date", "examination") + ","
                               + GetColumn("semester", "Semester", "examination") + ","
                               + GetColumn("subject", "Sub. Code", "examination") + ","
                               + GetInnerColumn("subject", "name", "Id", GetColumn("subject", "", "examination"), "Subject") + ","
                               + GetInnerColumn("user", "name", "Id", GetColumn("professor", "", "examination"), "Professor") + ","
                               + GetColumn("status", "Status", "examination") + " FROM answersheets INNER JOIN examination" +
                               " ON " + JoinOn("examination", "") + "=" + JoinOn("number", "", "examination") + " WHERE " + SearchQuery;


            SelectQry = SelectQry.Replace("status", "examination.status");

            dgExam.DataSource = new DatabaseHelper().FillData(SelectQry);

            DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            {
                button.Name = "btnPdf";
                button.HeaderText = "Answer Sheet";
                button.Text = "Pdf";
                button.UseColumnTextForButtonValue = true; //dont forget this line
                this.dgExam.Columns.Add(button);
            }

            //dgExam.Columns[0].Visible = false;
            //dgExam.Columns[1].Visible = false;
            //dgExam.Columns[2].Visible = false;
            //dgExam.Columns[3].Visible = false;
            if (dgExam.Rows.Count > 1)
            {
                dgExam.Columns[4].Visible = false;
            }
            lst = new List<SearchParms>();
        }
        private void dgExam_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgExam.Columns["btnPdf"].Index)
            {
                //MessageBox.Show(dgExam.CurrentRow.Cells[0].Value.ToString());
                Form2.StudentName = dgExam.CurrentRow.Cells[1].Value.ToString();
                Form2.Grade = dgExam.CurrentRow.Cells[2].Value.ToString();
                Form2.Comments = dgExam.CurrentRow.Cells[3].Value.ToString();
                Form2.PdfLink = System.Configuration.ConfigurationManager.AppSettings["PDFFolderPath"] + "1421227603.pdf"; //dgExam.CurrentRow.Cells[4].Value.ToString();
                Form2.ExamId = dgExam.CurrentRow.Cells[5].Value.ToString();
                
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Guid.NewGuid().ToString() + ".pdf";
                filePath = path + Path.GetFileNameWithoutExtension(Form2.PdfLink) + "_" + fileName;
                AppendToDocument(Form2.PdfLink, filePath, Form2.StudentName, Form2.ExamId, Form2.Grade, Form2.Comments);
                Form2.pdfName = Path.GetFileName(Form2.PdfLink);
                Form2.PdfLink = filePath;
                new Form2().ShowDialog();
                //Do something with your button.
            }
        }
        private string JoinOn(string columnName, string AliasName, string tableName = "answersheets")
        {
            string _As = " As '" + AliasName + "'";
            if (AliasName == "")
                _As = "";


            if (IsColumnEncrypted(tableName, columnName))
                return "CAST(AES_DECRYPT(" + tableName + "." + columnName + ", 'D6E2D35C80AA466C9C23A0120874B4B2') AS CHAR) " + _As;
            else
                return tableName + "." + columnName + _As;
        }
        private string GetColumn(string columnName, string AliasName, string tableName = "answersheets")
        {
            string _As = " As '" + AliasName + "'";
            if (AliasName == "")
                _As = "";


            if (IsColumnEncrypted(tableName, columnName))
                return "CAST(AES_DECRYPT(" + columnName + ", 'D6E2D35C80AA466C9C23A0120874B4B2') AS CHAR) " + _As;
            else
                return columnName + _As;
        }
        private string GetInnerColumn(string tableName, string columnName, string searchField, string searchValue, string AliasName)
        {

            if (IsColumnEncrypted(tableName, columnName))
                return "(SELECT CAST(AES_DECRYPT(" + columnName + ", 'D6E2D35C80AA466C9C23A0120874B4B2') AS CHAR) FROM " + tableName + " WHERE " + searchField + "=" + GetColumn(searchValue, "") + " LIMIT 1) AS '" + AliasName + "'";
            else
                return "(SELECT " + columnName + " FROM " + tableName + " WHERE " + searchField + "=" + GetColumn(searchValue, "") + " LIMIT 1) AS '" + AliasName + "'";
        }
        public bool IsColumnEncrypted(string tableName, string columnName)
        {
            var dt = new DatabaseHelper().FillData("SELECT * FROM applicationinfo WHERE tablename='" + tableName + "' And columnname='" + columnName + "'");
            if (dt.Rows.Count > 0)
                return true;

            return false;
        }

        public void MergeFiles(string destinationFile, string[] sourceFiles)
        {

            try
            {
                int f = 0;
                // we create a reader for a certain document
                PdfReader reader = new PdfReader(sourceFiles[f]);
                // we retrieve the total number of pages
                int n = reader.NumberOfPages;
                //Console.WriteLine("There are " + n + " pages in the original file.");
                // step 1: creation of a document-object
                Document document = new Document(reader.GetPageSizeWithRotation(1));
                // step 2: we create a writer that listens to the document
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationFile, FileMode.Create));
                // step 3: we open the document
                document.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;
                int rotation;
                // step 4: we add content
                while (f < sourceFiles.Length)
                {
                    int i = 0;
                    while (i < n)
                    {
                        i++;
                        document.SetPageSize(reader.GetPageSizeWithRotation(i));
                        document.NewPage();
                        page = writer.GetImportedPage(reader, i);
                        rotation = reader.GetPageRotation(i);
                        if (rotation == 90 || rotation == 270)
                        {
                            cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                        }
                        else
                        {
                            cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                        }
                        //Console.WriteLine("Processed page " + i);
                    }
                    f++;
                    if (f < sourceFiles.Length)
                    {
                        reader = new PdfReader(sourceFiles[f]);
                        // we retrieve the total number of pages
                        n = reader.NumberOfPages;
                        //Console.WriteLine("There are " + n + " pages in the original file.");
                    }
                }
                // step 5: we close the document
                document.Close();
            }
            catch (Exception e)
            {
                string strOb = e.Message;
            }
        }

        public int CountPageNo(string strFileName)
        {
            // we create a reader for a certain document
            PdfReader reader = new PdfReader(strFileName);
            // we retrieve the total number of pages
            return reader.NumberOfPages;
        }

        private void AppendToDocument(string sourcePdfPath2, string outputPdfPath, string studentName, string ExamNo, string Grade, string comments)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            string fileName = Guid.NewGuid().ToString() + ".pdf";
            string filePath1 = path + Path.GetFileNameWithoutExtension(Form2.PdfLink) + "_temp_" + fileName;

          
            using (var document = new Document(PageSize.A4, 10f, 10f, 10f, 0f))
            {
                using (var writer = PdfWriter.GetInstance(document, new FileStream(filePath1, FileMode.Create)))
                {
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(6);

                    PdfPCell cell = new PdfPCell(new Phrase("Student Information"));
                    cell.Colspan = 6;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);


                    table.AddCell("Student Name");
                    table.AddCell(studentName);
                    table.AddCell("Exam No.");
                    table.AddCell(ExamNo);

                    table.AddCell("Grade");
                    table.AddCell(Grade);

                    table.AddCell("Comments");
                    cell = new PdfPCell(new Phrase(comments));
                    cell.Colspan = 5;
                    table.AddCell(cell);

                    document.Open();
                    document.Add(table);

                    // Add a simple and wellknown phrase to the document in a flow layout manner  
                    Paragraph p = new Paragraph("Copyright INDKARTA 2021-2023");
                    p.Alignment = Element.ALIGN_CENTER;
                    document.Add(p);
                    // Close the document  
                    document.Close();
                    // Close the writer instance  
                    writer.Close();
                    // Always close open filehandles explicity  

                }
            }

            MergeFiles(outputPdfPath, new string[] { sourcePdfPath2, filePath1 });

            
        }

    }



}

