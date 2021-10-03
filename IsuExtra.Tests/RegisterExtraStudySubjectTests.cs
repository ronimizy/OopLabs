using System;
using Isu.Entities;
using IsuExtra.Entities;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class RegisterExtraStudySubjectTests : IsuExtraTestsBase
    {
        [Test]
        public void NullValueTest_ArgumentNullExceptionThrown()
            => Assert.Throws<ArgumentNullException>(() => ScheduleService.RegisterExtraStudySubject(null!));

        [Test]
        public void NotRegisteredFacultyTest_ForeignFacultyPassed_ScheduleExceptionThrown()
        {
            const string facultyName = "ИС";
            const char facultyLetter = 'M';

            const string subjectName = "Math";

            Faculty faculty = ForeignIsuService.AddFaculty(facultyName, facultyLetter);
            var subject = new ExtraStudySubject(subjectName, faculty);

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.RegisterExtraStudySubject(subject));
        }

        [Test]
        public void InvalidFacultyTest_ForeignFacultyWithSameLetterPassed_ScheduleExceptionThrown()
        {
            const string facultyName = "ИС";
            const char facultyLetter = 'M';

            const string subjectName = "Math";

            Faculty _ = IsuService.AddFaculty(facultyName, facultyLetter);
            Faculty foreignFaculty = ForeignIsuService.AddFaculty(facultyName, facultyLetter);

            var subject = new ExtraStudySubject(subjectName, foreignFaculty);

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.RegisterExtraStudySubject(subject));
        }

        [Test]
        public void ExistingSubjectTest_SubjectAddedTwice_ScheduleExceptionThrown()
        {
            const string facultyName = "ИС";
            const char facultyLetter = 'M';

            const string subjectName = "Math";
            
            Faculty faculty = IsuService.AddFaculty(facultyName, facultyLetter);
            var subject = new ExtraStudySubject(subjectName, faculty);

            ScheduleService.RegisterExtraStudySubject(subject);

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.RegisterExtraStudySubject(subject));
        }
    }
}