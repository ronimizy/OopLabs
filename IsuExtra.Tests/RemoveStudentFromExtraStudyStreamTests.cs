using Isu.Entities;
using Isu.Models;
using IsuExtra.Entities;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class RemoveStudentFromExtraStudyStreamTests : IsuExtraTestsBase
    {
        [Test]
        public void RemovedSignedStudentTest()
        {
            const string facultyName = "IS";
            const char facultyLetter = 'M';

            const string foreignFacultyName = "CN";
            const char foreignFacultyLetter = 'W';

            const string groupName = "M3200";
            const string studentName = "Bill";

            const string subjectName = "Math";

            Faculty _ = IsuService.AddFaculty(facultyName, facultyLetter);
            Faculty foreignFaculty = IsuService.AddFaculty(foreignFacultyName, foreignFacultyLetter);
            Group group = IsuService.AddGroup(new GroupName(groupName));
            Student student = IsuService.AddStudent(group, studentName);

            var subject = new ExtraStudySubject(subjectName, foreignFaculty);
            var stream = new ExtraStudyStream(string.Empty, new Schedule(), 2);

            ScheduleService.RegisterExtraStudySubject(subject);
            ScheduleService.AddStreamToExtraStudySubject(subject.Id, stream);
            ScheduleService.AddStudentToExtraStudyStream(stream.Id, student.Id);

            Assert.DoesNotThrow(() => ScheduleService.RemoveStudentFromExtraStudyStream(stream.Id, student.Id));
        }

        [Test]
        public void RemoveNotSignedStudent_ScheduleServiceExceptionThrown()
        {
            const string facultyName = "IS";
            const char facultyLetter = 'M';

            const string foreignFacultyName = "CN";
            const char foreignFacultyLetter = 'W';

            const string groupName = "M3200";
            const string studentName = "Bill";

            const string subjectName = "Math";

            Faculty _ = IsuService.AddFaculty(facultyName, facultyLetter);
            Faculty foreignFaculty = IsuService.AddFaculty(foreignFacultyName, foreignFacultyLetter);
            Group group = IsuService.AddGroup(new GroupName(groupName));
            Student student = IsuService.AddStudent(group, studentName);

            var subject = new ExtraStudySubject(subjectName, foreignFaculty);
            var stream = new ExtraStudyStream(string.Empty, new Schedule(), 2);

            ScheduleService.RegisterExtraStudySubject(subject);
            ScheduleService.AddStreamToExtraStudySubject(subject.Id, stream);

            Assert.Throws<ScheduleServiceException>(() => ScheduleService.RemoveStudentFromExtraStudyStream(stream.Id, student.Id));
        }
    }
}