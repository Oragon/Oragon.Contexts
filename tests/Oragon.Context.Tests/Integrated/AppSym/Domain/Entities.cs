using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Context.Tests.Integrated.AppSym.Domain
{
    public partial class Classroom
    {
        public virtual int ClassroomId { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<Student> Students { get; set; }
    }

    public partial class Language
    {
        public virtual string LanguageId { get; set; }

        public virtual IList<Student> Students { get; set; }
    }

    public partial class Student
    {
        public virtual int StudentId { get; set; }

        public virtual string FullName { get; set; }

        public virtual IList<Classroom> Classrooms { get; set; }

        public virtual Language Language { get; set; }
    }
}
