using WebAppStudents.Models;
using Newtonsoft.Json;

namespace WebAppStudents.DataAccessLayer
{
    public class StorageHelper
    {
        public static void SaveStudents(IWebHostEnvironment environment, List<StudentViewModel> list)
        {
            string json = JsonConvert.SerializeObject(list, Formatting.Indented);

            string wwwRootPath = environment.WebRootPath;
            string filename = Path.Combine(wwwRootPath, "Storage", "students.json");

            File.WriteAllText(filename, json, System.Text.Encoding.UTF8);
        }

        public static List<StudentViewModel>? LoadStudents(IWebHostEnvironment environment)
        {
            string wwwRootPath = environment.WebRootPath;
            string filename = Path.Combine(wwwRootPath, "Storage", "students.json");

            if (!File.Exists(filename))
            {
                return null;
            }

            string json = File.ReadAllText(filename);

            var list = JsonConvert.DeserializeObject<List<StudentViewModel>>(json);

            return list;
        }

        public static void SaveAcademicGroups(IWebHostEnvironment environment, List<AcademicGroupViewModel> list)
        {
            string json = JsonConvert.SerializeObject(list, Formatting.Indented);

            string wwwRootPath = environment.WebRootPath;
            string filename = Path.Combine(wwwRootPath, "Storage", "academicGroups.json");

            File.WriteAllText(filename, json, System.Text.Encoding.UTF8);
        }

        public static List<AcademicGroupViewModel>? LoadAcademicGroups(IWebHostEnvironment environment)
        {
            string wwwRootPath = environment.WebRootPath;
            string filename = Path.Combine(wwwRootPath, "Storage", "academicGroups.json");

            if (!File.Exists(filename))
            {
                return null;
            }

            string json = File.ReadAllText(filename);

            var list = JsonConvert.DeserializeObject<List<AcademicGroupViewModel>>(json);

            return list;
        }
    }
}