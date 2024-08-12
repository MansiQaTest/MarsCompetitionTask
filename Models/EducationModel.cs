using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionTask.Models
{
    public class TestCaseData2
    {
        public string TestCase { get; set; }
        public List<EducationModel> Data { get; set; }
    }

    public class EducationModel
    {
        public string Country { get; set; }
        public string University { get; set; }
        public string Title { get; set; }
        public string Degree { get; set; }
        public string GraduationYear { get; set; }
    }

   }



