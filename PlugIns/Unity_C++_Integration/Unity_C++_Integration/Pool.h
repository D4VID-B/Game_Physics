#pragma once
#include "Object.h"

class Pool
{
private:

	Object mObjectPool[10];
	Object mTheObject;

	void addObject(Object object, int i)
	{
		mObjectPool[i] = object;
	}

public:

	Pool() {};
	~Pool() {};

	Object getFreeObject()
	{
		Object freeObj;

		for (int i = 0; i < 30; i++)
		{
			if (mObjectPool[i].isInUse() == true) //Object is not in use
			{
				freeObj = mObjectPool[i];
			}
		}

		return freeObj;

	}
};