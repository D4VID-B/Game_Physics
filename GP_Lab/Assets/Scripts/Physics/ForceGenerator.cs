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

        return Vector2.zero;
    }
    // f_sliding = f_gravity + f_normal
    public static Vector2 GenerateForce_sliding(Vector2 f_gravity, Vector2 f_normal)
    {

        return Vector2.zero;
    }
    // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)
    public static Vector2 GenerateForce_friction_static(Vector2 f_normal, Vector2 f_opposing, float frictionCoefficient_static)
    {

        return Vector2.zero;
    }
    // f_friction_k = -coeff*|f_normal| * unit(vel)
    public static Vector2 GenerateForce_friction_kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic)
    {

        return Vector2.zero;
    }
    // f_drag = (p * u^2 * area * coeff)/2
    public static Vector2 GenerateForce_drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {

        return Vector2.zero;
    }
    // f_spring = -coeff*(spring length - spring resting length)
    public static Vector2 GenerateForce_spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {

        return Vector2.zero;
    }
}
