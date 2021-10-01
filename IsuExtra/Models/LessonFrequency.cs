using System;

namespace IsuExtra.Models
{
    [Flags]
    public enum LessonFrequency
    {
        Odd = 1 << 1,
        Even = 1 << 2,
        Persistent = Odd | Even,
    }
}