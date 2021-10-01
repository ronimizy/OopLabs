using System;
using Isu.Entities;
using Isu.Models;
using IsuExtra.Entities;
using IsuExtra.Models;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class AddStudentToExtraStudyStreamTests : IsuExtraTestsBase
    {
        private Faculty _faculty = null!;
        private Faculty _foreignFaculty = null!;
        private Group _group = null!;
        private Student _student = null!;

        [SetUp]
        public void Setup()
        {
            const string facultyName = "ИС";
            const char facultyLetter = 'M';

            const string foreignFacultyName = "CN";
            const char foreignFacultyLetter = 'W';

            const string groupName = "M3200";
            const string studentName = "Bill";

            _faculty = IsuService.AddFaculty(facultyName, facultyLetter);
            _foreignFaculty = IsuService.AddFaculty(foreignFacultyName, foreignFacultyLetter);
            _group = IsuService.AddGroup(new GroupName(groupName));
            _student = IsuService.AddStudent(_group, studentName);
        }

        [Test]
        public void InvalidStreamTest_InvalidStreamIdPassed_ScheduleServiceExceptionThrown()
        {
            const string subjectName = "Math";

            var subject = new ExtraStudySubject(subjectName, _foreignFaculty);

            ScheduleService.RegisterExtraStudySubject(subject);

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.AddStudentToExtraStudyStream(Guid.Empty, _student.Id));
        }

        [Test]
        public void MaximumExtraStudyCountTest_StudentSignedForThreeStudies_ScheduleServiceExceptionThrown()
        {
            const string subjectOneName = "Math";
            const string subjectTwoName = "Physics";
            const string subjectThreeName = "History";

            var subjectOne = new ExtraStudySubject(subjectOneName, _foreignFaculty);
            var subjectTwo = new ExtraStudySubject(subjectTwoName, _foreignFaculty);
            var subjectThree = new ExtraStudySubject(subjectThreeName, _foreignFaculty);

            var streamOne = new ExtraStudyStream(string.Empty, new Schedule(), 10);
            var streamTwo = new ExtraStudyStream(string.Empty, new Schedule(), 10);
            var streamThree = new ExtraStudyStream(string.Empty, new Schedule(), 10);

            ScheduleService.RegisterExtraStudySubject(subjectOne);
            ScheduleService.RegisterExtraStudySubject(subjectTwo);
            ScheduleService.RegisterExtraStudySubject(subjectThree);

            ScheduleService.AddStreamToExtraStudySubject(subjectOne.Id, streamOne);
            ScheduleService.AddStreamToExtraStudySubject(subjectTwo.Id, streamTwo);
            ScheduleService.AddStreamToExtraStudySubject(subjectThree.Id, streamThree);

            ScheduleService.AddStudentToExtraStudyStream(streamOne.Id, _student.Id);
            ScheduleService.AddStudentToExtraStudyStream(streamTwo.Id, _student.Id);

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.AddStudentToExtraStudyStream(streamTwo.Id, _student.Id));
        }

        [Test]
        public void RepetitiveSigningTest_StudentAddedToExtraStudyStreamTwice_ScheduleServiceExceptionThrown()
        {
            const string subjectName = "Math";

            var subject = new ExtraStudySubject(subjectName, _foreignFaculty);
            var stream = new ExtraStudyStream(string.Empty, new Schedule(), 20);

            ScheduleService.RegisterExtraStudySubject(subject);
            ScheduleService.AddStreamToExtraStudySubject(subject.Id, stream);
            ScheduleService.AddStudentToExtraStudyStream(stream.Id, _student.Id);

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.AddStudentToExtraStudyStream(stream.Id, _student.Id));
        }

        [Test]
        public void MaxCapacityTest_StreamReachedMaxCapacity_StudentAdded_ScheduleServiceExceptionThrown()
        {
            const string subjectName = "Math";
            
            var subject = new ExtraStudySubject(subjectName, _foreignFaculty);
            var stream = new ExtraStudyStream(string.Empty, new Schedule(), 0);

            ScheduleService.RegisterExtraStudySubject(subject);
            ScheduleService.AddStreamToExtraStudySubject(subject.Id, stream);

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.AddStudentToExtraStudyStream(stream.Id, _student.Id));
        }

        [Test]
        public void ForeignFacultyTest_StudentAddedToForeignFacultySubject_ScheduleServiceExceptionThrown()
        {
            const string subjectName = "Math";
            
            var subject = new ExtraStudySubject(subjectName,  _faculty);
            var stream = new ExtraStudyStream(string.Empty, new Schedule(), 10);

            ScheduleService.RegisterExtraStudySubject(subject);
            ScheduleService.AddStreamToExtraStudySubject(subject.Id, stream);

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.AddStudentToExtraStudyStream(stream.Id, _student.Id));
        }

        [Test]
        public void ConflictingScheduleTest_ScheduleServiceExceptionThrown()
        {
            const string mentorName = "VAnna";
            const string roomName = "1337";

            const string subjectName = "Base Math";
            const string extraSubjectName = "Math";
            
            var lesson = new Lesson(LessonFrequency.Persistent,
                                    TimeSpan.FromHours(12),
                                    TimeSpan.FromHours(13),
                                    new Mentor(mentorName),
                                    roomName);
            var subject = new StudySubject(subjectName, _group.Course);
            var extraSubject = new ExtraStudySubject(extraSubjectName, _foreignFaculty);
            var stream = new ExtraStudyStream(string.Empty, new Schedule(lesson), 10);

            ScheduleService.RegisterStudySubject(subject);
            ScheduleService.AddGroupToStudySubject(subject.Id, new GroupStudySchedule(_group, new Schedule(lesson)));
            ScheduleService.RegisterExtraStudySubject(extraSubject);
            ScheduleService.AddStreamToExtraStudySubject(extraSubject.Id, stream);

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.AddStudentToExtraStudyStream(stream.Id, _student.Id));
        }
    }
}