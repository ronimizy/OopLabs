using System;
using Isu.Entities;
using Isu.Models;
using IsuExtra.Entities;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class RegisterStudySubjectTests : IsuExtraTestsBase
    {
        [Test]
        public void NullValueTest_ArgumentNullExceptionThrown()
            => Assert.Throws<ArgumentNullException>(() => ScheduleService.RegisterStudySubject(null!));
        
        [Test]
        public void UnknownFacultyTest_NotRegisteredFacultyPassed_ScheduleServiceExceptionThrown()
        {
            const string facultyName = "ИС";
            const char facultyLetter = 'M';
            
            const string groupName = "M3200";
            const string subjectName = "Math";
            
            Faculty _ = ForeignIsuService.AddFaculty(facultyName, facultyLetter);
            Group group = ForeignIsuService.AddGroup(new GroupName(groupName));
            var subject = new StudySubject(subjectName, group.Course);
            
            Assert.Throws<ScheduleServiceException>(() => ScheduleService.RegisterStudySubject(subject));
        }

        [Test]
        public void ExistingSubjectTest_SubjectRegisteredTwice_ScheduleServiceExceptionThrown()
        {
            const string facultyName = "ИС";
            const char facultyLetter = 'M';
            
            const string groupName = "M3200";
            const string subjectName = "Math";
            
            Faculty _ = IsuService.AddFaculty(facultyName, facultyLetter);
            Group group = IsuService.AddGroup(new GroupName(groupName));
            var subject = new StudySubject(subjectName, group.Course);

            ScheduleService.RegisterStudySubject(subject);
            
            Assert.Throws<ScheduleServiceException>(() => ScheduleService.RegisterStudySubject(subject));
        }
    }
}