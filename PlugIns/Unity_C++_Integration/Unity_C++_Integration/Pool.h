#pragma once
#include "Object.h"
#include <vector>

class Pool
{
private:

	std::vector<Object> mObjectPool;

public:

	Pool() {};
	~Pool() {};

	void addObject(Object object)
	{
		mObjectPool.push_back(object);
	}

	void updatePool(int chunckSize)
	{

		for (auto i = 0; i < chunckSize; ++i)
		{
			mObjectPool.at(i).flipColor();
			Object temp = mObjectPool.at(i);
			mObjectPool.erase(mObjectPool.begin());
			mObjectPool.push_back(temp);
		}
	}

	void clearPool()
	{
		mObjectPool.clear();
	}

};