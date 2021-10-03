using System;
using Isu.Entities;
using IsuExtra.Entities;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class AddStreamToStudySubjectTests : IsuExtraTestsBase
    {
        [Test]
        public void NullValueTest_ArgumentNullExceptionThrown()
            => Assert.Throws<ArgumentNullException>(() => ScheduleService.AddStreamToExtraStudySubject(Guid.Empty, null!));

        [Test]
        public void InvalidSubjectIdTest_InvalidIdPassed_ScheduleServiceExceptionThrown()
            => Assert.Throws<ScheduleServiceException>(() => ScheduleService
                                                           .AddStreamToExtraStudySubject(
                                                               Guid.NewGuid(),
                                                               new ExtraStudyStream("КИБ1", new Schedule(), 1)));

        [Test]
        public void ExistingStreamTest_ExtraStudyStreamAddedTwice_ScheduleExceptionThrown()
        {
            const string facultyName = "IS";
            const char facultyLetter = 'M';

            const string subjectName = "Math";
            
            Faculty faculty = IsuService.AddFaculty(facultyName, facultyLetter);
            var subject = new ExtraStudySubject(subjectName, faculty);
            
            ScheduleService.RegisterExtraStudySubject(subject);

            var stream = new ExtraStudyStream(string.Empty, new Schedule(), 10);
            
            ScheduleService.AddStreamToExtraStudySubject(subject.Id, stream);

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.AddStreamToExtraStudySubject(subject.Id, stream));
        }
    }
}