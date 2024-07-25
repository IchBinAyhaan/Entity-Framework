using Azure;
using EFCoreWithEntity.Constants;
using EFCoreWithEntity.Services;
namespace EFCoreWithEntity
{
    public class Program
    {
        public static void Main()
        {
            while (true)
            {
                ShowMenu();
                Messages.InputMessage("choice");
                string choiceInput = (Console.ReadLine());
                int choice;
                bool isSucceeded = int.TryParse(choiceInput, out choice);
                if (isSucceeded)
                {
                    switch ((Operations)choice)
                    {
                        case Operations.AllTeachers:
                            TeacherService.GetAllTeacher();
                            break;
                        case Operations.CreateTeacher:
                            TeacherService.AddTeacher();
                            break;
                        case Operations.UpdateTeacher:
                            TeacherService.UpdateTeacher();                          
                            break;
                        case Operations.RemoveTeacher:
                            TeacherService.DeleteTeacher();
                            break;
                        case Operations.DetailsTeacher:
                            TeacherService.GetTeacherDetails();
                            break;
                        case Operations.AllGroups:
                            GroupService.GetAllGroups();
                            break;
                        case Operations.CreateGroup:
                            GroupService.AddGroup();
                            break;
                        case Operations.UpdateGroup:
                            GroupService.UpdateGroup();
                            break;
                        case Operations.RemoveGroup:
                            GroupService.DeleteGroup();
                            break;
                        case Operations.DetailsGroup:
                            GroupService.GetDetailOfGroup();
                            break;
                        case Operations.AllStudents:
                            StudentService.GetAllStudents();
                            break;
                        case Operations.CreateStudent:
                            StudentService.AddStudent();
                            break;
                        case Operations.UpdateStudent:
                            StudentService.UpdateStudent();
                            break;
                        case Operations.RemoveStudent:
                            StudentService.DeleteStudent();
                            break;
                        case Operations.DetailsStudent:
                            StudentService.GetStudentDetails();
                            break;

                        case Operations.Exit:
                            return;
                        default:
                            Messages.InvalidInputMessages("Choice");
                            break;
                    }
                }
                else
                {
                    Messages.InvalidInputMessages("Choice");
                }

            }

        }
        public static void ShowMenu()
        {
            Console.WriteLine("---MENU----");
            Console.WriteLine("1.All teachers ");
            Console.WriteLine("2. Add teacher");
            Console.WriteLine("3. Update teacher");
            Console.WriteLine("4. Delete teacher");
            Console.WriteLine("5. Details of teacher");
            Console.WriteLine("6.All groups ");
            Console.WriteLine("7. Add group");
            Console.WriteLine("8. Update group");
            Console.WriteLine("9. Delete group");
            Console.WriteLine("10. Details of group");
            Console.WriteLine("6.All students ");
            Console.WriteLine("7. Add student");
            Console.WriteLine("8. Update student");
            Console.WriteLine("9. Delete student");
            Console.WriteLine("10. Details of student");
            Console.WriteLine("0. Exit");
        }

    }
}

