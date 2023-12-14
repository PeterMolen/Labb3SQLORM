using Labb3SQLORM.Models;

namespace Labb3SQLORM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HogwartzContext context = new HogwartzContext();
            //context.GetPersonnel();
            //context.SortStudents();
            //context.ClassesNstudents();
            //context.ClassNlowHighGrades();
            //context.AddUser();
            context.AddProffesor();
        }
    }
}
