using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

  public class SearchExam
    {
        public string ExamNumber { get; set; }
        public string Year { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Professor { get; set; }
        public string Semester { get; set; }
        public string Subject { get; set; }
        public string StudentId { get; set; }
    }

public class SearchParms
{
    public string Field { get; set; }
    public string Value { get; set; }
}
 
