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
    public class AddGroupToStudySubjectTests : IsuExtraTestsBase
    {
        private StudySubject _subject = null!;
        private Group _group = null!;
        
        [SetUp]
        public void Setup()
        {
            Faculty _ = IsuService.AddFaculty("ИС", 'M');
            _group = IsuService.AddGroup(new GroupName("M3200"));
            _subject = new StudySubject("Math", _group.Course);

            ScheduleService.RegisterStudySubject(_subject);
        }

        [Test]
        public void NullValueTest_ArgumentNullExceptionThrown()
            => Assert.Throws<ArgumentNullException>(() => ScheduleService.AddGroupToStudySubject(Guid.Empty, null!));

        [Test]
        public void InvalidFacultyIdTest_EmptyGuidPassed_ScheduleServiceExceptionThrown()
        {
            var schedule = new GroupStudySchedule(_group, new Schedule());
            Assert.Throws<ScheduleServiceException>(() => ScheduleService.AddGroupToStudySubject(Guid.Empty, schedule));
        }

        [Test]
        public void ForeignFacultyTest_GroupFromForeignFacultyPassed_ScheduleServiceExceptionThrown()
        {
            const string facultyName = "CN";
            const char facultyLetter = 'W';

            const string groupName = "W3200";
            
            Faculty _ = IsuService.AddFaculty(facultyName, facultyLetter);
            Group group = IsuService.AddGroup(new GroupName(groupName));
            var schedule = new GroupStudySchedule(group, new Schedule());

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.AddGroupToStudySubject(_subject.Id, schedule));
        }

        [Test]
        public void RegisteredGroupTest_GroupAddedTwice_ScheduleServiceExceptionThrown()
        {
            var schedule = new GroupStudySchedule(_group, new Schedule());
            ScheduleService.AddGroupToStudySubject(_subject.Id, schedule);

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.AddGroupToStudySubject(_subject.Id, schedule));
        }

        [Test]
        public void ConflictingScheduleTest_GroupWithConflictingSchedulePassed_ScheduleServiceExceptionThrown()
        {
            const string subjectName = "Physics";
            const string mentorName = "Zin4ik";
            const string roomName = "228";
            
            var lesson = new Lesson(LessonFrequency.Persistent, TimeSpan.FromHours(11), TimeSpan.FromHours(12),
                                    new Mentor(mentorName), roomName);
            var subject = new StudySubject(subjectName, _group.Course);
            var schedule = new GroupStudySchedule(_group, new Schedule(lesson));
            
            ScheduleService.RegisterStudySubject(subject);
            ScheduleService.AddGroupToStudySubject(subject.Id, schedule);
            
            Assert.Throws<ScheduleServiceException>(() => ScheduleService.AddGroupToStudySubject(_subject.Id, schedule));
        }
    }
}