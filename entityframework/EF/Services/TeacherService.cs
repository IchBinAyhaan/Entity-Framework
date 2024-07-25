
using EFCoreWithEntity.Constants;
using EFCoreWithEntity.Contexts;
using EFCoreWithEntity.Entities;
using EFCoreWithEntity.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreWithEntity.Services
{
    public static class TeacherService
    {
        private static readonly AppDbContext _context;
        static TeacherService()
        {
           _context = new AppDbContext();
        }

        public static void GetAllTeacher()
        {
            foreach(var teacher in _context.Teachers.Where(t => !t.IsDeleted).ToList())
            {
                Console.WriteLine($"id:{teacher.Id},Name:{teacher.Name},Surname:{teacher.Surname}");
            }
        } 
        public static void AddTeacher()
        {
        TeacherNameInput: Messages.InputMessage("teacher name");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMessages("teacher name");
                goto TeacherNameInput;
            }
        TeacherSurnameInput: Messages.InputMessage("teacher surname");
            string surname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(surname))
            {
                Messages.InvalidInputMessages("teacher surname");
                goto TeacherSurnameInput;
            }
            Teacher teacher = new Teacher()
            {
                Name = name,
                Surname = surname
            };
            _context.Teachers.Add(teacher);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }
            Messages.SuccessMessage("teacher","added");

           
       






        }
        public static void UpdateTeacher()
        {
            GetAllTeacher();
        TeacherIdInput: Messages.InputMessage("Teacher id");
            var id = Console.ReadLine();
            int teacherId;
            bool isSucceeded = int.TryParse(id, out teacherId);
            if (isSucceeded)
            {
                Messages.InvalidInputMessages("Teacher id");
                goto TeacherIdInput;
            }
            var teacher = _context.Teachers.Find(teacherId);
            if (teacher is null)
            {
                Messages.NotFoundMessage("Teacher");
                return;
            }
            TeacherNameInput: Messages.WantToChangeMessage("name");
            var choiceInput = Console.ReadLine();
            char choice;
            isSucceeded = char.TryParse(choiceInput, out choice);
            if (isSucceeded || !choice .IsvalidChoice())
            {
                Messages.InvalidInputMessages("choice");
                goto TeacherNameInput;
            }
            string newName = string.Empty;
           if(choice == 'y')
            {
                NewNameInput: Messages.InputMessage("new name");
                 newName = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(newName))
                {
                    goto NewNameInput;
                }

            }
        TeacherSurnameInput: Messages.WantToChangeMessage("surname");
           choiceInput = Console.ReadLine();
            isSucceeded = char.TryParse(choiceInput,out choice);
            if (!isSucceeded || !choice.IsvalidChoice())
            {
                Messages.InvalidInputMessages("choice");
                goto TeacherSurnameInput;
            }
            string newSurname = string.Empty;

            if (choice == 'y')
            {
            NewSurnameInput: Messages.InputMessage("new surname");
                 newSurname = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(newSurname))
                {
                    goto NewSurnameInput;
                }
            }
            if(string.IsNullOrEmpty(newName)) teacher.Name = newName;
            if(string.IsNullOrEmpty(newSurname)) teacher.Surname = newSurname;

            _context.Teachers.Update(teacher);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }
            Messages.SuccessMessage("Teacher", "updated");
        }
        public static void DeleteTeacher()
        {
            GetAllTeacher();
        TeacherIdInput: Messages.InputMessage("Teacher id");
            var idInput = Console.ReadLine();
            int id;
            bool isSucceeded = int.TryParse(idInput,out id);
            if (isSucceeded)
            {
                Messages.InvalidInputMessages("Teacher id");
                goto TeacherIdInput;
            }
            var teacher = _context.Teachers.Find(id);
            if (teacher is null)
            {
                Messages.NotFoundMessage("Teacher");
                return;
            }
         teacher.IsDeleted = true;
            _context.Teachers.Update(teacher);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }
            Messages.SuccessMessage("Teacher", "deleted");
        }
        public static void GetTeacherDetails()
        {
            GetAllTeacher();
        TeacherIdInput: Messages.InputMessage("Teacher id");
            var idInput = Console.ReadLine();
            int id;
            bool isSucceeded = int.TryParse(idInput, out id);
            if (isSucceeded)
            {
                Messages.InvalidInputMessages("Teacher id");
                goto TeacherIdInput;
            }
            var teacher = _context.Teachers.Include(x => x.groups).FirstOrDefault(x => x.Id == id);
            if(teacher is null)
            {
                Messages.NotFoundMessage("Teacher");
                return;
            }
            Console.WriteLine($"id:{teacher.Id},Name:{teacher.Name},Surname: {teacher.Surname}");
            foreach(var group in teacher.groups)
                Console.WriteLine(group.Name);


        }

    }
}
    

