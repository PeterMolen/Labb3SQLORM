using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Labb3SQLORM.Models
{
    public partial class HogwartzContext : DbContext
    {
        public void GetPersonnel()
        {
            Console.WriteLine("Vill du se all personal (A) eller endast Proffesorer (P)?");
            string userChoice = Console.ReadLine();

            using (var dbContext = new HogwartzContext()) // Replace with your DbContext name
            {
                IQueryable<object> personnelQuery;

                if (userChoice.ToLower() == "a")
                {
                    personnelQuery = GetAllPersonnelQuery(dbContext);
                }
                else if (userChoice.ToLower() == "p")
                {
                    personnelQuery = GetTeachersQuery(dbContext);
                }
                else
                {
                    Console.WriteLine("Ogiltigt val.");
                    return;
                }

                var personnelList = personnelQuery.ToList();

                foreach (var person in personnelList)
                {
                    Console.WriteLine($"Title: {person.GetType().GetProperty("Title").GetValue(person)}, FirstName:" +
                        $" {person.GetType().GetProperty("FirstName").GetValue(person)}, LastName: " +
                        $"{person.GetType().GetProperty("LastName").GetValue(person)}");

                }
            }
        }

        private IQueryable<object> GetAllPersonnelQuery(HogwartzContext dbContext)
        {
            return from professor in dbContext.Proffesors
                   join occupation in dbContext.Occupations on professor.ProffesorId equals occupation.FkProffesorId
                   join profession in dbContext.Proffesions on occupation.FkProffesionId equals profession.ProffesionId
                   select new
                   {
                       Title = profession.Title,
                       FirstName = professor.FirstName,
                       LastName = professor.LastName
                   };
        }

        private IQueryable<object> GetTeachersQuery(HogwartzContext dbContext)
        {
            return from professor in dbContext.Proffesors
                   join occupation in dbContext.Occupations on professor.ProffesorId equals occupation.FkProffesorId
                   join profession in dbContext.Proffesions on occupation.FkProffesionId equals profession.ProffesionId
                   where profession.Title.ToLower() == "Proffesor"
                   select new
                   {
                       Title = profession.Title,
                       FirstName = professor.FirstName,
                       LastName = professor.LastName
                   };
        }

        public void SortStudents()
        {
            using (var dbContext = new HogwartzContext())
            {
                // asks user for sorting preferences
                Console.WriteLine("Vill du sortera på förnamn eller efternamn?");
                Console.WriteLine("skriv: förnamn eller efternamn");
                string sortChoice = Console.ReadLine();

                Console.WriteLine("Vill du ha stigande eller fallande ordning?");
                Console.WriteLine("skriv: stigande eller fallande");
                string orderChoice = Console.ReadLine();

                // LINQ query to retrieve and display sorted students
                var sortedStudents = GetSortedStudents(dbContext.Students, sortChoice, orderChoice);

                // loop to show result
                foreach (var student in sortedStudents)
                {
                    // Display student information
                    Console.WriteLine($"Student ID: {student.StudentId}, Name: {student.StudentFirstName} {student.StudentLastName}");
                }
            }
        }

        // Method to construct LINQ query based on user input
        static IQueryable<Student> GetSortedStudents(IQueryable<Student> students, string sortBy, string sortOrder)
        {
            switch (sortBy.ToLower())
            {
                // Sort by first name
                case "förnamn":
                    return sortOrder.ToLower() == "stigande"
                        ? students.OrderBy(student => student.StudentFirstName)
                        : students.OrderByDescending(student => student.StudentFirstName);

                // Sort by last name
                case "efternamn":
                    return sortOrder.ToLower() == "stigande"
                        ? students.OrderBy(student => student.StudentLastName)
                        : students.OrderByDescending(student => student.StudentLastName);

                // Invalid sorting option
                default:
                    throw new ArgumentException("Ogiltig sortering.");
            }
        }

        public void ClassesNstudents()
        {
            using (var dbContext = new HogwartzContext())
            {
                // Display the list of classes
                var classesList = dbContext.Classes
                    .Select(c => new
                    {
                        ClassId = c.ClassId,
                        ClassName = c.ClassName,
                        ClassInfo = c.ClassInfo
                    })
                    .ToList();

                // Display classes to the user
                Console.WriteLine("Available Classes:");
                foreach (var c in classesList)
                {
                    Console.WriteLine($"ClassID: [{c.ClassId}] ClassName: {c.ClassName}, ClassInfo: {c.ClassInfo}");
                }

                // Prompt the user to choose a ClassID
                Console.Write("Enter the ClassID you want to see students for: ");
                int selectedClassId = int.Parse(Console.ReadLine());

                // Retrieve and display the students' names for the selected class
                var studentsInClass = dbContext.Students
                    .Join(dbContext.Enrollments,
                          student => student.StudentId,
                          enrollment => enrollment.FkStudentId,
                          (student, enrollment) => new { student, enrollment })
                    .Join(dbContext.Classes,
                          combined => combined.enrollment.FkClassId,
                          classInfo => classInfo.ClassId,
                          (combined, classInfo) => new
                          {
                              Student = combined.student,
                              ClassId = classInfo.ClassId,
                              ClassName = classInfo.ClassName,
                              ClassInfo = classInfo.ClassInfo
                          })
                    .Where(result => result.ClassId == selectedClassId)
                    .Select(result => new
                    {
                        StudentFirstName = result.Student.StudentFirstName,
                        StudentLastName = result.Student.StudentLastName
                    })
                    .ToList();

                // Display students' names in the selected class
                Console.WriteLine($"\nStudents in ClassID: {selectedClassId}");
                foreach (var student in studentsInClass)
                {
                    Console.WriteLine($"StudentName: {student.StudentFirstName} {student.StudentLastName}");
                }
            }
        }

        public void GetGradesWithDetails()
        {
            using (var dbContext = new HogwartzContext())
            {
                DateTime startDate = new DateTime(2023, 11, 14);
                DateTime endDate = new DateTime(2023, 12, 14);

                var gradesWithDetails = dbContext.Grades
                    .Where(grade => grade.GradeDateSet >= startDate && grade.GradeDateSet <= endDate)
                    .Join(dbContext.SetGrades,
                        grade => grade.GradesId,
                        setGrade => setGrade.FkGradesId,
                        (grade, setGrade) => new { grade, setGrade })
                    .Join(dbContext.Classes,
                        combined => combined.setGrade.FkClassId,
                        classes => classes.ClassId,
                        (combined, classes) => new { combined.grade, combined.setGrade, classes })
                    .Join(dbContext.Students,
                        combined => combined.setGrade.FkStudentId,
                        student => student.StudentId,
                        (combined, student) => new
                        {
                            combined.grade.GradesId,
                            combined.grade.GradeDateSet,
                            combined.grade.GradesInfo,
                            student.StudentFirstName,
                            student.StudentLastName,
                            combined.classes.ClassName
                        })
                    .ToList();

                foreach (var gradeDetails in gradesWithDetails)
                {
                    Console.WriteLine($"GradeId: {gradeDetails.GradesId}, GradeDateSet: {gradeDetails.GradeDateSet}, GradesInfo:" +
                        $" {gradeDetails.GradesInfo}, StudentFirstName: {gradeDetails.StudentFirstName}, StudentLastName: " +
                        $"\n{gradeDetails.StudentLastName}, ClassName: {gradeDetails.ClassName}");

                }
            }
        }

        public void ClassNlowHighGrades()
        {
            using (var dbContext = new HogwartzContext())
            {
                var result = dbContext.Grades
                    .Join(dbContext.SetGrades, grade => grade.GradesId, setGrade => setGrade.FkGradesId, (grade, setGrade)
                    => new { grade, setGrade })
                    .Join(dbContext.Classes, combined => combined.setGrade.FkClassId, classes => classes.ClassId, (combined, classes)
                    => new { combined.grade, combined.setGrade, classes })
                    .Join(dbContext.Students, combined => combined.setGrade.FkStudentId, student => student.StudentId, (combined, student)
                    => new
                    {
                        combined.grade.GradeDateSet,
                        combined.grade.GradesInfo,
                        combined.setGrade.FkClassId,
                        combined.setGrade.FkStudentId,
                        combined.classes.ClassName
                    })
                    .GroupBy(result => new { result.GradeDateSet, result.FkClassId, result.FkStudentId, result.ClassName })
                    .Select(group => new
                    {
                        GradeDateSet = group.Key.GradeDateSet,
                        MaxGradesInfo = group.Max(item => item.GradesInfo),
                        MinGradesInfo = group.Min(item => item.GradesInfo),
                        ClassName = group.Key.ClassName
                    })
                    .ToList();

                foreach (var item in result)
                {
                    Console.WriteLine($"GradeDateSet: {item.GradeDateSet}, MaxGradesInfo: {item.MaxGradesInfo}, MinGradesInfo: {item.MinGradesInfo}," +
                        $" ClassName: {item.ClassName}");
                }
            }
        }

        public void averageGradeLowNhigh()  // CANT USE BEACUSE my grades are in NVARCHAR isntead of exempel INT
        {
            using (var dbContext = new HogwartzContext())
            {
                var result = dbContext.Grades
                    .Join(dbContext.SetGrades, grade => grade.GradesId, setGrade => setGrade.FkGradesId, (grade, setGrade) =>
                    new { grade, setGrade })
                    .Join(dbContext.Classes, combined => combined.setGrade.FkClassId, classes => classes.ClassId, (combined, classes) =>
                    new { combined.grade, combined.setGrade, classes })
                    .Join(dbContext.Students, combined => combined.setGrade.FkStudentId, student => student.StudentId, (combined, student) =>
                    new
                    {
                        combined.grade.GradeDateSet,
                        combined.grade.GradesInfo,
                        combined.setGrade.FkClassId,
                        combined.setGrade.FkStudentId,
                        combined.classes.ClassName
                    })
                    .GroupBy(result => new { result.GradeDateSet, result.FkClassId, result.FkStudentId, result.ClassName })
                    .Select(group => new
                    {
                        GradeDateSet = group.Key.GradeDateSet,
                        MaxGradesInfo = group.Max(item => int.Parse(item.GradesInfo)),
                        MinGradesInfo = group.Min(item => int.Parse(item.GradesInfo)),
                        AvgGradesInfo = group.Average(item => double.Parse(item.GradesInfo)),
                        ClassName = group.Key.ClassName
                    })
                    .ToList();

                foreach (var item in result)
                {
                    Console.WriteLine($"GradeDateSet: {item.GradeDateSet}, MaxGradesInfo: {item.MaxGradesInfo}, MinGradesInfo: " +
                        $"{item.MinGradesInfo}, AvgGradesInfo: {item.AvgGradesInfo}, ClassName: {item.ClassName}");
                }
            }
        }
        public void AddUser()
        {
            using (HogwartzContext context = new HogwartzContext())
            {
                Console.Write("Enter Student First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Enter Student Last Name: ");
                string lastName = Console.ReadLine();

                Student student813 = new Student()
                {
                    StudentFirstName = firstName,
                    StudentLastName = lastName
                };

                context.Students.Add(student813);
                context.SaveChanges();

                Console.WriteLine("Student added successfully!");
            }
        }

        public void AddProffesor()
        {
            using (HogwartzContext context = new HogwartzContext())
            {

                Console.Write("Enter Proffesor id: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter Proffesor First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Enter Proffesor Last Name: ");
                string lastName = Console.ReadLine();

                Proffesor proffesor16 = new Proffesor()
                {
                    ProffesorId = id,
                    FirstName = firstName,
                    LastName = lastName
                };

                context.Proffesors.Add(proffesor16);
                context.SaveChanges();

                Console.WriteLine("Proffesor added successfully!");
            }

        }
    }
}


