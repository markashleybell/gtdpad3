using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Reliability",
    "CA2007:Consider calling ConfigureAwait on the awaited task",
    Justification = "No synchronisation context in ASP.NET Core...")]
[assembly: SuppressMessage(
    "Design",
    "RCS1090:Call 'ConfigureAwait(false)'.",
    Justification = "No synchronisation context in ASP.NET Core...")]

