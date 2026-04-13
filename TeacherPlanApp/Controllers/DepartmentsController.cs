using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeacherPlanApp.Data;
using TeacherPlanApp.Models;

namespace TeacherPlanApp.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _context;

        // Подключаем нашу базу данных
        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        // 1. READ: Открывает страницу со списком всех кафедр
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments.ToListAsync();
            return View(departments);
        }

        // 2. CREATE (Шаг 1): Просто открывает пустую форму для заполнения
        public IActionResult Create()
        {
            return View();
        }

        // 3. CREATE (Шаг 2): Получает заполненные данные из формы и сохраняет в БД
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync(); // Сохраняем в Postgres
                return RedirectToAction(nameof(Index)); // Возвращаемся к списку
            }
            return View(department);
        }
        // --- UPDATE: РЕДАКТИРОВАНИЕ ---

        // 1. Открывает форму с текущими данными кафедры
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            
            var department = await _context.Departments.FindAsync(id); // Ищем в БД по ID
            if (department == null) return NotFound();
            
            return View(department); // Отдаем данные на страницу
        }

        // 2. Сохраняет новые данные в БД
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(department); // Говорим EF Core обновить запись
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // --- DELETE: УДАЛЕНИЕ ---

        // 1. Открывает страницу с вопросом "Вы точно хотите удалить?"
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments.FirstOrDefaultAsync(m => m.Id == id);
            if (department == null) return NotFound();

            return View(department);
        }

        // 2. Физически удаляет из БД
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department); // Удаляем
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}