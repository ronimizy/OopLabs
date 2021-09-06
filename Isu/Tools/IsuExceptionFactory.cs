using Isu.Entities;
using Isu.Models;

namespace Isu.Tools
{
    public static class IsuExceptionFactory
    {
        public static IsuException InvalidCourseNumberException(int value, string details = "")
            => new ($"{value} - is an invalid value for course number. {details}");

        public static IsuException InvalidGroupNameException(string name, string details = "")
            => new ($"'{name}' - is invalid value for group name. {details}");

        public static IsuException ExistingGroupException(GroupName name)
            => new ($"Group named '{name.Name}' already exists");

        public static IsuException AlienGroupException(Group group)
            => new ($"Grouped named '{group.Name}' doesn't being tracked by the service");

        public static IsuException AlienStudentException(Student student)
            => new ($"Grouped named '{student.Name}' doesn't being tracked by the service");

        public static IsuException MaximumStudentCount(Group group, int count)
            => new ($"{group} group already has {count} students");
    }
}