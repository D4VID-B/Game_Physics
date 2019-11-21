#include "MyPlugIn.h"

#include "Foo.h"
#include "Object.h"
#include "Pool.h"



void InitObj(Vector3 pos, Vector3 rot, Vector3 scl)
{
	Object newObj(pos, rot, scl);
}

void InitCircle(float rad, Vector3 pos, Vector3 rot, Vector3 scl)
{
	CircleCollider newCirc(rad);
	newCirc.setPosition(pos);
	newCirc.setRotation(rot);
	newCirc.setScale(scl);
}

void InitBox(float l, float w, float h, Vector3 pos, Vector3 rot, Vector3 scl)
{
	BoxCollider newBox(l, w, h);
	newBox.setPosition(pos);
	newBox.setRotation(rot);
	newBox.setScale(scl);
}

void InitPool(int poolSize)
{
	Pool newPool(poolSize);
}

int getObjID()
{

}

int getObjType()
{

}
