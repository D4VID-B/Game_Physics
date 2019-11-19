#pragma once
#include "Object.h"
#include <vector>

class Pool
{
private:

	Object mObjectPool[10];
	Object mTheObject;

	void addBullet(Object object, int i)
	{
		mObjectPool[i] = object;
	}

public:

	Pool() {};
	~Pool() {};

	Object* getFreeObject()
	{
		Object* freeObj = nullptr;

		for (int i = 0; i < 30; i++)
		{
			if (mObjectPool[i].isInUse() == true) //Object is not in use
			{
				freeObj = &mObjectPool[i];
			}
		}

		return freeObj;

	}
};


//template<typename T>
//class Pool : public Trackable
//{
//private:
//
//	T* objectPool;
//
//
//public:
//
//	Pool::Pool(int size)
//	{
//		objectPool = new objectPool[size];
//
//		for (int i = 0; i < size; i++)
//		{
//
//		}
//	}
//	
//	bool isInUse(T object)
//	{
//
//	}
//
//	T getFreeObject()
//	{
//
//	}
//};