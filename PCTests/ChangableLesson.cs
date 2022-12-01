using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTITSTimetableApp
{
    internal class ChangableLesson : Lesson
    {
        public int CurrentVar { get; set; }
        public List<Lesson> Variants { get; set; }
        public new string TeacherName { get => Variants[CurrentVar].TeacherName; set => Variants [CurrentVar].TeacherName = value; }
        public new string LessonName { get => Variants[CurrentVar].LessonName; set => Variants[CurrentVar].LessonName = value; }
        public new int ClassNo { get => Variants[CurrentVar].ClassNo; set => Variants[CurrentVar].ClassNo = value; }
    }
}
