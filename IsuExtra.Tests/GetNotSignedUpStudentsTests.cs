using Isu.Entities;
using Isu.Models;
using IsuExtra.Entities;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class GetNotSignedUpStudentsTests : IsuExtraTestsBase
    {
        [Test]
        public void UnknownGroupTest_UnknownGroupNamePassed_ScheduleServiceExceptionThrown()
            => Assert.Throws<ScheduleServiceException>(() => ScheduleService.GetNotSignedUpStudents(new GroupName("M3200")));

        [Test]
        public void ValidValueTest_StudentGroupNamePassed_ListWithStudentReturned()
        {
            const string facultyName = "ИС";
            const char facultyLetter = 'M';

            const string foreignFacultyName = "CN";
            const char foreignFacultyLetter = 'W';

            const string groupName = "M3200";

            const string studentName = "Bill";
            const string signedStudentName = "Bibletoon";
            
            const string subjectOneName = "Math";
            const string subjectTwoName = "Physics";
            
            _ = IsuService.AddFaculty(facultyName, facultyLetter);
            Faculty foreignFaculty = IsuService.AddFaculty(foreignFacultyName, foreignFacultyLetter);
            Group group = IsuService.AddGroup(new GroupName(groupName));
            Student student = IsuService.AddStudent(group, studentName);
            Student signedStudent = IsuService.AddStudent(group, signedStudentName);

            var subjectOne = new ExtraStudySubject(subjectOneName, foreignFaculty);
            var subjectTwo = new ExtraStudySubject(subjectTwoName, foreignFaculty);

            var streamOne = new ExtraStudyStream(string.Empty, new Schedule(), 10);
            var streamTwo = new ExtraStudyStream(string.Empty, new Schedule(), 10);

            ScheduleService.RegisterExtraStudySubject(subjectOne);
            ScheduleService.RegisterExtraStudySubject(subjectTwo);

            ScheduleService.AddStreamToExtraStudySubject(subjectOne.Id, streamOne);
            ScheduleService.AddStreamToExtraStudySubject(subjectTwo.Id, streamTwo);

            ScheduleService.AddStudentToExtraStudyStream(streamOne.Id, signedStudent.Id);
            ScheduleService.AddStudentToExtraStudyStream(streamTwo.Id, signedStudent.Id);

            CollectionAssert.AreEqual(new[] { student }, ScheduleService.GetNotSignedUpStudents(group.Name));
        }
    }
}