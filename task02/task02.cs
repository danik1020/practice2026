using System.Linq;
namespace task02{

public class StudentService
{
    private readonly List<Student> _students;

    public StudentService(List<Student> students) => _students = students;

    public IEnumerable<Student> GetStudentsByFaculty(string faculty) 
    {
        var selectedStudents = _students.Where(s => s.Faculty == faculty);
        return selectedStudents;
    }
  
    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
    {
        var selectedStudents = _students.Where(s => s.Grades.Average() >= minAverageGrade);
        return selectedStudents;
    }


    public IEnumerable<Student> GetStudentsOrderedByName()
    {
        var sortedStudents = from s in _students
                            orderby s.Name
                            select s;
        return sortedStudents;
    }

    public ILookup<string, Student> GroupStudentsByFaculty()
    { return _students.ToLookup(s => s.Faculty); }

    public string GetFacultyWithHighestAverageGrade()
    {
        if (_students == null) return null;

        return _students.GroupBy(static s => s.Faculty).MaxBy(keySelector: static g => g.SelectMany(static s => s.Grades).Average()).Key;
    }

}
public class Student
{
    public string Name { get; set; }
    public string Faculty { get; set; }
    public List<int> Grades { get; set; }
}
}
