using Oragon.Context.Tests.Integrated.AppSym.Data.Repositories;
using Oragon.Context.Tests.Integrated.AppSym.Domain;
using Oragon.Contexts.NHibernate;
using Oragon.Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oragon.Context.Tests.Integrated.AppSym.Services
{
    public class TestService : ITestService
    {
        #region Injected

        [Required]
        DomainEntityRepository DomainEntityRepository { get; set; }


        [Required]
        StudentQueryRepository StudentQueryRepository { get; set; }


        [Required]
        PermissiveRepository PermissiveRepository { get; set; }

        #endregion

        /// <summary>
        /// Test save with no relation
        /// </summary>
        [NHContext("OragonSamples", true)]
        public void Create1()
        {
            Console.WriteLine("TestService.Create() Init");

            Language languageEN = new Language() { LanguageId = "EN", Name = "English" };
            Language languageES = new Language() { LanguageId = "ES", Name = "Spanish" };

            Classroom classroom1 = new Classroom() { Name = "Class 1" };
            Classroom classroom2 = new Classroom() { Name = "Class 2" };

            this.Do(this.DomainEntityRepository.Save, languageEN, languageES, classroom1, classroom2);

            Console.WriteLine("TestService.Create() End");
        }

        /// <summary>
        /// Test save with relations
        /// </summary>
        [NHContext("OragonSamples", true)]
        public void Create2()
        {
            Classroom classroom3 = new Classroom() { Name = "Class 3" };
            Language languagePT = new Language() { LanguageId = "PT", Name = "Portuguese" };
            Student studentLuiz = new Student()
            {
                FullName = "Luis Carlos Faria",
                Language = languagePT,
                Classrooms = new List<Classroom>() {
                    classroom3
                }
            };

            Student studentTatiana = new Student()
            {
                FullName = "Tatiana",
                Language = languagePT,                
            };

            Classroom classroom4 = new Classroom() { 
                Name = "Class 4",
                Students = new List<Student> { studentTatiana }
                
            };

            this.Do(this.DomainEntityRepository.Save, classroom3, classroom4, languagePT, studentLuiz, studentTatiana);

        }

        /// <summary>
        /// Simplify operations
        /// </summary>
        /// <param name="action">Action to do</param>
        /// <param name="entities">Entities to perform the action</param>
        private void Do(Action<DomainEntity> action, params DomainEntity[] entities)
        {
            if (entities != null && entities.Any())
            {
                foreach (DomainEntity entity in entities)
                {
                    action(entity);
                }
            }
        }


        [NHContext("OragonSamples", false)]
        public List<DomainEntity> RetrieveAll()
        {
            List<DomainEntity> returnValue = new List<DomainEntity>();
            Console.WriteLine("TestService.Retrieve2() Init");

            var session = this.PermissiveRepository.GetSession();

            returnValue.AddRange(session.Query<Student>().ToArray());
            returnValue.AddRange(session.Query<Language>().ToArray());
            returnValue.AddRange(session.Query<Classroom>().ToArray());


            this.Do(session.Evict, returnValue.ToArray());

            Console.WriteLine("TestService.Retrieve2() End");
            
            return returnValue;
        }

        [NHContext("OragonSamples", true)]
        public void Update()
        {
            Console.WriteLine("TestService.Update() Init");




            Console.WriteLine("TestService.Update() End");
        }

        [NHContext("OragonSamples", true)]
        public void Delete()
        {
            Console.WriteLine("TestService.Delete() Init");


            Console.WriteLine("TestService.Delete() End");
        }
    }
}
