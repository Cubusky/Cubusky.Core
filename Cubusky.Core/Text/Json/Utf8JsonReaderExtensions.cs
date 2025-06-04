using System;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Cubusky.Text.Json
{
    internal static class Utf8JsonReaderExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotTokenType(this ref Utf8JsonReader reader, JsonTokenType tokenType)
        {
            if (reader.TokenType != tokenType)
            {
                throw new JsonException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotTokenType(this ref Utf8JsonReader reader, params JsonTokenType[] tokenTypes)
        {
            if (Array.IndexOf(tokenTypes, reader.TokenType) == -1)
            {
                throw new JsonException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadOrThrow(this ref Utf8JsonReader reader)
        {
            if (!reader.Read())
            {
                throw new JsonException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Read(this ref Utf8JsonReader reader, JsonTokenType tokenType) => reader.Read() && reader.TokenType == tokenType;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadOrThrow(this ref Utf8JsonReader reader, JsonTokenType tokenType)
        {
            if (!reader.Read(tokenType))
            {
                throw new JsonException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Read(this ref Utf8JsonReader reader, params JsonTokenType[] tokenTypes) => reader.Read() && Array.IndexOf(tokenTypes, reader.TokenType) != -1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadOrThrow(this ref Utf8JsonReader reader, params JsonTokenType[] tokenTypes)
        {
            if (!reader.Read(tokenTypes))
            {
                throw new JsonException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Read(this ref Utf8JsonReader reader, ReadOnlySpan<char> propertyName) => reader.Read(JsonTokenType.PropertyName) && reader.ValueTextEquals(propertyName) && reader.Read();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Read(this ref Utf8JsonReader reader, ReadOnlySpan<byte> utf8PropertyName) => reader.Read(JsonTokenType.PropertyName) && reader.ValueTextEquals(utf8PropertyName) && reader.Read();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadOrThrow(this ref Utf8JsonReader reader, ReadOnlySpan<char> propertyName)
        {
            if (!reader.Read(propertyName))
            {
                throw new JsonException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadOrThrow(this ref Utf8JsonReader reader, ReadOnlySpan<byte> utf8PropertyName)
        {
            if (!reader.Read(utf8PropertyName))
            {
                throw new JsonException();
            }
        }
    }
}
