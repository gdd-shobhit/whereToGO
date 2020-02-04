using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    // Vectors for the physics
    public Vector3 position;
    public Vector3 direction;
    public Vector3 velocity;
    public Vector3 acceleration;
    public float radius;
    private float time;
    Vector3 positionToWander;
    public float separateForce;
    public Terrain terrain;

    // The mass of the object. Note that this can't be zero
    public float mass = 1;

    public float maxSpeed = 4;

    public const float MIN_VELOCITY = 0.1f;



    private void Start()
    {
        // Initialize all the vectors
        position = transform.position;
        direction = Vector3.right;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        radius = gameObject.transform.localScale.x / 2; 
        time = Mathf.Infinity;
        positionToWander = Vector3.zero;
    }

    private void Update()
    {
        CalcSteeringForces();
      
        // Then, calculate the physics
        UpdatePhysics();
        // Finally, update the position
        UpdatePosition();
    }

    /// <summary>
    /// Updates the physics properties of the vehicle
    /// </summary>
    private void UpdatePhysics()
    {
        // Add acceleration to velocity, and have that be scaled with time
        velocity += acceleration * Time.deltaTime;
        

        // Change the position based on velocity over time
        position += velocity * Time.deltaTime;

        // Calculate the direction vector
        direction = velocity.normalized;

        // Reset the acceleration for the next frame
        acceleration = Vector3.zero;
    }

    /// <summary>
    /// Wraps the vehicle around the screen
    /// </summary>
    private void Bounce()
    {
        Camera cam = Camera.main;
        Vector3 max = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        Vector3 min = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));

        if (position.x > 25f && velocity.x > 0)
        {
            velocity.x *= -1;
        }
        if (position.z > 25f && velocity.z > 0)
        {
            velocity.z *= -1;
        }
        if (position.x < -25f && velocity.x < 0)
        {
            velocity.x *= -1;
        }
        if (position.z < -25f && velocity.z < 0)
        {
            velocity.z *= -1;
        }
    }

    /// <summary>
    /// Wraps the vehicle around the screen
    /// </summary>
    private void Wrap()
    {
        Camera cam = Camera.main;
        Vector3 max = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        Vector3 min = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));

        if (position.x > max.x && velocity.x > 0)
        {
            position.x = min.x;
        }
        if (position.y > max.y && velocity.y > 0)
        {
            position.y = min.y;
        }
        if (position.x < min.x && velocity.x < 0)
        {
            position.x = max.x;
        }
        if (position.y < min.y && velocity.y < 0)
        {
            position.y = max.y;
        }
    }

    /// <summary>
    /// Update the vehicle's position
    /// </summary>
    private void UpdatePosition()
    {
        // Atan2 determines angle of velocity against the right vector
        //float angle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
        // Sets the rotation of the vehicle to face towards it's velocity
        transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);

        // Update position
        gameObject.transform.position = position;
    }

    /// <summary>
    /// Applies friction to the vehicle
    /// </summary>
    /// <param name="coeff">The coefficient of friction</param>
    protected void ApplyFriction(float coeff)
    {
        // If the velocity is below a minimum value, just stop the vehicle
        if (velocity.magnitude < MIN_VELOCITY)
        {
            velocity = Vector3.zero;
            return;
        }

        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;
        acceleration += friction;
    }

    /// <summary>
    /// Applies a force to the vehicle
    /// </summary>
    /// <param name="force">The force to be applied</param>
    public void ApplyForce(Vector3 force)
    {
        // Make sure the mass isn't zero, otherwise we'll have a divide by zero error
        if (mass == 0)
        {
            Debug.LogError("Mass cannot be zero!");
            return;
        }

        // Add our force to the acceleration for this frame
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(force);
        //acceleration += force / mass;
    }

    public Vector3 Seek(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = targetPosition - position;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        //Debug.DrawLine(position, position + steeringForce, Color.red);
        return steeringForce;
    }

    public float DistanceTo(Vector3 targetPosition)
    {
        return Mathf.Sqrt(Mathf.Pow(this.position.x - targetPosition.x, 2)+ Mathf.Pow(this.position.y - targetPosition.y, 2)+ Mathf.Pow(this.position.z - targetPosition.z, 2));
    }

    public Vector3 Seek(GameObject targetObj)
    {
        return Seek(targetObj.transform.position);
    }

    private Vector3 Flee(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = position - targetPosition;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        return steeringForce;
    }

    public Vector3 Flee(GameObject targetObject)
    {
        return Flee(targetObject.transform.position);
    }

    protected abstract void CalcSteeringForces();


    //public Vector3 ObstacleAvoidance(Obstacle obs)
    //{
    //    Vector3 desiredVelocity = Vector3.zero;
    //    Vector3 vectorToObstacle = obs.transform.position - this.position;      
    //    float weight = 0;
    //    float rightVectorDot = Vector3.Dot(this.transform.right, vectorToObstacle);
    //    float distance = vectorToObstacle.magnitude - obs.radius;
    //    // checking if the obstacle is in certain distance
    //    if (Math.Abs(rightVectorDot) < radius + obs.radius)
    //    {
    //        // calc weight
    //        if (distance <= 0)
    //        {
    //            weight = float.MaxValue;
    //        }
    //        else
    //        {
    //            // 2f is the safe distance hardcoded
    //            weight = (float)Math.Pow(2f/distance, 2f);
    //        }
    //        // clamping weight
    //        weight = Mathf.Min(weight, 100);
        
    //        if (distance < 10f)
    //        {
    //            // If the obstacle is in left
    //            if (rightVectorDot < 0)
    //            {
    //                // if left then is it colliding?
                   
    //                if (Math.Abs(rightVectorDot) < radius + obs.radius)
    //                {
    //                    desiredVelocity = transform.right;
    //                    desiredVelocity *= maxSpeed;
                        
    //                }
    //            }
    //            // Checking if obstacle is right
    //            else
    //            {
    //                // if right then is it colliding?
    //                if (Math.Abs(rightVectorDot) < radius +obs.radius)
    //                {
    //                    desiredVelocity = -transform.right;
    //                    desiredVelocity *= maxSpeed;

    //                }
    //            }
                
    //        }

          

    //    }
    //    Vector3 steeringForce =(desiredVelocity-velocity)* weight;
    //    return steeringForce;

    //}

    public Vector3 GetFuturePosition(float seconds = 1f)

    {
        return position + velocity * seconds;
    }


    public Vector3 KeepInPark()
    {
        if (GetFuturePosition(1.5f).x > 95)
        {
            return Seek(new Vector3(terrain.terrainData.size.x/2, terrain.terrainData.size.y/2, terrain.terrainData.size.z/2));
        }
        if(GetFuturePosition(1.5f).x < 5)
        {
            return Seek(new Vector3(terrain.terrainData.size.x / 2, terrain.terrainData.size.y / 2, terrain.terrainData.size.z / 2));
        }
        if (GetFuturePosition(1.5f).z > 95)
        {
            return Seek(new Vector3(terrain.terrainData.size.x / 2, terrain.terrainData.size.y / 2, terrain.terrainData.size.z / 2));
        }
        if (GetFuturePosition(1.5f).z < 5)
        {
            return Seek(new Vector3(terrain.terrainData.size.x / 2, terrain.terrainData.size.y / 2, terrain.terrainData.size.z / 2));
        }
        else
        {
            return Vector3.zero;
        }
        
    }

    protected Vector3 Separate(Vector3 targetPosition, float desiredDistance)
    {
        // Calculate distance to the other object
        float distanceToTarget = Vector3.Distance(position, targetPosition);

        // if the distance is basically 0, then it's probably me'
        if (distanceToTarget <= float.Epsilon)
        {
            return Vector3.zero;
        }

        // Flee away from the other object
        Vector3 fleeForce = Flee(targetPosition);

        // Scale the force based on how close I am
        fleeForce = fleeForce.normalized * Mathf.Pow(desiredDistance / distanceToTarget, 2);

        // Draw that force
        Debug.DrawLine(position, position + fleeForce, Color.cyan);
        separateForce = fleeForce.magnitude;
      
        return fleeForce;
    }

    protected Vector3 Align(Vector3 averageVelocity)
    {
        Vector3 desiredVelocity = averageVelocity;
        desiredVelocity.Normalize();
        desiredVelocity*= maxSpeed;
        Vector3 steerForce = desiredVelocity - velocity;
        
        return steerForce;
    }


    protected Vector3 Wander()
    {
        Vector3 wanderForce = Vector3.zero;
        time += Time.deltaTime;
        if (time > 2)
        {
            Vector3 circleCenter = GetFuturePosition(1.5f);
           
            positionToWander = circleCenter + GetRandomPositionOnCircle();
            time = 0f;
        }
        
        Debug.DrawLine(position, positionToWander, Color.green);
        if (positionToWander != Vector3.zero)
        {
            wanderForce = Seek(positionToWander);
            return wanderForce;
        }
        return Vector3.zero;
        
    }

    public Vector3 GetRandomPositionOnCircle()
    {
        // circle would be a 5 radius Circle
        float randomAngle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
        Vector3 positionOfPoint = Vector3.zero;
        positionOfPoint.z = Mathf.Sin(randomAngle) * 5f;
        positionOfPoint.x = Mathf.Cos(randomAngle) * 5f;
        return positionOfPoint;
    }

    public Vector3 TerrainCheck()
    {
        Vector3 force = Vector3.zero;
        Vector3 futurePos = GetFuturePosition();
        float distance = DistanceTo(futurePos);
        float weight = Mathf.Pow(distance, 2);
        if (futurePos.y <= terrain.SampleHeight(futurePos))
        {
            force += this.Seek(new Vector3(futurePos.x, terrain.SampleHeight(futurePos), futurePos.z)) * weight;
        }
        return force; 
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        // Make sure that mass isn't set to 0
        mass = Mathf.Max(mass, 0.0001f);
    }
#endif
}
