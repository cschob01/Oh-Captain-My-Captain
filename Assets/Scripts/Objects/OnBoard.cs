using System;
using System.Drawing;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEditor.SpeedTree.Importer;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.PlayerSettings;

public class OnBoard : MonoBehaviour
{
    public MovingObject movingObject = null;
    private Vector2 momentum = Vector2.zero;

    public Collider2D mapCol;
    public Collider2D playerCol;

    protected Rigidbody2D object_rb;

    void Awake()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TransformOnBoard();
        // KeepInside();
    }

    void TransformOnBoard()
    {
        Vector2 next_pos = transform.position;

        /////////////////////////////////////////////////////////
        //Spin
        /////////////////////////////////////////////////////////
        float cos = Mathf.Cos(Ship.Instance.spin * Time.fixedDeltaTime);
        float sin = Mathf.Sin(Ship.Instance.spin * Time.fixedDeltaTime);
        next_pos -= Ship.Instance.center;    // Get relative to center
        next_pos = new Vector2(              // Rotate
            next_pos.x * cos - next_pos.y * sin,
            next_pos.x * sin + next_pos.y * cos
        );
        next_pos += Ship.Instance.center;    // Bring back

        // Rotate momentum to compensate for spin
        momentum = new Vector2(
            momentum.x * cos - momentum.y * sin,
            momentum.x * sin + momentum.y * cos
        );

        // Rotate vel to compensate for spin
        if (movingObject != null)
        {
            movingObject.vel = new Vector2(
                movingObject.vel.x * cos - movingObject.vel.y * sin,
                movingObject.vel.x * sin + movingObject.vel.y * cos
            );
        }

        /////////////////////////////////////////////////////////
        //Grav
        /////////////////////////////////////////////////////////
        // pos -= Ship.Instance.vel * Time.fixedDeltaTime;

        /////////////////////////////////////////////////////////
        //Vel
        /////////////////////////////////////////////////////////
        next_pos += (momentum - Ship.Instance.vel) * Time.fixedDeltaTime;

        transform.position = next_pos;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        bool[] touching = { false, false, false, false };

        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;

            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            {
                if (normal.x > 0)
                {
                    //Debug.Log("Hit wall on LEFT side of player");
                    touching[0] = true;
                }
                else
                {
                    //Debug.Log("Hit wall on RIGHT side of player");
                    touching[0] = true;
                    touching[2] = true;
                }
            }
            else
            {
                if (normal.y > 0)
                {
                    //Debug.Log("Hit from BELOW (floor)");
                    touching[1] = true;
                }
                else
                {
                    //Debug.Log("Hit from ABOVE (ceiling)");
                    touching[1] = true;
                    touching[3] = true;
                }
            }
        }

        // Getting force vector due to spin
        Vector2 pos = transform.position;
        Vector2 radius = pos - Ship.Instance.center;
        Vector2 tangent = new Vector2(-radius.y, radius.x);
        Vector2 spin_force = tangent * -Ship.Instance.spin;
        ////// Total momentum of bumped-into wall
        Vector2 wal = Ship.Instance.vel + spin_force;

        if (movingObject != null) // Modify vel + Momentum
        {
            if (touching[0])
            {
                if ((movingObject.vel.x < 0) == touching[2]) // Facing away from wall
                {
                    movingObject.vel.x = 0;
                    momentum.x = wal.x;
                }
                else // Facing wall
                {
                    if (touching[2]) // Facing Right wall
                    {
                        movingObject.vel.x = Mathf.Max(wal.x - momentum.x, 0);
                        momentum.x = Mathf.Min(momentum.x, wal.x);
                    }
                    else // Facing Left wall
                    {
                        movingObject.vel.x = Mathf.Min(wal.x - momentum.x, 0);
                        momentum.x = Mathf.Max(momentum.x, wal.x);
                    }
                }
            }

            if (touching[1])
            {
                if ((movingObject.vel.y < 0) == touching[3]) // Facing away from wall
                {
                    movingObject.vel.y = 0;
                    momentum.y = wal.y;
                }
                else // Facing wall
                {
                    if (touching[3]) // Facing Up wall
                    {
                        movingObject.vel.y = Mathf.Max(wal.y - momentum.y, 0);
                        momentum.y = Mathf.Min(momentum.y, wal.y);
                    }
                    else // Facing Down wall
                    {
                        movingObject.vel.y = Mathf.Min(wal.y - momentum.y, 0);
                        momentum.y = Mathf.Max(momentum.y, wal.y);
                    }

                }
            }
        }
        else
        {
            // Idle objects simply pushed by ship
            if (touching[0]) momentum.x = wal.x;
            if (touching[1]) momentum.y = wal.y;
        }

    }

    void KeepInside()
    {
        Vector2 pos = transform.position;

        ColliderDistance2D dist = mapCol.Distance(playerCol);

        if (dist.distance <= 0f) // Outside
        {
            Vector2 normal = dist.normal;

            Debug.Log("Normal: " + normal);

            bool rightWall = normal.x < -0.5f;
            bool leftWall = normal.x > 0.5f;
            bool upWall = normal.y < -0.5f;
            bool downWall = normal.y > 0.5f;

            bool[] touching = {
                rightWall || leftWall,
                upWall || downWall,
                rightWall, // Right wall
                upWall, // Up wall
            };

            if (touching[0] || touching[1])
            {
                // Getting force vector due to spin
                Vector2 radius = pos - Ship.Instance.center;
                Vector2 tangent = new Vector2(-radius.y, radius.x);
                Vector2 spin_force = tangent * -Ship.Instance.spin;
                ////// Total momentum of bumped-into wall
                Vector2 wal = Ship.Instance.vel + spin_force;

                if (movingObject != null) // Modify vel + Momentum
                {
                    if (touching[0])
                    {
                        if ((movingObject.vel.x < 0) == touching[2]) // Facing away from wall
                        {
                            movingObject.vel.x = 0;
                            momentum.x = wal.x;
                        }
                        else // Facing wall
                        {
                            if (touching[2]) // Facing Right wall
                            {
                                movingObject.vel.x = Mathf.Max(wal.x - momentum.x, 0);
                                momentum.x = Mathf.Min(momentum.x, wal.x);
                            }
                            else // Facing Left wall
                            {
                                movingObject.vel.x = Mathf.Min(wal.x - momentum.x, 0);
                                momentum.x = Mathf.Max(momentum.x, wal.x);
                            }
                        }
                    }

                    if (touching[1])
                    {
                        if ((movingObject.vel.y < 0) == touching[3]) // Facing away from wall
                        {
                            movingObject.vel.y = 0;
                            momentum.y = wal.y;
                        }
                        else // Facing wall
                        {
                            if (touching[3]) // Facing Up wall
                            {
                                movingObject.vel.y = Mathf.Max(wal.y - momentum.y, 0);
                                momentum.y = Mathf.Min(momentum.y, wal.y);
                            }
                            else // Facing Down wall
                            {
                                movingObject.vel.y = Mathf.Min(wal.y - momentum.y, 0);
                                momentum.y = Mathf.Max(momentum.y, wal.y);
                            }

                        }
                    }
                }
                else
                {
                    // Idle objects simply pushed by ship
                    if (touching[0]) momentum.x = wal.x;
                    if (touching[1]) momentum.y = wal.y;
                }
            }
            transform.position += (Vector3)(normal * -dist.distance);
        }
        else
        {
            Debug.Log("Not inside");
        }
    }

    private bool InMap(Vector2 obj)
    {
            Vector3Int cell = Ship.Instance.spaceship.WorldToCell(obj);
            return Ship.Instance.spaceship.HasTile(cell);
    }

}
