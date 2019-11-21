#pragma once
#include "Object.h"
#include <vector>
#include <thread>

class Pool
{
private:

	std::vector<Object> mObjectPool;
	static const int numThreads = 5;

	//std::thread threadList[numThreads];

public:



	Pool() {};
	~Pool() {};

	/*void flipColor(Object obj)
	{
		if (obj.color == 0)
		{
			obj.color = 1;
		}
		else
		{
			obj.color = 0;
		}
	}*/

	void addObject(Object object)
	{
		mObjectPool.push_back(object);
	}

	void updatePool(int chunckSize)
	{
		/*if (numThreads == chunckSize)
		{

			for (auto i = 0; i < chunckSize; ++i)
			{
				Object tmp;
				threadList[i] = std::thread(&Pool::flipColor, mObjectPool.at(i));
				
			}

			for (auto i = 0; i < chunckSize; ++i)
			{
				threadList[i].join();
				Object temp = mObjectPool.at(i);
				mObjectPool.erase(mObjectPool.begin());
				mObjectPool.push_back(temp);

			}

		}
		else
		{*/
			for (auto i = 0; i < chunckSize; ++i)
			{
				mObjectPool.at(i).flipColor();
				Object temp = mObjectPool.at(i);
				mObjectPool.erase(mObjectPool.begin());
				mObjectPool.push_back(temp);
			}
		//}

		
	}

	void clearPool()
	{
		mObjectPool.clear();
	}

};