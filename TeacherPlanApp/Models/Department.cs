namespace TeacherPlanApp.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string? Name { get; set; } // Название кафедры
        
        // Навигационное свойство для связи "один-ко-многим"
        public List<Teacher> Teachers { get; set; } = new();
    }
}