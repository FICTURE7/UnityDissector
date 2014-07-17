Unity3D-Deompiler
=================

Some classes written(pretty badly) in C# that can be used to decompile/unpack .unity3d files.

###Usage Example
```javascript
    Unpacker unpacker = new Unpacker("Direcory pointing to file");
    unpacker.Unpack();
```
=================
###Data Types

Type | Size | Range | Note |
--- | ---| --- | --- |
int | 4 bytes | -2147483648 to 2147483647 | Not really sure if signed or not |
string | N/A | N/A | Null terminated |

=================
###Info
This section will be giving you the info about .unity3d files header and other stuffs and thing.
>Some of the info might not be 100% true.

.unity3d files content are compressed using the [LZMA](http://en.wikipedia.org/wiki/Lempel%E2%80%93Ziv%E2%80%93Markov_chain_algorithm) compression algorithm. You can get the latest SDK [here](http://www.7-zip.org/sdk.html).

####Compressed File Header

Here is an example of a compressed unity3d file's header and also get yourself a hex editor such as [HxD](http://mh-nexus.de/en/downloads.php?product=HxD)
######HEX VIEW
Offset | 00 | 01 | 02 | 03 | 04 | 05 | 06 | 07 | 08 | 09 | 0A | 0B | 0C | 0D | 0E | 0F |
--- | ---| --- | --- |--- | ---| --- | --- |--- | ---| --- | --- |--- | ---| --- | --- | --- |
| **0000000** | 55 | 6E | 69 | 74 | 79 | 57 | 65 | 62 | 00 | 00 | 00 | 00 | 03 | 33 | 2E | 78 |
| **0000010** | 2E | 78 | 00 | 34 | 2E | 32 | 2E | 31 | 66 | 34 | 00 | 00 | 24 | E5 | 0B | 00 |
| **0000020** | 00 | 00 | 3C | 00 | 00 | 00 | 01 | 00 | 00 | 00 | 01 | 00 | 24 | E4 | CF | 00 |
| **0000030** | 73 | 5C | A0 | 00 | 24 | E5 | 0B | 00 | 00 | 02 | 38 | 00 |

```
55 6E 69 74 79 57 65 62 00 00 00 00 03 33 2E 78 2E 78 00 34 2E 32 2E 31 66 34 00 00 24 E5 0B
00 00 00 3C 00 00 00 01 00 00 00 01 00 24 E4 CF 00 73 5C A0 00 24 E5 0B 00 00 02 38 00
```

######TEXT VIEW
```
U n i t y W e b . . . . . 3 . x . x . 4 . 2 . 1 f 4 . . $ å . . . . < . . . . . . . . . $ ä 
Ï . s \ . $ å . . . . 8 .
```

The first 9 bytes, `55 6E 69 74 79 57 65 62 *00*`, of the header is a string, `UnityWeb`. I guess we will call it the .unity3d file signature. 

The next 4 bytes, `00 00 00 03` bytes is an int, `3`. Still  not really sure about it, maybe its the build number? But it seems to be a static value.

The next 6 bytes, `33 2E 78 2E 78 *00*` is a string, `3.x.x`. It is the Unity Webplayer minium requirement?

The next 8 bytes, `34 2E 32 2E 31 66 34 *00*` is a string `4.2.1f4`. Maybe it could be the version of the Unity Engine it was build.

The next 4 bytes, `00 24 E5 0B` is an int, `2417931`.  It is the file size in bytes.

The next 4 bytes, `00 00 00 3C` is an int, `60`. It is the offset were the data starts/ending of header?

The next 8 bytes, `00 00 00 01 00 00 00 01`, I don`t know much about them. Maybe they are long or 2 ints but they seems to be static too.

The next 4 bytes, `00 24 E4 CF` is an int, `2417871`. The file size without the header.

The next 4 bytes, `00 73 5C A0` is an int, `7560352`. The file size decompressed.

The next 4 bytes, `00 24 E5 0B` is an int, `2417931`. The file size in bytes, again?

The next 4 bytes, `00 00 02 38` is an int, `568`. The offset were the data starts in decompressed file.

The remaining 1 byte, `00` maybe is a null terminator?

=================
###TODO List
- [ ] Complete documentation.
- [ ] Complete CompressedFileHeader.cs
- [ ] Clean the code.
- [ ] Add repacking function.
- [ ] Add output log file.
