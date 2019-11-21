#pragma once
#include "Object.h"
#include <vector>

class Pool
{
private:

	
	std::vector<Object*> mObjectPool;
	Object mTheObject;
	int mPoolSize;

public:

	Pool(int poolSize) { mPoolSize = poolSize; };
	~Pool() {};

	void addObject(Object* object)
	{
		////if we arent at capacity, fill the pool
		//if (mObjectPool.size() < mPoolSize)
		//{
		//	mObjectPool.push_back(object);
		//}

		////if we are at capacity the object wont be added

		//add object no matter what 
		mObjectPool.push_back(object);
	}


	//this can be called once every frame, once every 10 frames, once every X frames
	void updatePool()
	{
		Object* tempObj = nullptr;

		for (int i = 0; i < mPoolSize; i++)
		{
			//update collision data
		}
	}

	/*
	Object getFreeObject()
	{
		Object freeObj = NULL;

		//only do based on size of pool
		for (int i = 0; i < mPoolSize; i++)
		{
			
			if (mObjectPool[i].isInUse() == true) //Object is not in use
			{
				freeObj = mObjectPool[i];
			}
			
		}

		return freeObj;

	}
	*/
};