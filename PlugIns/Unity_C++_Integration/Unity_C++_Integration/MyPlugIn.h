#ifndef MYUNITYPLUGIN_H
#define MYUNITYPLUGIN_H

#include "Lib.h"

#ifdef __cplusplus
extern "C"
{
#else
//Non-C/C++ stuff
#endif // __cplusplus

MYUNITYPLUGIN_SYMBOL int InitFoo(int f_new);
MYUNITYPLUGIN_SYMBOL int DoFoo(int bar);
MYUNITYPLUGIN_SYMBOL int TermFoo();

#ifdef __cplusplus
}

#else
//Non-C/C++ stuff
#endif // __cplusplus


#endif // !MYUNITYPLUGIN_H