using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using Isu.Models;
using Isu.Services;
using IsuExtra.Entities;
using IsuExtra.Models;
using IsuExtra.Tools;
using Utility.Extensions;

namespace IsuExtra.Services.Implementations
{
    internal class ScheduleService : IScheduleService
    {
        private readonly ScheduleServiceConfiguration _configuration;
        private readonly IIsuService _isuService;

        private readonly List<StudySubject> _subjects = new();
        private readonly List<ExtraStudySubject> _extraStudySubjects = new();

        public ScheduleService(ScheduleServiceConfiguration configuration, IIsuService isuService)
        {
            _configuration = configuration;
            _isuService = isuService;
        }

        public void RegisterStudySubject(StudySubject subject)
        {
            subject.ThrowIfNull(nameof(subject));

            Faculty faculty = _isuService
                .FindFaculty(subject.Course.Faculty.Letter)
                .ThrowIfNull(ScheduleServiceExceptionFactory.NotRegisteredFaculty(subject.Course.Faculty));

            if (!faculty.Equals(subject.Course.Faculty))
                throw ScheduleServiceExceptionFactory.UnknownCourse(subject.Course);

            if (_subjects.Contains(subject))
                throw ScheduleServiceExceptionFactory.ExistingStudySubject(subject);

            _subjects.Add(subject);
        }

        public void AddGroupToStudySubject(Guid subjectId, GroupStudySchedule groupSchedule)
        {
            groupSchedule.ThrowIfNull(nameof(groupSchedule));

            StudySubject subject = _subjects
                .SingleOrDefault(s => s.Id.Equals(subjectId))
                .ThrowIfNull(ScheduleServiceExceptionFactory.NotRegisteredStudySubject(subjectId));

            GroupName groupName = groupSchedule.Group.Name;
            Course subjectCourse = subject.Course;

            if (subjectCourse.Faculty.Letter != groupName.FacultyLetter ||
                subjectCourse.Number != groupName.CourseNumber)
                throw ScheduleServiceExceptionFactory.ForeignGroup(subject, groupSchedule.Group);

            if (subject.GroupSchedules.Any(g => g.Group.Equals(groupSchedule.Group)))
                throw ScheduleServiceExceptionFactory.ExistingGroupStudySchedule(subject, groupSchedule.Group);

            Schedule schedule = GetGroupSchedule(groupSchedule.Group);

            if (groupSchedule.Schedule.IsIntersectsWith(schedule))
                throw ScheduleServiceExceptionFactory.ConflictingStudyGroupSchedule(groupSchedule);

            subject.AddGroupSchedule(groupSchedule);
        }

        public IReadOnlyCollection<StudySubject> GetAvailableStudySubjects(GroupName groupName)
        {
            Group group = _isuService.FindGroup(groupName)
                .ThrowIfNull(ScheduleServiceExceptionFactory.UnknownGroup(groupName));

            IEnumerable<StudySubject> subjects = _subjects
                .Where(s => s.Course.Faculty.Letter == groupName.FacultyLetter)
                .Where(s => s.Course.Number == groupName.CourseNumber)
                .Where(s => s.GroupSchedules.All(g => !g.Group.Equals(group)));

            return subjects.ToList();
        }

        public void RegisterExtraStudySubject(ExtraStudySubject subject)
        {
            subject.ThrowIfNull(nameof(subject));

            Faculty faculty = _isuService
                .FindFaculty(subject.Faculty.Letter)
                .ThrowIfNull(ScheduleServiceExceptionFactory.NotRegisteredFaculty(subject.Faculty));

            if (!faculty.Equals(subject.Faculty))
                throw ScheduleServiceExceptionFactory.InvalidFaculty(faculty, subject.Faculty);

            if (_extraStudySubjects.Contains(subject))
                throw ScheduleServiceExceptionFactory.ExistingExtraStudySubject(subject);

            _extraStudySubjects.Add(subject);
        }

        public void AddStreamToExtraStudySubject(Guid subjectId, ExtraStudyStream stream)
        {
            stream.ThrowIfNull(nameof(stream));

            ExtraStudySubject subject = _extraStudySubjects
                .SingleOrDefault(s => s.Id.Equals(subjectId))
                .ThrowIfNull(ScheduleServiceExceptionFactory.NotRegisteredExtraStudySubject(subjectId));

            if (subject.Streams.Contains(stream))
                throw ScheduleServiceExceptionFactory.ExistingExtraStudyStream(subject, stream);

            subject.AddStream(stream);
        }

