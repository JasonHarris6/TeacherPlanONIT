namespace TeacherPlanApp.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? FullName { get; set; } // ФИО
        public string? Position { get; set; } // Должность (ассистент, доцент и т.д.)
        
        // Внешний ключ
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}