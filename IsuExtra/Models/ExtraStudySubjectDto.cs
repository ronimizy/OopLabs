using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Entities;

namespace IsuExtra.Models
{
    public record ExtraStudySubjectDto(Guid Id,
                                       string Name,
                                       Guid FacultyId,
                                       IReadOnlyCollection<ExtraStudyStream> AvailableStreams);

    internal static class ExtraStudySubjectDtoConverter
    {
        public static ExtraStudySubjectDto ToDto(this ExtraStudySubject subject, Schedule filteringSchedule)
            => new ExtraStudySubjectDto(subject.Id, subject.Name, subject.Faculty.Id, subject.Streams
                                            .Where(stream => !filteringSchedule.IsIntersectsWith(stream.Schedule))
                                            .Where(stream => stream.Count < stream.Capacity)
                                            .ToList());
    }
}