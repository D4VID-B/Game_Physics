using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator
{
    public static Vector2 generateForce_Gravity(float particleMass, float gravitationalConstant, Vector2 worldUp)
    {
        //f_gravity: f = mg
        Vector2 f_gravity = particleMass * gravitationalConstant * worldUp;
        return f_gravity;
    }

    // f_normal = proj(f_gravity, surfaceNormal_unit)
    public static Vector2 GenerateForce_normal(Vector2 f_gravity, Vector2 surfaceNormal_unit)
    {
        //surfaceNormal_unit = unit vector of the surface -> cos(x), sin(y)
        //proj = built into unity
        
        Vector2 normalForce = Vector3.Project(f_gravity, surfaceNormal_unit);

        return normalForce;
    }
    // f_sliding = f_gravity + f_normal
    public static Vector2 GenerateForce_sliding(Vector2 f_gravity, Vector2 f_normal)
    {
        return f_gravity + f_normal;
    }
    // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)
    // f_friction_k = -coeff*|f_normal| * unit(vel)
    public static Vector2 GenerateForce_friction(Vector2 f_normal, Vector2 f_opposing, Vector2 particleVelocity, float frictionCoefficient_static, float frictionCoefficient_kinetic)
    {
        //lookup friction coef table for materials
        //f_opposing x = mg*sin()*cos() y = mg*sin()*sin()

        Vector2 friction = new Vector2(0f, 0f);

        if (particleVelocity == Vector2.zero)
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
            else if (f_opposing.magnitude > max)
            {
                friction = (-frictionCoefficient_static * f_normal) * particleVelocity.normalized;
            }
        }
        else if(particleVelocity != Vector2.zero)
        {
            friction = -frictionCoefficient_kinetic * f_normal.magnitude * particleVelocity.normalized; 
        }


        
        return friction;
    }
   
    // f_drag = (p * u^2 * area * coeff)/2
    public static Vector2 GenerateForce_drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        float dragMag = (fluidDensity * (particleVelocity.magnitude * particleVelocity.magnitude) * objectArea_crossSection*objectDragCoefficient) / 2;
        float dragDir = 1;

        Vector2 drag = new Vector2(dragMag, dragDir);

        return drag;
    }
    // f_spring = -coeff*(spring length - spring resting length)
    public static Vector2 GenerateForce_spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        //both floats are arbitrary
        //coeff = stiffness
        //length (float) = particlepos - anchorpos => magnitude of the vector
        // direction - comes from the length calculation -> direction = normalise (/by length)

        Vector2 displacement = (particlePosition - anchorPosition);

        float springLength = displacement.magnitude;

        float f_strength = -springStiffnessCoefficient * (springLength - springRestingLength);

        Vector2 springForce = (displacement * f_strength)/springLength;

        return springForce;
    }
}
