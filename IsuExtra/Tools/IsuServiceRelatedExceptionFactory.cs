using Isu.Entities;
using Isu.Models;

namespace IsuExtra.Tools
{
    public static class IsuServiceRelatedExceptionFactory
    {
        public static ScheduleServiceException NotRegisteredFaculty(Faculty faculty)
            => new ScheduleServiceException($"Faculty: {faculty} is not registered.");

        public static ScheduleServiceException UnknownCourse(Course course)
            => new ScheduleServiceException($"Course: {course} is unknown.");

        public static ScheduleServiceException UnknownGroup(GroupName name)
            => new ScheduleServiceException($"Group named '{name}' is unknown.");
    }
}