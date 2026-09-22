using Microsoft.AspNetCore.Mvc;
using WebAppStudents.DataAccessLayer;
using WebAppStudents.Models;

namespace WebAppStudents.Controllers
{
    public class AcademicGroupsController : Controller
    {
        private static List<AcademicGroupViewModel> _groupsList = new List<AcademicGroupViewModel>();
        private readonly IWebHostEnvironment _environment;

        public AcademicGroupsController(IWebHostEnvironment environment)
        {
            this._environment = environment;

            if (_groupsList.Count == 0)
            {
                _groupsList =
                    StorageHelper.LoadAcademicGroups(environment) ??
                    new List<AcademicGroupViewModel>();
            }
        }

        // Просмотр списка всех групп
        public IActionResult AcademicGroupsListView()
        {
            return View(_groupsList);
        }

        // Просмотр детальной информации о группе
        public IActionResult AcademicGroupDetailsView(int id)
        {
            var model = _groupsList
                .FirstOrDefault(x => x.AcademicGroupId == id);

            if (model == null)
            {
                return View("ErrorView", "group not found");
            }

            return View(model);
        }

        // Открытие формы создания новой группы
        public IActionResult AcademicGroupCreateView()
        {
            var model = new AcademicGroupViewModel();
            return View(nameof(AcademicGroupEditView), model);
        }

        // Открытие формы редактирования существующей группы
        public IActionResult AcademicGroupEditView(int id)
        {
            var model = _groupsList
                .FirstOrDefault(x => x.AcademicGroupId == id);

            if (model == null)
            {
                return View("ErrorView", "group not found");
            }

            return View(model);
        }

        // Обработка сохранения (создание или обновление)
        [HttpPost]
        public IActionResult AcademicGroupUpdate(AcademicGroupViewModel model)
        {
            if (model.AcademicGroupId == 0)
            {
                // Новая группа: вычисляем следующий свободный ID
                long nextId = _groupsList.Count > 0
                    ? _groupsList.Max(x => x.AcademicGroupId) + 1
                    : 1;

                model.AcademicGroupId = nextId;
                _groupsList.Add(model);
            }
            else
            {
                // Существующая группа: находим индекс и обновляем данные
                int index = _groupsList
                    .FindIndex(x => x.AcademicGroupId == model.AcademicGroupId);

                if (index > -1)
                {
                    _groupsList[index] = model;
                }
                else
                {
                    return View("ErrorView", "group not found");
                }
            }

            // Записываем актуальный список в academicGroups.json
            StorageHelper.SaveAcademicGroups(_environment, _groupsList);

            // Возвращаемся обратно к списку групп
            return RedirectToAction(nameof(AcademicGroupsListView));
        }

        // Удаление группы
        public IActionResult AcademicGroupDeleteView(int id)
        {
            var model = _groupsList
                .FirstOrDefault(x => x.AcademicGroupId == id);

            if (model == null)
            {
                return View("ErrorView", "group not found");
            }

            _groupsList.Remove(model);

            // Сохраняем изменения в JSON после удаления
            StorageHelper.SaveAcademicGroups(_environment, _groupsList);

            return RedirectToAction(nameof(AcademicGroupsListView));
        }

        // Главная точка входа контроллера
        public IActionResult Index()
        {
            return RedirectToAction(nameof(AcademicGroupsListView));
        }
    }
}