#pragma once

struct Vector3
{
private:

	float x;
	float y;
	float z;

public:

	float get_X() { return x; }
	float get_Y() { return y; }
	float get_Z() { return z; }

	float normalize()
	{

		return 0;
	}

	float magnitude()
	{

		return 0;
	}

	Vector3() { x = 0.0f; y = 0.0f; z = 0.0f; }
	Vector3(float _x, float _y, float _z) { x = _x; y = _y; z = _z; }
};

struct Transform
{
private:

	Vector3 mPosition;
	Vector3 mRotation;
	Vector3 mScale;

public:

	Transform()
	{
		mPosition = Vector3();
		mRotation = Vector3();
		mScale = Vector3();
	}

	Vector3 getPosition() { return mPosition; }
	Vector3 getRotation() { return mRotation; }
	Vector3 getScale() { return mScale; }

	void setPosition(Vector3 newPos) { Vector3 mPosition = newPos; }
	void setRotation(Vector3 newRot) { mRotation = newRot; }
	void setScale(Vector3 newSc) { mScale = newSc; }
};

class Object
{
protected:

	Transform mTransform;
	CircleCollider c_Col;
	BoxCollider b_Col;

	int mObjectID;
	int color = 0;

public:

	Object(const Object &other)
	{
		mTransform = other.mTransform;
		mObjectID = other.mObjectID;
		color = other.color;
	}

	Object() { mObjectID = -1; };

	Object(int ID) { mObjectID = ID; }

	Object(Vector3 pos, Vector3 rot, Vector3 scl, int iD, int colType) 
	{
		mTransform.setPosition(pos);
		mTransform.setRotation(rot);
		mTransform.setScale(scl);
		mObjectID = iD;
		color = colType; //Temporary
	}

	Object(Vector3 pos, Vector3 rot, Vector3 scl, int iD, int colType)
	{
		mTransform.setPosition(pos);
		mTransform.setRotation(rot);
		mTransform.setScale(scl);
		mObjectID = iD;
		color = colType; //Temporary
	}

	void flipColor()
	{
		if (color == 0)
		{
			color = 1;
		}
		else color = 0;
	}

	int getID() { return mObjectID; }
};

class Collider 
{
	//Needed??

	bool checkCollision(); //Some collision functionality
};

class CircleCollider : public Collider
{
private:
	float mRadius;


public:

	float getRadius() { return mRadius; }
	void setRadius(float newRad) { mRadius = newRad; }

	CircleCollider() { }
	CircleCollider(float rad, int iD) 
	{ 
		mRadius = rad;
	}
};

class BoxCollider : public Collider
{
private:

	float mLength = 0;
	float mHeight = 0;
	float mWidth = 0;
	//Could be replaced by a Vector3

public:

	float getLength() { return mLength; }
	float getHeight() { return mHeight; }
	float getWidth() { return mWidth; }

	void setLength(float newLen) { mLength = newLen; }
	void setHeight(float newHei) { mHeight = newHei; }
	void setWidth(float newWdt) { mWidth = newWdt; }

	BoxCollider() {}
	BoxCollider(float length, float height, float width, int iD) 
	{
		mLength = length;
		mHeight = height;
		mWidth = width;
	}
};


class RigidBody : public Object
{
	//Collision Tests here or in collider?

};

class SoftBody : public Object
{
	//Cloth and stuff
};

class Particle : public Object
{
	//Polymorphic class containint both 2D and 3D dunctionality
	//Or just 3D stuff - z ignored if used in 2D (bool flag)
};