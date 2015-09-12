# UnityDissector
Command-line application that can extract files from .unity3d format.

## Compiling
The simplest way to compile it is by using Visual Studio and press `F6` or by using MonoDevelop.

You can also compile it by using the latest version of mono
```
git clone git://github.com/FICTURE7/UnityDissector.git
```
In the root directory where `UnityDissector.sln` is found run
```
xbuild
```
And you should have the binaries in `\bin\Debug`.

## Usage
UnityDissector has 3 options currently

|     Option     |                        Description                        |
|----------------|-----------------------------------------------------------|
| --help, -l     | Prints information on how to use this thing and exits.    |
| --extract, -ex | Extracts the files inside of the specified .unity3d file. |
| --list, -l     | Prints all files inside of the specified .unity3d file.   |

#### Examples
Extracting files from `test.unity3d`
```
[mono] UnityDissector -ex test.unity3d
```
Or
```
[mono] UnityDissector --extract test.unity3d
```

Listing files from `test.unity3d`
```
[mono] UnityDissector -l test.unity3d
```
Or
```
[mono] UnityDissector --list test.unity3d
```

Printing help
```
[mono] UnityDissector -h
```
or
```
[mono] UnityDissector --help
```
