using System;
using System.Collections.Generic;
using Isu.Entities;
using Isu.Models;
using Isu.Services;
using IsuExtra.Entities;
using IsuExtra.Models;
using IsuExtra.Services.Implementations;

namespace IsuExtra.Services
{
    public interface IScheduleService
    {
        void RegisterStudySubject(StudySubject subject);
        void AddGroupToStudySubject(Guid subjectId, GroupStudySchedule groupSchedule);
        IReadOnlyCollection<StudySubject> GetAvailableStudySubjects(GroupName groupName);

        void RegisterExtraStudySubject(ExtraStudySubject subject);
        void AddStreamToExtraStudySubject(Guid subjectId, ExtraStudyStream stream);
        void AddStudentToExtraStudyStream(Guid streamId, Guid studentId);
        void RemoveStudentFromExtraStudyStream(Guid streamId, Guid studentId);
        IReadOnlyCollection<Student> GetNotSignedUpStudents(GroupName groupName);
        IReadOnlyCollection<ExtraStudySubjectDto> GetAvailableExtraStudySubjects(Guid studentId);

        static IScheduleService Create(ScheduleServiceConfiguration configuration, IIsuService isuService)
            => new ScheduleService(configuration, isuService);
    }
}