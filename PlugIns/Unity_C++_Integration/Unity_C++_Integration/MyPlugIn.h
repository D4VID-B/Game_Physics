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
MYUNITYPLUGIN_SYMBOL void InitAndPushObj(int ID);
MYUNITYPLUGIN_SYMBOL void InitPool(int poolSize);


//MYUNITYPLUGIN_SYMBOL bool** updateCollisionsInPool();	//either update collisions in pool (required collision data in here)
//MYUNITYPLUGIN_SYMBOL Object** getArrayOfCollisionsToTest(); //push out an array of collisions for unity to test

//Object identification
MYUNITYPLUGIN_SYMBOL int getObjID();

//Object Data Access
//MYUNITYPLUGIN_SYMBOL Vector3 getPosition();
//MYUNITYPLUGIN_SYMBOL Vector3 getRotation();
//MYUNITYPLUGIN_SYMBOL Vector3 getScale();
MYUNITYPLUGIN_SYMBOL int getRadius();
//MYUNITYPLUGIN_SYMBOL Vector3 getDimentions();

//Object Data Modification
MYUNITYPLUGIN_SYMBOL void setPosition();
MYUNITYPLUGIN_SYMBOL void setRotation();
MYUNITYPLUGIN_SYMBOL void setScale();
MYUNITYPLUGIN_SYMBOL void setRadius();
MYUNITYPLUGIN_SYMBOL void setDimentions();

//Pool Functions
MYUNITYPLUGIN_SYMBOL void updateObjectsInPool(int chunkSize);
MYUNITYPLUGIN_SYMBOL void addObjToPool(int ID);
MYUNITYPLUGIN_SYMBOL void clearPool(int id);

#ifdef __cplusplus
}

#else
//Non-C/C++ stuff
#endif // __cplusplus


#endif // !MYUNITYPLUGIN_H