using KTITSTimetableApp;
using System.ComponentModel.Design.Serialization;
using System.Net.Http.Json;
using System.Text.Json;
namespace PCTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var d1 = new List<Lesson>() { null, new Lesson() { ClassNo = 123, LessonName = "testing", TeacherName = "test name ofteacher" }, new Lesson() { ClassNo = 456, LessonName = "testing2", TeacherName = "test name ofteacher2546456" } };
            var d2 = new List<Lesson>() { new Lesson() { ClassNo = 123, LessonName = "testing", TeacherName = "test name ofteacher" }, null , new Lesson() { ClassNo = 456, LessonName = "testing2", TeacherName = "test name ofteacher2546456" } };
            var d3 = new List<Lesson>() { null, new Lesson() { ClassNo = 123, LessonName = "testing", TeacherName = "test name ofteacher" }, new Lesson() { ClassNo = 456, LessonName = "testing2", TeacherName = "test name ofteacher2546456" } };

            List<List<Lesson>> list = new List<List<Lesson>>() { d1, d2, d3 };
            var str = JsonSerializer.Serialize(list, new JsonSerializerOptions() { WriteIndented = true});
            Console.WriteLine(str);
        }
    }
}