// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

#if NET8_0_OR_GREATER
[assembly: SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "Target Frameworks includes .NET Standard 2.1, which does not support primary constructors.", Scope = "namespaceanddescendants", Target = "~N:Cubusky.Benchmarks")]
[assembly: SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "Target Frameworks includes .NET Standard 2.1, which does not support 'new(...)'.", Scope = "namespaceanddescendants", Target = "~N:Cubusky.Benchmarks")]
[assembly: SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "Target Frameworks includes .NET Standard 2.1, which does not support simplified collection initialization.", Scope = "namespaceanddescendants", Target = "~N:Cubusky.Benchmarks")]
[assembly: SuppressMessage("Style", "IDE0300:Simplify collection initialization", Justification = "Target Frameworks includes .NET Standard 2.1, which does not support simplified collection initialization.", Scope = "namespaceanddescendants", Target = "~N:Cubusky.Benchmarks")]
[assembly: SuppressMessage("Style", "IDE0301:Simplify collection initialization", Justification = "Target Frameworks includes .NET Standard 2.1, which does not support simplified collection initialization.", Scope = "namespaceanddescendants", Target = "~N:Cubusky.Benchmarks")]
[assembly: SuppressMessage("Style", "IDE0305:Simplify collection initialization", Justification = "Target Frameworks includes .NET Standard 2.1, which does not support simplified collection initialization.", Scope = "namespaceanddescendants", Target = "~N:Cubusky.Benchmarks")]
[assembly: SuppressMessage("Style", "IDE0306:Simplify collection initialization", Justification = "Target Frameworks includes .NET Standard 2.1, which does not support simplified collection initialization.", Scope = "namespaceanddescendants", Target = "~N:Cubusky.Benchmarks")]
[assembly: SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "The IDE can make no claim to when a suppression is unnecessary or not.", Scope = "namespaceanddescendants", Target = "~N:Cubusky.Benchmarks")]
#endif

[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Benchmarks MUST be instance methods, static methods are not supported.", Scope = "namespaceanddescendants", Target = "~N:Cubusky.Benchmarks")]