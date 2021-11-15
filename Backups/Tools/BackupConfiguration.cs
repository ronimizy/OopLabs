using System;
using Utility.Extensions;

namespace Backups.Tools
{
    public static class BackupConfiguration
    {
        public const char PathDelimiter = '/';
        public const char ExtensionDelimiter = '.';

        public static string FormatDateTime(DateTime dateTime)
            => dateTime.ToString("u");

        public static string GetTypeKey(Type type)
            => type.FullName.ThrowIfNull(nameof(Type.FullName));
    }
}