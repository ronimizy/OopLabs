using System.Linq;
using Isu.Entities;
using Isu.Tools;
using Isu.Models;
using Isu.Services;
using NUnit.Framework;

namespace Isu.Tests
{
    [TestFixture]
    public class Tests
    {
        private IIsuService _service;
        private IsuServiceConfiguration _configuration;

        [OneTimeSetUp]
        public void Setup()
        {
            _configuration = new IsuServiceConfiguration
            {
                MaxStudentCount = 20
            };
            _service = IIsuService.Create(_configuration);
            _service.AddFaculty("ИС", 'M');
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group group = _service.AddGroup(new GroupName("M3200"));
            Student student = _service.AddStudent(group, "Student A");

            Assert.AreEqual(student.Group, group);
            Assert.IsTrue(group.Students.Contains(student));
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Group group = _service.AddGroup(new GroupName("M3202"));

            for (int i = 0; i < _configuration.MaxStudentCount; i++)
                _service.AddStudent(group, $"Student D{i}");

            Assert.Throws<IsuException>(() => _service.AddStudent(group, $"Student D{_configuration.MaxStudentCount}"));
        }

        [TestCase("M32001")]
        [TestCase("33200")]
        [TestCase("Z320a")]
        [TestCase("M320a")]
        public void CreateGroupWithInvalidName_ThrowException(string name)
            => Assert.Throws<IsuException>(() => _service.AddGroup(new GroupName(name)));

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Group from = _service.AddGroup(new GroupName("M3204"));
            Group to = _service.AddGroup(new GroupName("M3205"));

            Student student = _service.AddStudent(from, "Student C");

            _service.ChangeStudentGroup(student, to);

            Assert.AreEqual(student.Group, to);
        }
    }
}