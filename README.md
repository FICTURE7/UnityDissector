UnityDissector
=================

Small command-line program used to unpack or repack(still beta) unity3d files. [Download](https://github.com/FICTURE7/Unity3D-Disassembler/releases)!

Example usage
```
UnityDissector -h
UnityDissector -ex test.unity3d
UnityDissector -l test.unity3d
```

When repacking file in a sub directory. (E.g: root/resources/unity_built_in_extra). You will have to copy the file(in my case unity_built_in_extra) and paste it in the root directory and replace the '/' by '!'. 

(E.g: root/resources!unity_built_in_extra)

The program will automatically replace the '!' by a '\'.
