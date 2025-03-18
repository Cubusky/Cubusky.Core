using Cubusky.IO.Serialization;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Cubusky.Tests.IO.Serialization
{
    public partial class StreamSerializerTests
    {
        public static readonly TheoryData<IStreamSerializer, string> StreamSerializers = new TheoryData<IStreamSerializer, string>()
        {
            { new JsonSerializer(), Person.Json },
#if NET8_0_OR_GREATER
            { new JsonSerializer(PersonContext.Default.Person), Person.Json },
            { new JsonSerializer(PersonContext.Default), Person.Json },
#endif
        };

        public static readonly TheoryData<IAsyncStreamSerializer, string> AsyncStreamSerializers = new TheoryData<IAsyncStreamSerializer, string>()
        {
            { new JsonSerializer(), Person.Json },
#if NET8_0_OR_GREATER
            { new JsonSerializer(PersonContext.Default.Person), Person.Json },
            { new JsonSerializer(PersonContext.Default), Person.Json },
#endif
        };

        [Theory]
        [MemberData(nameof(StreamSerializers))]
        public void IStreamSerializer_Serialize(IStreamSerializer serializer, string serializedString)
        {
            using var memory = new MemoryStream();
            serializer.Serialize(memory, Person.JohnDoe);

            memory.Position = 0;
            using var reader = new StreamReader(memory);
            Assert.Equal(serializedString, reader.ReadToEnd());
        }

        [Theory]
        [MemberData(nameof(StreamSerializers))]
        public void IStreamSerializer_Deserialize(IStreamSerializer serializer, string serializedString)
        {
            using var memory = new MemoryStream();
            using var writer = new StreamWriter(memory);
            writer.Write(serializedString);
            writer.Flush();
            memory.Position = 0;

            var person = serializer.Deserialize<Person>(memory);
            Assert.Equal(Person.JohnDoe, person);
        }

        [Theory]
        [MemberData(nameof(AsyncStreamSerializers))]
        public async Task IAsyncStreamSerializer_SerializeAsync(IAsyncStreamSerializer serializer, string serializedString)
        {
            await using var memory = new MemoryStream();
            await serializer.SerializeAsync(memory, Person.JohnDoe);

            memory.Position = 0;
            using var reader = new StreamReader(memory);
            Assert.Equal(serializedString, reader.ReadToEnd());
        }

        [Theory]
        [MemberData(nameof(AsyncStreamSerializers))]
        public async Task IAsyncStreamSerializer_DeserializeAsync(IAsyncStreamSerializer serializer, string serializedString)
        {
            await using var memory = new MemoryStream();
            await using var writer = new StreamWriter(memory);
            writer.Write(serializedString);
            writer.Flush();
            memory.Position = 0;

            var person = await serializer.DeserializeAsync<Person>(memory);
            Assert.Equal(Person.JohnDoe, person);
        }
    }
}
