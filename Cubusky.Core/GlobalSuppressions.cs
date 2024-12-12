// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "Target Frameworks includes .NET Standard 2.1, which does not support primary constructors.", Scope = "namespaceanddescendants", Target = "~N:Cubusky")]
[assembly: SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "Target Frameworks includes .NET Standard 2.1, which does not support 'new(...)'.", Scope = "namespaceanddescendants", Target = "~N:Cubusky")]
[assembly: SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "Target Frameworks includes .NET Standard 2.1, which does not support simplified collection initialization.", Scope = "namespaceanddescendants", Target = "~N:Cubusky")]
[assembly: SuppressMessage("Style", "IDE0300:Simplify collection initialization", Justification = "Target Frameworks includes .NET Standard 2.1, which does not support simplified collection initialization.", Scope = "namespaceanddescendants", Target = "~N:Cubusky")]
#endif