using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Design",
    "RCS1090:Call 'ConfigureAwait(false)'.",
    Justification = "No synchronisation context in ASP.NET Core...")]
