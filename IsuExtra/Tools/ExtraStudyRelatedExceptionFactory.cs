using System;
using Isu.Entities;
using Isu.Models;
using IsuExtra.Entities;

namespace IsuExtra.Tools
{
    internal static class ExtraStudyRelatedExceptionFactory
    {
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