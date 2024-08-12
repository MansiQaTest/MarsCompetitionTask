using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionTask.Models
{
    public class TestCaseData4
    {
        public string TestCase { get; set; }
        public List<CertificationModel> Data { get; set; }
    }
    public class CertificationModel
    {
        public string CertificationName { get; set; }
        public string CertificationFrom { get; set; }
        public string CertificationYear { get; set; }
    }
}