        public void AddStudentToExtraStudyStream(Guid streamId, Guid studentId)
        {
            Student student = _isuService.GetStudent(studentId);
            (ExtraStudySubject subject, ExtraStudyStream stream) = GetStreamSubject(streamId);

            if (GetStudentExtraStudySubjectCount(student) == _configuration.MaximumExtraStudyCount)
                throw ScheduleServiceExceptionFactory.MaximumExtraStudySubjectAmount(student, _configuration.MaximumExtraStudyCount);

            if (subject.Streams.Any(s => s.Contains(student)))
                throw ScheduleServiceExceptionFactory.AlreadySignedStudent(subject, student);

            if (stream.Count == stream.Capacity)
                throw ScheduleServiceExceptionFactory.MaximumExtraStudyStreamStudentCount(stream);

            if (subject.Faculty.Letter == student.Group.Name.FacultyLetter)
                throw ScheduleServiceExceptionFactory.InvalidExtraStudySubjectForStudent(subject, student);

            Schedule studentSchedule = GetStudentSchedule(student);

            if (studentSchedule.IsIntersectsWith(stream.Schedule))
                throw ScheduleServiceExceptionFactory.ConflictingExtraStudyStreamSchedule(student, stream);

            stream.AddStudent(student);
        }

        public void RemoveStudentFromExtraStudyStream(Guid streamId, Guid studentId)
        {
            Student student = _isuService.GetStudent(studentId);
            (ExtraStudySubject _, ExtraStudyStream stream) = GetStreamSubject(streamId);

            if (!stream.RemoveStudent(student))
                throw ScheduleServiceExceptionFactory.NonExistingStudent(stream, student);
        }

        public IReadOnlyCollection<Student> GetNotSignedUpStudents(GroupName groupName)
        {
            Group group = _isuService
                .FindGroup(groupName)
                .ThrowIfNull(ScheduleServiceExceptionFactory.UnknownGroup(groupName));

            IEnumerable<Student> students = group.Students
                .Where(student => _extraStudySubjects.Count(subject => subject.HasStudent(student)) < _configuration.MaximumExtraStudyCount);

            return students.ToList();
        }

        public IReadOnlyCollection<ExtraStudySubjectDto> GetAvailableExtraStudySubjects(Guid studentId)
        {
            Student student = _isuService.GetStudent(studentId);
            Schedule studentSchedule = GetStudentSchedule(student);

            IEnumerable<ExtraStudySubjectDto> extraSubjects = _extraStudySubjects
                .Select(subject => subject.ToDto(studentSchedule))
                .Where(subject => subject.AvailableStreams.Count > 0);

            return extraSubjects.ToList();
        }

        private Schedule GetGroupSchedule(Group group)
        {
            group.ThrowIfNull(nameof(group));

            IEnumerable<Schedule> schedules = _subjects
                .SelectMany(s => s.GroupSchedules)
                .Where(s => s.Group.Id.Equals(group.Id))
                .Select(s => s.Schedule);

            return new Schedule(schedules);
        }

        private Schedule GetStudentSchedule(Student student)
        {
            Schedule groupSchedule = GetGroupSchedule(student.Group);

            IEnumerable<Schedule> extraStudySubjectSchedules = _extraStudySubjects
                .SelectMany(s => s.Streams)
                .Where(s => s.Contains(student))
                .Select(s => s.Schedule);

            return new Schedule(extraStudySubjectSchedules.Append(groupSchedule));
        }

        private (ExtraStudySubject Subject, ExtraStudyStream Stream) GetStreamSubject(Guid streamId)
        {
            (ExtraStudySubject, ExtraStudyStream) subjectStream = _extraStudySubjects
                .SelectMany(s => s.Streams, (subj, str) => (subj, str))
                .SingleOrDefault(p => p.str.Id.Equals(streamId));

            if (subjectStream == default)
                throw ScheduleServiceExceptionFactory.NotRegisteredExtraStudyStream(streamId);

            return subjectStream;
        }

        private int GetStudentExtraStudySubjectCount(Student student)
            => _extraStudySubjects
                .SelectMany(s => s.Streams)
                .Count(s => s.Contains(student));
    }
}