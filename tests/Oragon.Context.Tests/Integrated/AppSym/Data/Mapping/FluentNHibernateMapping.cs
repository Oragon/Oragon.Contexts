using FluentNHibernate.Mapping;
using Oragon.Context.Tests.Integrated.AppSym.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Context.Tests.Integrated.AppSym.Data.Mapping
{
    public partial class ClassroomMapper : ClassMap<Classroom>
    {

        partial void CompleteMappings();

        public ClassroomMapper()
        {
            Table("CLASSROOM");

            OptimisticLock.None();

            DynamicUpdate();
            Id(it => it.ClassroomId, "CLASSROOM_ID").GeneratedBy.Native("CLASSROOM_SEQ").Index("PK_CLASSROOM").CustomSqlType("int");
            HasManyToMany(x => x.Students)
                .ParentKeyColumns.Add("CLASSROOM_ID", p=> p.UniqueKey("PK_STUDENT_CLASSROOM"))
                .Table("STUDENT_CLASSROOM")
                .ChildKeyColumns.Add("STUDENT_ID", p => p.UniqueKey("PK_STUDENT_CLASSROOM"))
                .ForeignKeyConstraintNames("FK_CLASSROOM_TO_STUDENT_CLASSROOM", "FK_STUDENT_TO_STUDENT_CLASSROOM")                
                .LazyLoad()
                .Fetch.Select()
                .AsBag();
            Map(it => it.Name, "NAME").Not.Nullable().CustomSqlType("VARCHAR(100)");
            this.CompleteMappings();
        }

    }


    public partial class StudentMapper : ClassMap<Student>
    {

        partial void CompleteMappings();

        public StudentMapper()
        {
            Table("STUDENT");

            OptimisticLock.None();

            DynamicUpdate();
            Id(it => it.StudentId, "STUDENT_ID").GeneratedBy.Native("STUDENT_SEQ").Index("PK_STUDENT").CustomSqlType("int");
            HasManyToMany(x => x.Classrooms)
                .ParentKeyColumns.Add("STUDENT_ID", p => p.UniqueKey("PK_STUDENT_CLASSROOM"))
                .Table("STUDENT_CLASSROOM")
                .ChildKeyColumns.Add("CLASSROOM_ID", p => p.UniqueKey("PK_STUDENT_CLASSROOM"))
                .ForeignKeyConstraintNames("FK_STUDENT_TO_STUDENT_CLASSROOM", "FK_CLASSROOM_TO_STUDENT_CLASSROOM")
                .LazyLoad()
                .Fetch.Select()
                .AsBag();
            Map(it => it.FullName, "FULL_NAME").Not.Nullable().CustomSqlType("VARCHAR(300)");
            this.CompleteMappings();
        }

    }

}
