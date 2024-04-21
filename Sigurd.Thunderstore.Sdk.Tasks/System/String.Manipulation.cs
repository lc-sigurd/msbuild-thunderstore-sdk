/*
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-or-later license.
 */

using System.Text;

namespace System;

#if NETFRAMEWORK
public partial class StringNetCore
{
    public static string Concat(ReadOnlySpan<char> first, ReadOnlySpan<char> second)
    {
        int length = first.Length + second.Length;
        if (length == 0) return string.Empty;

        var builder = new ValueStringBuilder(length);
        builder.Append(first);
        builder.Append(second);
        return builder.ToString();
    }

    public static string Concat(ReadOnlySpan<char> first, ReadOnlySpan<char> second, ReadOnlySpan<char> third)
    {
        int length = first.Length + second.Length + third.Length;
        if (length == 0) return string.Empty;

        var builder = new ValueStringBuilder(length);
        builder.Append(first);
        builder.Append(second);
        builder.Append(third);
        return builder.ToString();
    }

    public static string Concat(ReadOnlySpan<char> first, ReadOnlySpan<char> second, ReadOnlySpan<char> third, ReadOnlySpan<char> fourth)
    {
        int length = first.Length + second.Length + third.Length + fourth.Length;
        if (length == 0) return string.Empty;

        var builder = new ValueStringBuilder(length);
        builder.Append(first);
        builder.Append(second);
        builder.Append(third);
        builder.Append(fourth);
        return builder.ToString();
    }

    public static string Concat(ReadOnlySpan<char> first, ReadOnlySpan<char> second, ReadOnlySpan<char> third, ReadOnlySpan<char> fourth, ReadOnlySpan<char> fifth)
    {
        int length = first.Length + second.Length + third.Length + fourth.Length + fifth.Length;
        if (length == 0) return string.Empty;

        var builder = new ValueStringBuilder(length);
        builder.Append(first);
        builder.Append(second);
        builder.Append(third);
        builder.Append(fourth);
        builder.Append(fifth);
        return builder.ToString();
    }

    public static string Concat(string? first, string? second)
    {
        if (string.IsNullOrEmpty(first))
            return second ?? string.Empty;

        if (string.IsNullOrEmpty(second))
            return first!;

        var builder = new ValueStringBuilder(first!.Length + second!.Length);
        builder.Append(first);
        builder.Append(second);
        return builder.ToString();
    }

    public static string Concat(string? first, string? second, string? third)
    {
        if (string.IsNullOrEmpty(first))
            return Concat(second, third);

        if (string.IsNullOrEmpty(second))
            return Concat(first, third);

        if (string.IsNullOrEmpty(third))
            return Concat(first, second);

        var builder = new ValueStringBuilder(first!.Length + second!.Length + third!.Length);
        builder.Append(first);
        builder.Append(second);
        builder.Append(third);
        return builder.ToString();
    }

    public static string Concat(string? first, string? second, string? third, string? fourth)
    {
        if (string.IsNullOrEmpty(first))
            return Concat(second, third, fourth);

        if (string.IsNullOrEmpty(second))
            return Concat(first, third, fourth);

        if (string.IsNullOrEmpty(third))
            return Concat(first, second, fourth);

        if (string.IsNullOrEmpty(fourth))
            return Concat(first, second, third);

        var builder = new ValueStringBuilder(first!.Length + second!.Length + third!.Length);
        builder.Append(first);
        builder.Append(second);
        builder.Append(third);
        builder.Append(fourth);
        return builder.ToString();
    }

    public static string Concat(params string?[] values)
    {
        int length = 0;
        foreach (string? value in values)
        {
            length += value?.Length ?? 0;
        }

        var builder = new ValueStringBuilder(length);
        foreach (string? value in values)
        {
            builder.Append(value);
        }

        return builder.ToString();
    }
}
#endif
