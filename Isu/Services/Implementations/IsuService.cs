using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Isu.Entities;
using Isu.Models;
using Isu.Tools;
using Utility.Extensions;

namespace Isu.Services.Implementations
{
    internal sealed class IsuService : IIsuService
    {
        private readonly IsuApplicationConfiguration _configuration;
        private readonly List<Group> _groups = new ();
        private readonly List<Student> _students = new ();
        private int _studentIdCounter;

        public IsuService(IsuApplicationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Group AddGroup(GroupName name)
        {
            name.ThrowIfNull(nameof(name));

            if (_groups.Any(g => g.Name.Equals(name)))
                throw IsuExceptionFactory.ExistingGroupException(name);

            var group = new Group(name);
            _groups.Add(group);

            return group;
        }

        public Student AddStudent(Group group, string name)
        {
            group.ThrowIfNull(nameof(group));
            name.ThrowIfNull(nameof(name));

            if (!_groups.Contains(group))
                throw IsuExceptionFactory.AlienGroupException(group);

            if (group.Students.Count == _configuration.MaxStudentCount)
                throw IsuExceptionFactory.MaximumStudentCount(group, _configuration.MaxStudentCount);

            var student = new Student(GetNewStudentId(), name, group);
            group.AddStudent(student);
            _students.Add(student);

            return student;
        }

        public Student? GetStudent(int id)
            => _students.SingleOrDefault(s => s.Id == id);

        public Student? FindStudent(string name)
        {
            name.ThrowIfNull(nameof(name));
            return _students.SingleOrDefault(s => s.Name == name);
        }

        public IReadOnlyList<Student> FindStudents(GroupName groupName)
        {
            groupName.ThrowIfNull(nameof(groupName));
            return _students
                .Where(s => s.Group.Name.Equals(groupName))
                .ToList();
        }

        public IReadOnlyList<Student> FindStudents(CourseNumber courseNumber)
        {
            courseNumber.ThrowIfNull(nameof(courseNumber));
            return _students
                .Where(s => s.Group.CourseNumber.Equals(courseNumber))
                .ToList();
        }

        public Group? FindGroup(GroupName groupName)
        {
            groupName.ThrowIfNull(nameof(groupName));
            return _groups.SingleOrDefault(g => g.Name.Equals(groupName));
        }

        public IReadOnlyList<Group> FindGroups(CourseNumber courseNumber)
        {
            courseNumber.ThrowIfNull(nameof(courseNumber));
            return _groups
                .Where(g => g.CourseNumber.Equals(courseNumber))
                .ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            student.ThrowIfNull(nameof(student));
            student.Group.ThrowIfNull(nameof(student.Group));
            newGroup.ThrowIfNull(nameof(newGroup));

            if (!_students.Contains(student))
                throw IsuExceptionFactory.AlienStudentException(student);

            if (!_groups.Contains(student.Group))
                throw IsuExceptionFactory.AlienGroupException(student.Group);

            if (!_groups.Contains(newGroup))
                throw IsuExceptionFactory.AlienGroupException(newGroup);

            if (newGroup.Students.Count == _configuration.MaxStudentCount)
                throw IsuExceptionFactory.MaximumStudentCount(newGroup, _configuration.MaxStudentCount);

            student.Group.RemoveStudent(student);
            student.Group = newGroup;
            newGroup.AddStudent(student);
        }

        private int GetNewStudentId()
            => Interlocked.Increment(ref _studentIdCounter);
    }
}