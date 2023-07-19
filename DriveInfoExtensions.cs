using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DriveService
{
    public static class DriveInfoExtensions
    {
        private static readonly Regex LetterRegex = new Regex("^[A-Z]:", RegexOptions.Compiled);

        public static string GetLetter(this DriveInfo driveInfo)
        {
            if (driveInfo == null)
                throw new ArgumentNullException(nameof(driveInfo));

            return LetterRegex.Match(driveInfo.Name).Value;
        }
    }
}