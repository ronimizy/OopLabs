namespace BackupsExtra.Tools
{
    internal static class ExceptionFactory
    {
        public static BackupsExtraException InvalidFileExtensionException(string name, string extension)
            => new BackupsExtraException($"Backup Job save file must have an extension {extension}, name: {name}");
    }
}