/*
 * The contents of this file are largely based upon
 * https://github.com/dotnet/runtime/blob/a3dc1337a17a3de6aefe76e465f85ebf5ba6ae5e/src/libraries/System.Private.CoreLib/src/System/IO/Path.cs
 * Licensed to the .NET Foundation under one or more agreements.
 * The .NET Foundation licenses the referenced file to the Sigurd Team under the MIT license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-or-later license.
 */

using System.Diagnostics;
using System.Text;

namespace System.IO;

public static partial class PathNetCore
{


    public static string Join(string? path1, string? path2)
    {
#if NETFRAMEWORK
        if (string.IsNullOrEmpty(path1))
            return path2 ?? string.Empty;

        if (string.IsNullOrEmpty(path2))
            return path1!;

        return JoinInternal(path1.AsSpan(), path2.AsSpan());
#else
        return Path.Join(path1, path2);
#endif
    }

    public static string Join(string? path1, string? path2, string? path3)
    {
#if NETFRAMEWORK
        if (string.IsNullOrEmpty(path1))
            return Join(path2, path3);

        if (string.IsNullOrEmpty(path2))
            return Join(path1, path3);

        if (string.IsNullOrEmpty(path3))
            return Join(path1, path2);

        return JoinInternal(path1.AsSpan(), path2.AsSpan(), path3.AsSpan());
#else
        return Path.Join(path1, path2, path3);
#endif
    }

    public static string Join(string? path1, string? path2, string? path3, string? path4)
    {
#if NETFRAMEWORK
        if (string.IsNullOrEmpty(path1))
            return Join(path2, path3, path4);

        if (string.IsNullOrEmpty(path2))
            return Join(path1, path3, path4);

        if (string.IsNullOrEmpty(path3))
            return Join(path1, path2, path4);

        if (string.IsNullOrEmpty(path4))
            return Join(path1, path2, path3);

        return JoinInternal(path1.AsSpan(), path2.AsSpan(), path3.AsSpan(), path4.AsSpan());
#else
        return Path.Join(path1, path2, path3, path4);
#endif
    }

    public static string Join(params string?[] paths)
    {
#if NETFRAMEWORK
        if (paths is null)
            throw new ArgumentNullException(nameof(paths));

        if (paths.Length == 0)
            return string.Empty;

        int maxSize = 0;
        foreach (string? path in paths)
        {
            maxSize += path?.Length ?? 0;
        }
        maxSize += paths.Length - 1;

        var builder = new ValueStringBuilder(stackalloc char[260]); // MaxShortPath on Windows
        builder.EnsureCapacity(maxSize);

        for (int i = 0; i < paths.Length; i++)
        {
            string? path = paths[i];
            if (string.IsNullOrEmpty(path)) continue;

            if (builder.Length == 0)
            {
                builder.Append(path);
            }
            else
            {
                if (!PathInternalNetCore.IsDirectorySeparator(builder[^1]) && !PathInternalNetCore.IsDirectorySeparator(path![0]))
                    builder.Append(Path.DirectorySeparatorChar);

                builder.Append(path);
            }
        }

        return builder.ToString();
#else
        return Path.Join(paths);
#endif
    }

#if NETFRAMEWORK
    private static unsafe string JoinInternal(ReadOnlySpan<char> first, ReadOnlySpan<char> second)
    {
        Debug.Assert(first.Length > 0 && second.Length > 0, "should have dealt with empty paths");

        bool hasSeparator = PathInternalNetCore.IsDirectorySeparator(first[^1]) || PathInternalNetCore.IsDirectorySeparator(second[0]);

        return hasSeparator ?
            StringNetCore.Concat(first, second) :
            StringNetCore.Concat(first, PathInternalNetCore.DirectorySeparatorCharAsString.AsSpan(), second);
    }

    private static unsafe string JoinInternal(ReadOnlySpan<char> first, ReadOnlySpan<char> second, ReadOnlySpan<char> third)
    {
        Debug.Assert(first.Length > 0 && second.Length > 0 && third.Length > 0, "should have dealt with empty paths");

        bool firstHasSeparator = PathInternalNetCore.IsDirectorySeparator(first[^1]) || PathInternalNetCore.IsDirectorySeparator(second[0]);
        bool secondHasSeparator = PathInternalNetCore.IsDirectorySeparator(second[^1]) || PathInternalNetCore.IsDirectorySeparator(third[0]);

        return (firstHasSeparator, secondHasSeparator) switch
        {
            (false, false) => StringNetCore.Concat(first, PathInternalNetCore.DirectorySeparatorCharAsString.AsSpan(), second, PathInternalNetCore.DirectorySeparatorCharAsString.AsSpan(), third),
            (false, true) => StringNetCore.Concat(first, PathInternalNetCore.DirectorySeparatorCharAsString.AsSpan(), second, third),
            (true, false) => StringNetCore.Concat(first, second, PathInternalNetCore.DirectorySeparatorCharAsString.AsSpan(), third),
            (true, true) => StringNetCore.Concat(first, second, third),
        };
    }

    private static unsafe string JoinInternal(ReadOnlySpan<char> first, ReadOnlySpan<char> second, ReadOnlySpan<char> third, ReadOnlySpan<char> fourth)
    {
        Debug.Assert(first.Length > 0 && second.Length > 0 && third.Length > 0 && fourth.Length > 0, "should have dealt with empty paths");

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

        var needSeparator1 = PathInternalNetCore.IsDirectorySeparator(first[^1]) || PathInternalNetCore.IsDirectorySeparator(second[0]) ? (byte)1 : (byte)0;
        var needSeparator2 = PathInternalNetCore.IsDirectorySeparator(second[^1]) || PathInternalNetCore.IsDirectorySeparator(third[0]) ? (byte)1 : (byte)0;
        var needSeparator3 = PathInternalNetCore.IsDirectorySeparator(third[^1]) || PathInternalNetCore.IsDirectorySeparator(fourth[0]) ? (byte)1 : (byte)0;

        var builder = new ValueStringBuilder(first.Length + second.Length + third.Length + fourth.Length + needSeparator1 + needSeparator2 + needSeparator3);
        builder.Append(first);
        if (needSeparator1 != 0)
            builder.Append(Path.DirectorySeparatorChar);
        builder.Append(second);
        if (needSeparator2 != 0)
            builder.Append(Path.DirectorySeparatorChar);
        builder.Append(third);
        if (needSeparator3 != 0)
            builder.Append(Path.DirectorySeparatorChar);
        builder.Append(fourth);
        return builder.ToString();
    }
#endif
}
