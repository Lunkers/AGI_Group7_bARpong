using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class FollowCameraSystem : SystemBase
{
   protected override void OnUpdate()
    {

        Entities.WithoutBurst().ForEach((ref Translation translation, ref Rotation r, in ref Throwable throwable) =>
        {
            if (!throwable.thrown)
            {
                if (Input.touchCount > 0)
                {
                    var touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Moved)
                    {
                        // var objectDistance = Vector3.Distance(Camera.main.transform.position, new Vector3(translation.Value.x, translation.Value.y, translation.Value.z))
                        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0.4f));
                        translation.Value = new float3(touchPosition.x, touchPosition.y, touchPosition.z);
                    }
                }
                else
                {
                    Camera camData = Camera.main;

                    //Debug.Log(camData.transform.position);
                    translation.Value = camData.transform.position + (camData.transform.forward * 0.4f)  - (camData.transform.up * 0.1f) ;
                    // throwable.angle = math.radians(-camData.transform.localRotation.eulerAngles.x);
                    r.Value = camData.transform.localRotation;
                }
            }


        }).Run();
    }
}
