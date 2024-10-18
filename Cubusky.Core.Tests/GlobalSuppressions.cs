// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "TheoryData may be used for multiple test methods. In these cases, some parameters will be unused.", Scope = "namespaceanddescendants", Target = "~N:Cubusky.Heatmaps.Tests")]
