using System;
using Isu.Entities;
using Isu.Models;
using IsuExtra.Entities;

namespace IsuExtra.Tools
{
    internal static class ScheduleServiceExceptionFactory
    {
        public static ScheduleServiceException OverlappingSchedule()
            => new ScheduleServiceException("Provided lessons are overlapping.");

        public static ScheduleServiceException InvalidLessonTime(TimeSpan begin, TimeSpan end, string description)
            => new ScheduleServiceException($"Provided time for lesson is invalid. Begin: {begin}, end: {end}. {description}.");

        // IIsuService related
        public static ScheduleServiceException NotRegisteredFaculty(Faculty faculty)
            => new ScheduleServiceException($"Faculty: {faculty} is not registered.");

        public static ScheduleServiceException UnknownCourse(Course course)
            => new ScheduleServiceException($"Course: {course} is unknown.");

        public static ScheduleServiceException UnknownGroup(GroupName name)
            => new ScheduleServiceException($"Group named '{name}' is unknown.");

        // Study
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

        // ExtraStudy
        // RegisterExtraStudySubject
        public static ScheduleServiceException InvalidFaculty(Faculty expected, Faculty received)
            => new ScheduleServiceException($"Invalid Faculty. Expected: {expected}, received: {received}");

        public static ScheduleServiceException ExistingExtraStudySubject(ExtraStudySubject subject)
            => new ScheduleServiceException($"ExtraStudySubject: \"{subject}\" is already been registered.");

        // AddStreamToExtraStudySubject
        public static ScheduleServiceException NotRegisteredExtraStudySubject(Guid id)
            => new ScheduleServiceException($"ExtraStudySubject with id: [{id}], is not registered.");

        public static ScheduleServiceException ExistingExtraStudyStream(ExtraStudySubject subject, ExtraStudyStream stream)
            => new ScheduleServiceException($"ExtraStudySubject: \"{subject}\" already contains ExtraStudyStream: \"{stream}\".");

        // AddStudentToExtraStudyStream
        public static ScheduleServiceException MaximumExtraStudySubjectAmount(Student student, int amount)
            => new ScheduleServiceException($"{nameof(Student)}: {student} already signed for maximum amount of {nameof(ExtraStudySubject)} ({amount}).");

        public static ScheduleServiceException AlreadySignedStudent(ExtraStudySubject subject, Student student)
            => new ScheduleServiceException($"Student: {student} is already signed for ExtraStudySubject: {subject}");

        public static ScheduleServiceException MaximumExtraStudyStreamStudentCount(ExtraStudyStream stream)
            => new ScheduleServiceException($"ExtraStudyStream: {stream}, already has a maximum student amount of {stream.Capacity}");

        public static ScheduleServiceException InvalidExtraStudySubjectForStudent(ExtraStudySubject subject, Student student)
            => new ScheduleServiceException($"{nameof(Student)}: \"{student}\" cannot be signed for {nameof(ExtraStudySubject)}: \"{subject}\".");

        public static ScheduleServiceException ConflictingExtraStudyStreamSchedule(Student student, ExtraStudyStream stream)
            => new ScheduleServiceException($"{nameof(Student)}: \"{student}\" cannot be added to {nameof(ExtraStudyStream)}: \"{stream}\", " +
                                            "because their schedules are conflicting.");

        // RemoveStudentFromExtraStudyStream
        public static ScheduleServiceException NonExistingStudent(ExtraStudyStream stream, Student student)
            => new ScheduleServiceException($"{nameof(Student)}: \"{student}\" is not contained in {nameof(ExtraStudyStream)}: \"{stream}\".");

        public static ScheduleServiceException NotRegisteredExtraStudyStream(Guid id)
            => new ScheduleServiceException($"ExtraStudyStream with id: [{id}], is not registered.");
    }
}