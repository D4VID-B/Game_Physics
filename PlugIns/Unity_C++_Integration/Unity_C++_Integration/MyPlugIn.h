#ifndef MYUNITYPLUGIN_H
#define MYUNITYPLUGIN_H

#include "Lib.h"

#ifdef __cplusplus
extern "C"
{
#else
//Non-C/C++ stuff
#endif // __cplusplus

//Construction
MYUNITYPLUGIN_SYMBOL void InitObj();
MYUNITYPLUGIN_SYMBOL void InitCircle();
MYUNITYPLUGIN_SYMBOL void InitBox();

//Object identification
MYUNITYPLUGIN_SYMBOL int getObjID();
MYUNITYPLUGIN_SYMBOL int getObjType();

//Object Data Access
MYUNITYPLUGIN_SYMBOL Vector3 getPosition();
MYUNITYPLUGIN_SYMBOL Vector3 getRotation();
MYUNITYPLUGIN_SYMBOL Vector3 getScale();
MYUNITYPLUGIN_SYMBOL int getRadius();
MYUNITYPLUGIN_SYMBOL Vector3 getDimentions();

//Object Data Modification
MYUNITYPLUGIN_SYMBOL void setPosition();
MYUNITYPLUGIN_SYMBOL void setRotation();
MYUNITYPLUGIN_SYMBOL void setScale();
MYUNITYPLUGIN_SYMBOL void setRadius();
MYUNITYPLUGIN_SYMBOL void setDimentions();

//Pool Functions
MYUNITYPLUGIN_SYMBOL int getNextFreeObj();
MYUNITYPLUGIN_SYMBOL bool addObjToPool();
MYUNITYPLUGIN_SYMBOL void clearPool();

#ifdef __cplusplus
}

#else
//Non-C/C++ stuff
#endif // __cplusplus


#endif // !MYUNITYPLUGIN_H