# FolderView

Simple immutable view of a folder structure on disk or on the Internet.

[![Build status](https://ci.appveyor.com/api/projects/status/xewan6agkbf4u3xe?svg=true)](https://ci.appveyor.com/project/dlebansais/folderview) [![codecov](https://codecov.io/gh/dlebansais/FolderView/branch/master/graph/badge.svg?token=ZDdGWyk2Qb)](https://codecov.io/gh/dlebansais/FolderView)

# API

## LocalLocation : ILocation

| Name      | Type     | Comment |
|-----------|----------|---------|
| LocalRoot | `string` | The path to the root folder of interest |

## GitHubLocation : ILocation

| Name           | Type     | Comment |
|----------------|----------|---------|
| UserName       | `string` | The user name for repositories on GitHub |
| RepositoryName | `string` | The repository name |
| RemoteRoot     | `string` | The path to the root folder in the repository, / for the repository root |

## Path : IPath

| Name      | Type            | Comment |
|-----------|-----------------|---------|
| Ancestors | `IList<string>` | The list of ancestor folders, from the root to the folder, an empty list for the root folder |
| Name      | `string`        | The name of a file or subfolder in the folder |

### RootFolderFrom
Gets a root folder from a local path or remote address.

`public static IFolder RootFolderFrom(ILocation location)`

### Combine
Combines a parent folder, or path, and a name to return the path to that name. In the case of a folder, a `null` parent indicates the root folder.

`public static IPath Combine(IFolder? parent, string name)`<br/>
`public static IPath Combine(IPath parent, string name)`

### GetRelativeFolder
Gets the folder starting from a parent and following a path.

`public static IFolder GetRelativeFolder(IFolder parent, IPath path)`

### GetRelativeFile
Gets the folder starting from a parent and following a path.

`public static IFile GetRelativeFile(IFolder parent, IPath path)`




