#pragma once
#include "Object.h"
#include <vector>

class Pool
{
private:

	
	std::vector<Object> mObjectPool;
	//Object mObjectPool[10];
	Object mTheObject;
	int mPoolSize;


	void addObject(Object object, int i)
	{
		mObjectPool[i] = object;
	}

public:

	Pool(int poolSize) { mPoolSize = poolSize; };
	~Pool() {};

	Object getFreeObject()
	{
		Object freeObj;

		for (int i = 0; i < mPoolSize; i++)
		{
			if (mObjectPool[i].isInUse() == true) //Object is not in use
			{
				freeObj = mObjectPool[i];
			}
		}

		return freeObj;

	}
};