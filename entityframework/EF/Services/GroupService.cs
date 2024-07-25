using EFCoreWithEntity.Constants;
using EFCoreWithEntity.Contexts;
using EFCoreWithEntity.Entities;
using EFCoreWithEntity.Extensions;
using EFCoreWithEntity.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreWithEntity.Services
{
    public static class GroupService
    {
        private static readonly AppDbContext _context;
        static GroupService()
        {
            _context = new AppDbContext();
        }
        public static void GetAllGroups()
        {
            foreach (var group in _context.Groups.ToList())
            {
                Console.WriteLine($"Id: {group.Id}, Name: {group.Name}, Limit: {group.Limit}, TeacherId: {group.TeacherId}");
            }
        }
        public static void AddGroup()
        {
            if (_context.Teachers.Count() == 0)
            {
                Messages.WarningMessage("Teacher");
                return;
            }
        GroupNameInput: Messages.InputMessage("Group name");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMessages("Group name");
                goto GroupNameInput;
            }
            var existGroup = _context.Groups.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if (existGroup is not null)
            {
                Messages.AlreadyExistMessage($"Group name - {name}");
                goto GroupNameInput;
            }
            Messages.InputMessage("Group limit");
            string limitInpur = Console.ReadLine();
            int limit;
            bool isSucceeded = int.TryParse(limitInpur, out limit);
            if (!isSucceeded)
            {
                Messages.InvalidInputMessages("Group limit");
                goto GroupNameInput;
            }
            if (!isSucceeded || limit <= 0)
            {
                Messages.InvalidInputMessages("Group limit");
                goto GroupNameInput;
            }
        GroupBeginDateInput: Messages.InputMessage("Group begin date (format : dd.MM.yyyy)");
            string beginDateInput = Console.ReadLine();
            DateTime beginDate;
            isSucceeded = DateTime.TryParseExact(beginDateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out beginDate);
            if (!isSucceeded)
            {
                Messages.InvalidInputMessages("Group end date");
                goto GroupBeginDateInput;
            }
        GroupEndDateInput: Messages.InputMessage("Group end date (format : dd.MM.yyyy),the difference should be at least 6 months");
            string endDateInput = Console.ReadLine();
            DateTime endDate;
            isSucceeded = DateTime.TryParseExact(endDateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
            if (!isSucceeded || beginDate.Date.AddMonths(6).Date > endDate.Date)
            {
                Messages.InvalidInputMessages("Group end date");
                goto GroupEndDateInput;
            }
        TeacherList: TeacherService.GetAllTeacher();
            Messages.InputMessage("Teacher id");
            string teacherIdInput = Console.ReadLine();
            int id;
            isSucceeded = int.TryParse(teacherIdInput, out id);
            if (!isSucceeded)
            {
                Messages.InvalidInputMessages("Teachermid");
                goto TeacherList;
            }
            var teacher = _context.Teachers.Find(id);
            if (teacher is null)
            {
                Messages.NotFoundMessage("Teacher");
                goto TeacherList;
            }
            var group = new Group
            {
                Name = name,
                Limit = limit,
                BeginDate = beginDate,
                EndDate = endDate,
                TeacherId = teacher.Id
            };
            _context.Groups.Add(group);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }
            Messages.SuccessMessage("Group", "added");
        }
        public static void UpdateGroup()
        {
        GroupIdInput: GetAllGroups();
            Messages.InputMessage("Group id");
            string IdInput = Console.ReadLine();
            int groupId;
            bool isSucceeded = int.TryParse(IdInput, out groupId);
            if (!isSucceeded)
            {
                Messages.InvalidInputMessages("Group's id");
                goto GroupIdInput;
            }
            var existGroup = _context.Groups.Find(groupId);
            if (existGroup is null)
            {
                Messages.NotFoundMessage("Group");
                goto GroupIdInput;
            }
            string Name = existGroup.Name;
        GroupNameInput: Messages.WantToChangeMessage("Name");
            string input = Console.ReadLine();
            isSucceeded = char.TryParse(input, out char answer);
            if (!isSucceeded || !answer.IsvalidChoice())
            {
                Messages.InvalidInputMessages("answer");
                goto GroupNameInput;
            }
            var ExistGroupName = _context.Groups.FirstOrDefault(n => n.Name.ToLower() == Name.ToLower() && n.Id != groupId);
            if (ExistGroupName is not null)
            {
                Messages.AlreadyExistMessage("Group name");
                goto GroupNameInput;
            }
            int Limit = existGroup.Limit;
             GroupLimitInput: Messages.WantToChangeMessage("Limit");
            input = Console.ReadLine();
            isSucceeded = char.TryParse(input, out answer);
            if (!isSucceeded || !answer.IsvalidChoice())
            {
                Messages.InvalidInputMessages("Answer");
                goto GroupLimitInput;
            }
            if (answer == 'y')
            {
            LimitInput: Messages.InputMessage("Limit");
                string newLimitInput = Console.ReadLine();
                isSucceeded = int.TryParse(newLimitInput, out Limit);
                if (!isSucceeded)
                {
                    Messages.InvalidInputMessages("Limit");
                    goto LimitInput;
                }
                var CountStudentInGroup = _context.Students.Where(x => x.GroupId == groupId).Count();
                if (CountStudentInGroup > Limit)
                {
                    Messages.LimitMessage("Limit", CountStudentInGroup);
                }
                BeginDateInput: Messages.WantToChangeMessage("Begin date");
                input = Console.ReadLine();
                isSucceeded = char.TryParse(input, out answer);
                if (isSucceeded || answer.IsvalidChoice())
                {
                    Messages.InvalidInputMessages("Answer");
                    goto BeginDateInput;
                }
                DateTime beginDate = existGroup.BeginDate;
                if (answer == 'y')
                {
                BeginDate: Messages.InputMessage("Begin date (format: dd.MM.yyyy)");
                    input = Console.ReadLine();
                    isSucceeded = DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out beginDate);
                    if (!isSucceeded)
                    {
                        Messages.InvalidInputMessages("Begin date");
                        goto BeginDate;
                    }
                }
                 EndDateInput: Messages.WantToChangeMessage("End date");
                input = Console.ReadLine();
                isSucceeded = char.TryParse(input, out answer);
                if (isSucceeded || answer.IsvalidChoice())
                {
                    Messages.InvalidInputMessages("Answer");
                    goto EndDateInput;
                }
                DateTime endDate = existGroup.EndDate;
                if (answer == 'y')
                {
                EndDate: Messages.InputMessage("end date (format: dd.MM.yyyy)");
                    input = Console.ReadLine();
                    isSucceeded = DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out beginDate);
                    if (!isSucceeded)
                    {
                        Messages.InvalidInputMessages("End date");
                        goto EndDate;
                    }
                }
                if (!isSucceeded || beginDate.Date.AddMonths(6).Date > endDate.Date)
                {
                    Messages.InvalidInputMessages("Group end date");
                    goto EndDateInput;
                }
                int teacherId = existGroup.TeacherId;
                if(_context.Teachers.Count() > 1)
                {
                TeacherInput: Messages.WantToChangeMessage("Teacher");
                    input = Console.ReadLine();
                    isSucceeded = char.TryParse(input,out answer);
                    if (!isSucceeded || answer.IsvalidChoice())
                    {
                        Messages.InvalidInputMessages("answer");
                        goto TeacherInput;
                    }
                    if (answer == 'y')
                    {
                    TeacherId: TeacherService.GetAllTeacher();
                        Messages.InputMessage("Teacher id");
                        string TeacherIdInput = Console.ReadLine();
                        isSucceeded = int .TryParse(TeacherIdInput,out teacherId);
                        if (!isSucceeded)
                        {
                            Messages.InvalidInputMessages("Teacher id");
                            goto TeacherInput;
                        }
                        var existTeacher = _context.Teachers.FirstOrDefault(x => x.Id !=existGroup.TeacherId && x.Id == teacherId);
                        if (existTeacher == null)
                        {
                            Messages.NotFoundMessage("Id");
                            goto TeacherId;
                        }
                    }
                }
                existGroup.Name = Name;
                existGroup.Limit = Limit;
                existGroup.TeacherId = teacherId;
                existGroup.BeginDate = beginDate;
                existGroup.EndDate = endDate;
                _context.Groups.Update(existGroup);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Messages.ErrorOccuredMessage();
                }
                Messages.SuccessMessage("Group", "Updated");
            }
       
           
        }
        public static void GetDetailOfGroup()
        {
            GetAllGroups();
        InputGroupId: Messages.InputMessage("Group id");
            string inputId = Console.ReadLine();
            int groupId;
            bool isSucceeded = int.TryParse(inputId, out groupId);
            if(!isSucceeded)
            {
                Messages.InvalidInputMessages("Group id");
                goto InputGroupId;
            }
            var group = _context.Groups.Include(x => x.Teacher).Include(x => x.Students).FirstOrDefault(x => x.Id == groupId);
            if(group is null)
            {
                Messages.NotFoundMessage("Group");
                return;
            }
            Console.WriteLine($"{group.Name},{group.Limit},{group.Teacher.Name}");
            foreach (var student in group.Students) 
            {
                Console.WriteLine($"{student.Name},{student.Surname}");
            }
        }
        public static void DeleteGroup()
        {
            GetAllGroups();
        InputGroupId: Messages.InputMessage("Group Id");
            string inputId = Console.ReadLine();
            int groupId;
            bool isSucceeded = int.TryParse(inputId, out groupId);
            if (!isSucceeded)
            {
                Messages.InvalidInputMessages("Group Id");
                goto InputGroupId;
            }
            var group = _context.Groups.Find();
            if (group is null)
            {
                Messages.NotFoundMessage("Group");
                return;
            }
            group.IsDeleted = true;
            _context.Groups.Update(group);
            try
            {
                _context.SaveChanges();
            }
            catch(Exception)
            {
                Messages.ErrorOccuredMessage();
                ;
            }
            Messages.SuccessMessage("Group", "deleted");
            

            
        }
    }
}


        
    

