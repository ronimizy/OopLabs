using System;
using System.Collections.Generic;
using Isu.Entities;
using Isu.Models;
using Isu.Services.Implementations;

namespace Isu.Services
{
    public interface IIsuService
    {
        Faculty AddFaculty(string name, char letter);
        Faculty? FindFaculty(char letter);

        Group AddGroup(GroupName name);

        Mentor AddMentor(string name);

        Student AddStudent(Group group, string name);

        Student GetStudent(Guid id);

        Student? FindStudent(string name);

        IReadOnlyList<Student> FindStudents(GroupName groupName);

        IReadOnlyList<Student> FindStudents(CourseNumber courseNumber);

        Group? FindGroup(GroupName groupName);

        IReadOnlyList<Group> FindGroups(CourseNumber courseNumber);

        void ChangeStudentGroup(Student student, Group newGroup);

        static IIsuService Create(IsuServiceConfiguration configuration)
            => new IsuService(configuration);
    }
}