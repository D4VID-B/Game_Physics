#pragma once

struct IntVector3
{
private:

	int x;
	int y;
	int z;

public:

	int get_X() { return x; };
	int get_Y() { return y; };
	int get_Z() { return z; };

	int normalise()
	{
		return 0;
	}

	int magnitude()
	{
		return 0;
	}
};


class Object
{
private:


};