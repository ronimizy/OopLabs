using Isu.Entities;
using Isu.Models;

namespace Isu.Tools
{
    internal static class IsuExceptionFactory
    {
        public static IsuException InvalidCourseNumberException(int value, string details = "")
            => new ($"{value} - is an invalid value for course number. {details}");

        public static IsuException InvalidGroupNameException(string name, string details = "")
            => new ($"'{name}' - is invalid value for group name. {details}");

        public static IsuException ExistingFacultyException(string name, char letter)
            => new ($"Faculty called '{name}' or with letter '{letter}' already exists");

        public static IsuException NonExistingFacultyException(char letter)
            => new ($"Faculty with letter '{letter}' doesn't exist");

        public static IsuException ExistingCourseException(Faculty faculty, CourseNumber number)
            => new ($"Faculty {faculty} already course with number {number}");

        public static IsuException ExistingGroupException(GroupName name)
            => new ($"Group named '{name}' already exists");

        public static IsuException AlienGroupException(Group group)
            => new ($"Grouped '{group}' doesn't being tracked by the service");

        public static IsuException AlienStudentException(Student student)
            => new ($"Grouped named '{student}' doesn't being tracked by the service");

        public static IsuException MaximumStudentCountException(Group group, int count)
            => new ($"{group} group already has {count} students");

        public static IsuException InvalidGroupChangeException(Group group)
            => new ($"Student is already in {group}");
    }
}