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
//MYUNITYPLUGIN_SYMBOL Vector3 getPosition(int ID);
//MYUNITYPLUGIN_SYMBOL Vector3 getRotation(int ID);
//MYUNITYPLUGIN_SYMBOL Vector3 getScale(int ID);
MYUNITYPLUGIN_SYMBOL int getRadius(int ID);
//MYUNITYPLUGIN_SYMBOL Vector3 getDimentions(int ID);

//Object Data Modification
MYUNITYPLUGIN_SYMBOL void setPosition(int ID);
MYUNITYPLUGIN_SYMBOL void setRotation(int ID);
MYUNITYPLUGIN_SYMBOL void setScale(int ID);
MYUNITYPLUGIN_SYMBOL void setRadius(int ID);
MYUNITYPLUGIN_SYMBOL void setDimentions(int ID);

//Pool Functions
MYUNITYPLUGIN_SYMBOL void updateObjectsInPool(int chunkSize);
MYUNITYPLUGIN_SYMBOL void addObjToPool(int ID);
MYUNITYPLUGIN_SYMBOL void clearPool(int ID);

#ifdef __cplusplus
}

#else
//Non-C/C++ stuff
#endif // __cplusplus


#endif // !MYUNITYPLUGIN_H