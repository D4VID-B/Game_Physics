using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using System.Runtime.InteropServices;

public class MyUnityPlugin
{
    [DllImport("Unity_C++_Integration")]

    public static extern int InitFoo(int f_new = 0);


    [DllImport("Unity_C++_Integration")]

    public static extern int DoFoo(int bar = 0);


    [DllImport("Unity_C++_Integration")]

    public static extern int TermFoo();


    [DllImport("Unity_C++_Integration")]
    public static extern void InitAndPush(int ID);


    [DllImport("Unity_C++_Integration")]
    public static extern void InitPool();


    [DllImport("Unity_C++_Integration")]
    public static extern void updateObjectsInPool(int chunkSize = 5);


    [DllImport("Unity_C++_Integration")]
    public static extern int getObjColor(int color);

}
