using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CrudWindowsFormsLinq
{
    public class PeopleLinqDB
    {
        private readonly string connectionString =
            @"Data Source=.;Initial Catalog=CrudWindowsForms;Integrated Security=True;";

        public List<Person> Get()
        {
            using (PersonasDataContext db = new PersonasDataContext(connectionString))
            {
                return db.Person.OrderBy(p => p.Id).ToList();
            }
        }

        public Person GetById(int id)
        {
            using (PersonasDataContext db = new PersonasDataContext(connectionString))
            {
                return db.Person.SingleOrDefault(p => p.Id == id);
            }
        }

        public int Add(string name, int age)
        {
            using (PersonasDataContext db = new PersonasDataContext(connectionString))
            {
                Person person = new Person { Name = name, Age = age };
                db.Person.InsertOnSubmit(person);
                db.SubmitChanges();
                return person.Id;
            }
        }

        public bool Update(int id, string name, int age)
        {
            using (PersonasDataContext db = new PersonasDataContext(connectionString))
            {
                Person person = db.Person.SingleOrDefault(p => p.Id == id);
                if (person == null)
                    return false;

                person.Name = name;
                person.Age = age;
                db.SubmitChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            using (PersonasDataContext db = new PersonasDataContext(connectionString))
            {
                Person person = db.Person.SingleOrDefault(p => p.Id == id);
                if (person == null)
                    return false;

                db.Person.DeleteOnSubmit(person);
                db.SubmitChanges();
                return true;
            }
        }

        public List<Person> Search(string text)
        {
            using (PersonasDataContext db = new PersonasDataContext(connectionString))
            {
                return db.Person
                    .Where(p => p.Name.Contains(text))
                    .OrderBy(p => p.Name)
                    .ToList();
            }
        }
    }
}