Unity3D-Deompiler
=================

Some classes written(pretty badly) in C# that can be used to decompile/unpack .unity3d files.

###Usage Example
```javascript
    Unpacker unpacker = new Unpacker("Direcory pointing to file");
    unpacker.Unpack();
```
=================
###TODO List
- [ ] Clean the code.
- [ ] Add repacking function.
- [ ] Add output log file.

=================
###Info
This section will be giving you the info about .unity3d files header and other stuffs and thing.
>Some of the info might not be 100% true.

.unity3d files content are compressed using the [LZMA](http://en.wikipedia.org/wiki/Lempel%E2%80%93Ziv%E2%80%93Markov_chain_algorithm) compression algorithm. You can get the latest SDK [here](http://www.7-zip.org/sdk.html).

####Compressed File Header
