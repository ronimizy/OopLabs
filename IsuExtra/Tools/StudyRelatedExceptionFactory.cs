using System;
using Isu.Entities;
using IsuExtra.Entities;

namespace IsuExtra.Tools
{
    public static class StudyRelatedExceptionFactory
    {
        // RegisterStudySubject
        public static ScheduleServiceException ExistingStudySubject(StudySubject subject)
            => new ScheduleServiceException($"StudySubject: {subject} is already registered.");

        // AddGroupToStudySubject
        public static ScheduleServiceException NotRegisteredStudySubject(Guid subjectId)
            => new ScheduleServiceException($"StudySubject with id: {subjectId} is not registered.");

        public static ScheduleServiceException ForeignGroup(StudySubject subject, Group group)
            => new ScheduleServiceException($"Group: {group}, is not assignable to {subject}.");

        public static ScheduleServiceException ExistingGroupStudySchedule(StudySubject subject, Group group)
            => new ScheduleServiceException($"StudySubject: {subject} already has a schedule for {group}");

        public static ScheduleServiceException ConflictingStudyGroupSchedule(GroupStudySchedule schedule)
            => new ScheduleServiceException($"StudyGroupSchedule: {schedule} is conflicting with {schedule.Group} schedule.");
    }
}