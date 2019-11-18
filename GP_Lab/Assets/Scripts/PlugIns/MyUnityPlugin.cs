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
}
