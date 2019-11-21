#include "MyPlugIn.h"

#include "Foo.h"
#include "Object.h"
#include "Pool.h"

//using globals is wrong but this is basic ass shit (instead use singleton)
Pool* POOL = nullptr;

void InitAndPushObj(int ID)
{
	Object newObj(ID);

	if (POOL != nullptr)
	{
		POOL->addObject(newObj);
	}
}


void InitPool()
{
	POOL = new Pool();
}

void updateObjectsInPool(int chunkSize)
{
	POOL->updatePool(chunkSize);
}

int getObjColor(int ID)
{
	return POOL->getObjectColor(ID);
}

void addObjToPool(int ID)
{

}

int getObjectID()
{
	return -1;
}

void setPosition(int ID)
{

}

void setRotation(int ID)
{

}

void setScale(int ID)
{

}

void setRadius(int ID)
{

}

void setDimentions(int ID)
{

}