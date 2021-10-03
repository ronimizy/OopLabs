using System;
using System.Linq;
using Isu.Entities;
using Isu.Models;
using IsuExtra.Entities;
using IsuExtra.Models;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class GetAvailableExtraStudySubjectsTests : IsuExtraTestsBase
    {
        [Test]
        public void ConflictingScheduleTest_AddConflictingScheduleToStudentAndExtraStudyStream_FetchAvailableSubject_ReceiveEmptyCollection()
        {
            const string facultyName = "ИС";
            const char facultyLetter = 'M';

            const string foreignFacultyName = "CN";
            const char foreignFacultyLetter = 'W';

            const string groupName = "M3200";
            const string studentName = "Bill";

            const string mentorName = "Fredi Kats";
            const string roomName = "229";

            const string extraSubjectName = "Extra OOP";
            const string subjectName = "OOP";

            _ = IsuService.AddFaculty(facultyName, facultyLetter);
            Faculty foreignFaculty = IsuService.AddFaculty(foreignFacultyName, foreignFacultyLetter);
            Group group = IsuService.AddGroup(new GroupName(groupName));
            Student student = IsuService.AddStudent(group, studentName);

            var lesson = new Lesson(DayOfWeek.Monday,
                                    LessonFrequency.Even,
                                    TimeSpan.FromHours(13) + TimeSpan.FromMinutes(30),
                                    TimeSpan.FromHours(16) + TimeSpan.FromMinutes(50),
                                    new Mentor(mentorName),
                                    roomName);
            var extraSubject = new ExtraStudySubject(extraSubjectName, foreignFaculty);
            var stream = new ExtraStudyStream("2", new Schedule(lesson), 10);

            var subject = new StudySubject(subjectName, group.Course);
            var groupSchedule = new GroupStudySchedule(group, new Schedule(lesson));

            ScheduleService.RegisterStudySubject(subject);
            ScheduleService.AddGroupToStudySubject(subject.Id, groupSchedule);

            ScheduleService.RegisterExtraStudySubject(extraSubject);
            ScheduleService.AddStreamToExtraStudySubject(extraSubject.Id, stream);

            CollectionAssert.IsEmpty(ScheduleService.GetAvailableExtraStudySubjects(student.Id));
        }

        [Test]
        public void ValidValuesTest_StudentIdPassed_ExtraStudyWithSingleStreamReturned()
        {
            const string facultyName = "ИС";
            const char facultyLetter = 'M';

            const string foreignFacultyName = "CN";
            const char foreignFacultyLetter = 'W';

            const string groupName = "M3200";
            const string studentName = "Bill";

            const string mentorName = "Fredi Kats";
            const string roomName = "229";

            const string extraSubjectName = "Extra OOP";

            _ = IsuService.AddFaculty(facultyName, facultyLetter);
            Faculty foreignFaculty = IsuService.AddFaculty(foreignFacultyName, foreignFacultyLetter);
            Group group = IsuService.AddGroup(new GroupName(groupName));
            Student student = IsuService.AddStudent(group, studentName);

            var lesson = new Lesson(DayOfWeek.Monday,
                                    LessonFrequency.Even,
                                    TimeSpan.FromHours(13) + TimeSpan.FromMinutes(30),
                                    TimeSpan.FromHours(16) + TimeSpan.FromMinutes(50),
                                    new Mentor(mentorName),
                                    roomName);
            var extraSubject = new ExtraStudySubject(extraSubjectName, foreignFaculty);
            var stream = new ExtraStudyStream("2", new Schedule(lesson), 10);

            ScheduleService.RegisterExtraStudySubject(extraSubject);
            ScheduleService.AddStreamToExtraStudySubject(extraSubject.Id, stream);

            CollectionAssert.AreEqual(new[] { stream },
                                      ScheduleService
                                          .GetAvailableExtraStudySubjects(student.Id)
                                          .Single().AvailableStreams);
        }
    }
}