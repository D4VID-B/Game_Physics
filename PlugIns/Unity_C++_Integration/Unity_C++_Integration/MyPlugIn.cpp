#include "MyPlugIn.h"

#include "Foo.h"
#include "Object.h"
#include "Pool.h"

//using globals is wrong but this is basic ass shit (instead use singleton)
Pool* POOL = nullptr;

void InitAndPushObj(Vector3 pos, Vector3 rot, Vector3 scl)
{
	Object newObj(pos, rot, scl);

	if (POOL != nullptr)
	{
		POOL->addObject(&newObj);
	}
}

void InitAndPushCircle(float rad, Vector3 pos, Vector3 rot, Vector3 scl)
{
	CircleCollider newCirc(rad);
	newCirc.setPosition(pos);
	newCirc.setRotation(rot);
	newCirc.setScale(scl);

	if (POOL != nullptr)
	{
		POOL->addObject(&newCirc);
	}
}

void InitAndPushBox(float l, float w, float h, Vector3 pos, Vector3 rot, Vector3 scl)
{
	BoxCollider newBox(l, w, h);
	newBox.setPosition(pos);
	newBox.setRotation(rot);
	newBox.setScale(scl);

	if (POOL != nullptr)
	{
		POOL->addObject(&newBox);
	}
}

void InitPool(int poolSize)
{
	POOL = new Pool(poolSize);
}

void updateCollisionsInPool()
{

}

