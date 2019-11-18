#pragma once
#include "Object.h"
#include <vector>

class Pool
{
private:

	Bullet mObjectPool[30];
	Bullet mTheBullet;

	void addBullet(Bullet object, int i)
	{
		mObjectPool[i] = object;
		//std::cout << "Add" << std::endl;
	}

public:

	Pool() {};
	~Pool() {};

	Pool::Pool(Sprite bulletSprite)
	{
		mTheBullet = Bullet(bulletSprite);

		for (int i = 0; i < 30; i++)
		{

			addBullet(mTheBullet, i);
		}

	}

	Bullet* getFreeObject()
	{
		Bullet* freeShot = nullptr;

		for (int i = 0; i < 30; i++)
		{
			if (!(mObjectPool[i].isInUse())) //Object is not in use
			{
				freeShot = &mObjectPool[i];
			}
		}

		return freeShot;

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