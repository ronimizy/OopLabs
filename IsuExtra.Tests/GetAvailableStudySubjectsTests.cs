using Isu.Entities;
using Isu.Models;
using IsuExtra.Entities;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class GetAvailableStudySubjectsTests : IsuExtraTestsBase
    {
        [Test]
        public void UnknownGroupTest_GroupNamePassed_ScheduleServiceExceptionThrown()
            => Assert.Throws<ScheduleServiceException>(() => ScheduleService.GetAvailableStudySubjects(new GroupName("W3200")));

        [Test]
        public void ValidValueTest_GroupNamePassed_StudySubjectDtoCollectionReturned()
        {
            const string facultyName = "ИС";
            const char facultyLetter = 'M';

            const string groupName = "M3200";
            const string otherGroupName = "M3201";
            const string subjectName = "Math";
            
            Faculty _ = IsuService.AddFaculty(facultyName, facultyLetter);
            Group group = IsuService.AddGroup(new GroupName(groupName));
            var subject = new StudySubject(subjectName, group.Course);

            Group otherGroup = IsuService.AddGroup(new GroupName(otherGroupName));
            var schedule = new GroupStudySchedule(otherGroup, new Schedule());

            ScheduleService.RegisterStudySubject(subject);
            ScheduleService.AddGroupToStudySubject(subject.Id, schedule);

            CollectionAssert.AreEqual(new[] { subject }, ScheduleService.GetAvailableStudySubjects(group.Name));
        }
    }
}