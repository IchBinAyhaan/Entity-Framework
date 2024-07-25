using EFCoreWithEntity.Constants;
using EFCoreWithEntity.Contexts;
using EFCoreWithEntity.Entities;
using EFCoreWithEntity.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreWithEntity.Services
{
    public class StudentService
    {
        private static readonly AppDbContext _context;

        static StudentService()
        {
            _context = new AppDbContext();
        }

        public static void GetAllStudents()
        {
            foreach (var student in _context.Students.Where(s => !s.IsDeleted).ToList())
            {
                Console.WriteLine($"id:{student.Id},Name:{student.Name},Surname:{student.Surname}");
            }
        }

        public static void AddStudent()
        {
        StudentNameInput:Messages.InputMessage("student name");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMessages("student name");
                goto StudentNameInput;
            }

        StudentSurnameInput: Messages.InputMessage("student surname");
            string surname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(surname))
            {
                Messages.InvalidInputMessages("student surname");
                goto StudentSurnameInput;
            }

            Student student = new Student()
            {
                Name = name,
                Surname = surname
            };

            _context.Students.Add(student);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }

            Messages.SuccessMessage("student", "added");
        }

        public static void UpdateStudent()
        {
            GetAllStudents();

        StudentIdInput: Messages.InputMessage("Student id");
            var idInput = Console.ReadLine();
            int studentId;
            bool isSucceeded = int.TryParse(idInput, out studentId);
            if (!isSucceeded)
            {
                Messages.InvalidInputMessages("Student id");
                goto StudentIdInput;
            }

            var student = _context.Students.Find(studentId);
            if (student is null)
            {
                Messages.NotFoundMessage("Student");
                return;
            }

        StudentNameInput: Messages.WantToChangeMessage("name");
            var choiceInput = Console.ReadLine();
            char choice;
            isSucceeded = char.TryParse(choiceInput, out choice);
            if (!isSucceeded || !choice.IsvalidChoice())
            {
                Messages.InvalidInputMessages("choice");
                goto StudentNameInput;
            }

            string newName = string.Empty;
            if (choice == 'y')
            {
            NewNameInput:Messages.InputMessage("new name");
                newName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newName))
                {
                    goto NewNameInput;
                }
            }

        StudentSurnameInput: Messages.WantToChangeMessage("surname");
            choiceInput = Console.ReadLine();
            isSucceeded = char.TryParse(choiceInput, out choice);
            if (!isSucceeded || !choice.IsvalidChoice())
            {
                Messages.InvalidInputMessages("choice");
                goto StudentSurnameInput;
            }

            string newSurname = string.Empty;
            if (choice == 'y')
            {
            NewSurnameInput:Messages.InputMessage("new surname");
                newSurname = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newSurname))
                {
                    goto NewSurnameInput;
                }
            }

            if (!string.IsNullOrEmpty(newName)) student.Name = newName;
            if (!string.IsNullOrEmpty(newSurname)) student.Surname = newSurname;

            _context.Students.Update(student);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }

            Messages.SuccessMessage("Student", "updated");
        }

        public static void DeleteStudent()
        {
            GetAllStudents();

        StudentIdInput: Messages.InputMessage("Student id");
            var idInput = Console.ReadLine();
            int id;
            bool isSucceeded = int.TryParse(idInput, out id);
            if (!isSucceeded)
            {
                Messages.InvalidInputMessages("Student id");
                goto StudentIdInput;
            }

            var student = _context.Students.Find(id);
            if (student is null)
            {
                Messages.NotFoundMessage("Student");
                return;
            }

            student.IsDeleted = true;
            _context.Students.Update(student);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }

            Messages.SuccessMessage("Student", "deleted");
        }

        public static void GetStudentDetails()
        {
            GetAllStudents();

        StudentIdInput:Messages.InputMessage("Student id");
            var idInput = Console.ReadLine();
            int id;
            bool isSucceeded = int.TryParse(idInput, out id);
            if (!isSucceeded)
            {
                Messages.InvalidInputMessages("Student id");
                goto StudentIdInput;
            }

            var student = _context.Students.Include(x => x.Group).FirstOrDefault(x => x.Id == id);
            if (student is null)
            {
                Messages.NotFoundMessage("Student");
                return;
            }

            Console.WriteLine($"id:{student.Id},Name:{student.Name},Surname: {student.Surname}");
            if (student.Group != null)
            {
                Console.WriteLine($"Group: {student.Group.Name}");
            }
        }
    }
}
