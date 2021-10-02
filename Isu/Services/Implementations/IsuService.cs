using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using Isu.Models;
using Isu.Tools;
using Utility.Extensions;

namespace Isu.Services.Implementations
{
    internal sealed class IsuService : IIsuService
    {
        private readonly IsuServiceConfiguration _configuration;

        private readonly List<Faculty> _faculties = new ();
        private readonly List<Group> _groups = new ();
        private readonly List<Student> _students = new ();

        public IsuService(IsuServiceConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Faculty AddFaculty(string name, char letter)
        {
            name.ThrowIfNull(nameof(name));

            if (_faculties.Any(f => f.Name == name || f.Letter == letter))
                throw IsuExceptionFactory.ExistingFacultyException(name, letter);

            var faculty = new Faculty(name, letter);
            _faculties.Add(faculty);

            return faculty;
        }

        public Faculty? FindFaculty(char letter)
            => _faculties.SingleOrDefault(f => f.Letter.Equals(letter));

        public Group AddGroup(GroupName name)
        {
            name.ThrowIfNull(nameof(name));

            if (_groups.Any(g => g.Name.Equals(name)))
                throw IsuExceptionFactory.ExistingGroupException(name);

            Faculty? faculty = _faculties.SingleOrDefault(f => f.Letter.Equals(name.FacultyLetter));

            if (faculty is null)
                throw IsuExceptionFactory.NonExistingFacultyException(name.FacultyLetter);

            Group group = faculty.AddGroup(name);
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
                throw IsuExceptionFactory.MaximumStudentCountException(group, _configuration.MaxStudentCount);

            var student = new Student(name, group);
            group.AddStudent(student);
            _students.Add(student);

            return student;
        }

        public Student GetStudent(Guid id)
            => _students.Single(s => s.Id == id);

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
            return _faculties
                .Where(f => f.Courses.Any(c => c.Number.Equals(courseNumber)))
                .Select(f => f.Courses.Single(c => c.Number.Equals(courseNumber)))
                .SelectMany(c => c.Groups)
                .SelectMany(g => g.Students)
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
                .Where(g => g.Name.CourseNumber.Equals(courseNumber))
                .ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            student.ThrowIfNull(nameof(student));
            student.Group.ThrowIfNull(nameof(student.Group));
            newGroup.ThrowIfNull(nameof(newGroup));

            if (student.Group.Equals(newGroup))
                throw IsuExceptionFactory.InvalidGroupChangeException(newGroup);

            if (!_students.Contains(student))
                throw IsuExceptionFactory.AlienStudentException(student);

            if (!_groups.Contains(student.Group))
                throw IsuExceptionFactory.AlienGroupException(student.Group);

            if (!_groups.Contains(newGroup))
                throw IsuExceptionFactory.AlienGroupException(newGroup);

            if (newGroup.Students.Count == _configuration.MaxStudentCount)
                throw IsuExceptionFactory.MaximumStudentCountException(newGroup, _configuration.MaxStudentCount);

            student.Group.RemoveStudent(student);
            student.Group = newGroup;
            newGroup.AddStudent(student);
        }
    }
}