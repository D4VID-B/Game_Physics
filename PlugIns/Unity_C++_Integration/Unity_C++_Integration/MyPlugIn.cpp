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


void InitPool(int poolSize)
{
	POOL = new Pool();
}

void updateObjectsInPool(int chunkSize)
{
	POOL->updatePool(chunkSize);
}

void addObjToPool(int ID)
{

}
