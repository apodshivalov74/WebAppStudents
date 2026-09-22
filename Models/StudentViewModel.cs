using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace WebAppStudents.Models
{
    /// <summary>
    /// то что нужно показать на страницы
    /// </summary>
    public class StudentViewModel
    {
        public StudentViewModel()
        {
            ListAcademicGroup = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "ПИ-136", Value = "ПИ-136"},
                new SelectListItem() {Text = "ПИ-234", Value = "ПИ-234"},
                new SelectListItem() {Text = "ПИ-332", Value = "ПИ-332"},
                new SelectListItem() {Text = "ПИ-428", Value = "ПИ-428"}
            };
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
