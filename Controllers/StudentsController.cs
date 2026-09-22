using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebAppStudents.DataAccessLayer;
using WebAppStudents.Models;

namespace WebAppStudents.Controllers
{
    // контроллер
    public class StudentsController : Controller
    {
        private static List<StudentViewModel> _studentsList = new List<StudentViewModel>();
        private readonly IWebHostEnvironment _environment;

        public StudentsController(IWebHostEnvironment environment)
        {
            this._environment = environment;
            if (_studentsList.Count == 0)
            {
                // ?? - оператор коалесцентности в C#8.0
                /*
                 * var list = StorageHelper.LoadStudents();
                 * if (list == null) list = new List<StudentViewModel>();
                 * 
                 * */
                _studentsList = 
                    StorageHelper.LoadStudents(environment) ?? 
                    new List<StudentViewModel>();
            }
        }

        public IActionResult StudentEditView(int id)
        {
            var model = _studentsList
                .FirstOrDefault(x => x.StudentViewModelId == id);

            if (model == null)
            {
                return View("ErrorView", "student not found");
            }

            FillAcademicGroups(model); // <-- подтягиваем группы из JSON

            return View(model);
        }

        // StudentDeleteView
        public IActionResult StudentDeleteView(int id)
        {
            var model = _studentsList
                .FirstOrDefault(x => x.StudentViewModelId == id);

            if (model == null)
            {
                return View("ErrorView", "student not found");
            }

            _studentsList.Remove(model);

            // return View("StudentsListView", _studentsList);
            return RedirectToAction("StudentsListView");
        }

        public IActionResult StudentsListView()
        {
            // asd joiasdu iuasydui asduiy asduiytauid
            return View(_studentsList);
        }

        // StudentCreateView
        public IActionResult StudentCreateView()
        {
            var model = new StudentViewModel();
            FillAcademicGroups(model); // <-- подтягиваем группы из JSON
            return View(nameof(StudentEditView), model);
        }

        [HttpPost]
        public IActionResult StudentUpdate(StudentViewModel model)
        {
            string studentActionName = string.Empty;

            if (model.StudentViewModelId == 0)
            {
                // студент новый - добавляем
                _studentsList.Add(model);
                model.StudentViewModelId = _studentsList.Count;
                studentActionName = "добавлен";
            }
            else
            {
                int index = _studentsList
                    .FindIndex(0, x => x.StudentViewModelId == model.StudentViewModelId);
                if (index > -1)
                {
                    // студент существующий - обновляем
                    _studentsList[index] = model;
                    studentActionName = "данные обновлены";
                }
                else
                {
                    return View("ErrorView", "student not found");
                }
            }

            StorageHelper.SaveStudents(_environment, _studentsList);

            string? lastname = model.LastName;

            ViewData["lastName"] = lastname;
            ViewData[nameof(studentActionName)] = studentActionName;
            return View("StudentThanksView");
        }


        // экшен контроллера - просит Клиент (обращается)
        public IActionResult StudentDetailsView(int id)
        {
            var model = _studentsList
                .FirstOrDefault(x => x.StudentViewModelId == id);

            if (model == null)
            {
                return View("ErrorView", "student not found");
            }

            // передача Модели - из контроллера на Представление
            return View(model); // вьюшка возвращается - клиенту в браузер
        }

        public IActionResult Index()
        {
            ViewData["counterStudents"] = _studentsList.Count;

            return View();
        }

        private void FillAcademicGroups(StudentViewModel model)
        {
            var groups = StorageHelper.LoadAcademicGroups(_environment) ?? new List<AcademicGroupViewModel>();

            model.ListAcademicGroup = groups
                .Select(g => new SelectListItem
                {
                    Text = g.Name,
                    Value = g.Name,
                    Selected = (g.Name == model.AcademicGroup) // автоматический выбор текущей группы
                })
                .ToList();
        }
    }
}
