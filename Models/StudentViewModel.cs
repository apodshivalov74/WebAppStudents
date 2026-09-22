using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace WebAppStudents.Models
{
    public class StudentViewModel
    {
        public StudentViewModel()
        {
            ListAcademicGroup = new List<SelectListItem>();
        }

        [JsonIgnore]
        public List<SelectListItem> ListAcademicGroup { get; set; }

        public long StudentViewModelId { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public int Age { get; set; }

        public string? AcademicGroup { get; set; }
    }
}