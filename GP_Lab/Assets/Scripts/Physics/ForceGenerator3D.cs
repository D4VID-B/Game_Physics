using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator3D
{
    public static Vector3 generateForce_Gravity(float particleMass, float gravitationalConstant, Vector3 worldUp)
    {
        //f_gravity: f = mg
        Vector3 f_gravity = particleMass * gravitationalConstant * worldUp;
        return f_gravity;
    }

    // f_normal = proj(f_gravity, surfaceNormal_unit)
    public static Vector3 GenerateForce_normal(Vector3 f_gravity, Vector3 surfaceNormal_unit)
    {
        //surfaceNormal_unit = unit vector of the surface -> cos(x), sin(y)
        //proj = built into unity

        //Vector2 normalForce = Vector3.Project(surfaceNormal_unit , - f_gravity);
        Vector3 normalForce = Vector3.Project(-f_gravity, surfaceNormal_unit.normalized);
        //Debug.Log(normalForce);
        return normalForce;
    }
    // f_sliding = f_gravity + f_normal
    public static Vector3 GenerateForce_sliding(Vector3 f_gravity, Vector3 f_normal)
    {
        return f_gravity + f_normal;
    }
    // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)
    // f_friction_k = -coeff*|f_normal| * unit(vel)
    public static Vector3 GenerateForce_friction(Vector3 f_normal, Vector3 f_opposing, Vector3 particleVelocity, float frictionCoefficient_static, float frictionCoefficient_kinetic)
    {
        //lookup friction coef table for materials
        //f_opposing x = mg*sin()*cos() y = mg*sin()*sin()

        Vector3 friction = new Vector3(0f, 0f, 0f);

        if (particleVelocity == Vector3.zero)
        {
            //f_opposing is arbitrary unless you know the source (if using slope, the opposing force is the sliding)
            //first calculate the max, then compare to opposing

            //Max:
            float max = frictionCoefficient_static * f_normal.magnitude;

            //Compare
            if (f_opposing.magnitude < max)
            {
                friction = -f_opposing;
            }
            else
            {
                //friction = -max * f_opposing;
                friction = -max * f_opposing.normalized;
            }
        }
        else if(particleVelocity != Vector3.zero)
        {
            friction = -frictionCoefficient_kinetic * f_normal.magnitude * particleVelocity.normalized; 
        }

        return friction;
    }
   
    // f_drag = (p * u^2 * area * coeff)/2
    public static Vector3 GenerateForce_drag(Vector3 particleVelocity, Vector3 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        Vector3 diff = fluidVelocity - particleVelocity;

        float dragMag = (fluidDensity * diff.magnitude * objectArea_crossSection*objectDragCoefficient) * 0.5f;

        Vector3 drag = diff*dragMag;

        return drag;
    }
    // f_spring = -coeff*(spring length - spring resting length)
    public static Vector3 GenerateForce_spring(Vector3 particlePosition, Vector3 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        //both floats are arbitrary
        //coeff = stiffness
        //length (float) = particlepos - anchorpos => magnitude of the vector
        // direction - comes from the length calculation -> direction = normalise (/by length)

        Vector3 displacement = particlePosition - anchorPosition;

        float springLength = displacement.magnitude;

        float f_strength = -springStiffnessCoefficient * (springLength - springRestingLength);

        Vector3 springForce = (displacement * f_strength)/springLength;

        return springForce;
    }

    public static Vector3 genereateImpulse(Vector3 direction, float forceMultiplier)
    {
        Vector3 impulse = new Vector3();

        return impulse;
    }
}
