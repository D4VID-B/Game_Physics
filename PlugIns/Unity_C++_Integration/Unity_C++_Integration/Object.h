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
};

class Object
{
protected:

	Vector3 mPosition;
	Vector3 mRotation;
	Vector3 mScale;

	int mObjectType = 0;
	int mObjectID;

	bool mActive = false;

public:

	Object(Vector3 pos, Vector3 rot, Vector3 scl) 
	{
		mPosition = pos;
		mRotation = rot;
		mScale = scl;
	}

	int getType() { return mObjectType; }
	int getID() { return mObjectID; }

	Vector3 getPosition() { return mPosition; }

	Vector3 getRotation() { return mRotation; }

	Vector3 getScale() { return mScale; }

	void setPosition(Vector3 newPos) { mPosition = newPos; }

	void setRotation(Vector3 newRot) { mRotation = newRot; }

	void setScale(Vector3 newSc) { mScale = newSc; }

	bool isInUse() { return mActive; }
};

class Collider : public Object 
{
	//Needed??
};

class CircleCollider : public Object
{
private:
	float mRadius;

public:

	float getRadius() { return mRadius; }
	void setRadius(float newRad) { mRadius = newRad; }

	CircleCollider() { mObjectType = 1; }
};

class BoxCollider : public Object
{
private:

	float mLength = 0;
	float mHeight = 0;
	float mWidth = 0;

public:

	float getLength() { return mLength; }
	float getHeight() { return mHeight; }
	float getWidth() { return mWidth; }

	void setLength(float newLen) { mLength = newLen; }
	void setHeight(float newHei) { mHeight = newHei; }
	void setWidth(float newWdt) { mWidth = newWdt; }

	BoxCollider() { mObjectType = 2; }
	BoxCollider(float length, float height, float width) {}
};


class RigidBody : public Object
{
	//Collision Tests?

};

class SoftBody : public Object
{

};

class Particle : public Object
{

};