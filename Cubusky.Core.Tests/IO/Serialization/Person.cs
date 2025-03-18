#if NET8_0_OR_GREATER
using System.Text.Json.Serialization;
#endif

namespace Cubusky.Tests.IO.Serialization
{
    public struct Person
    {
        public static readonly Person JohnDoe = new Person()
        {
            Name = "John Doe",
            Age = 30,
            GenderIdentity = "Genderqueer",
        };
        public const string Json = "{\"Name\":\"John Doe\",\"Age\":30,\"GenderIdentity\":\"Genderqueer\"}";

        public string Name { get; set; }
        public int Age { get; set; }
        public string GenderIdentity { get; set; }
    }

#if NET8_0_OR_GREATER
    [JsonSerializable(typeof(Person), GenerationMode = JsonSourceGenerationMode.Metadata)]
    public partial class PersonContext : JsonSerializerContext { }
#endif
}
