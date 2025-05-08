/*
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System.IO;

namespace Sigurd.Thunderstore.Sdk.Tasks.Extensions;

public static class FileSystemInfoExtensions
{
    public static bool HasAttributes(this FileSystemInfo info, FileAttributes attributes)
        => (info.Attributes & attributes) == attributes;

    public static string GetFullNameRelativeToFile(this FileSystemInfo info, string file)
        => info.GetFullNameRelativeTo(Path.GetDirectoryName(file)!);

    public static string GetFullNameRelativeTo(this FileSystemInfo info, string dir)
        => PathNetCore.GetRelativePath(dir, info.FullName);

    public static DirectoryInfo GetSubdirectory(this DirectoryInfo dir, string name)
        => new(PathNetCore.Join(dir.FullName, name));

    public static FileInfo GetFile(this DirectoryInfo dir, string name)
        => new(PathNetCore.Join(dir.FullName, name));
}
