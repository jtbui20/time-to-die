using DG.Tweening;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace DefaultNamespace.CustomAnimations
{
    public class CameraReference : MonoBehaviour
    {
        public Vector3 originalPosition;
        public Vector3 originalRotation;

        public float SafeStartingDistance = 10f;
        
        public void Start()
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation.eulerAngles;
            
        }
        
        public Vector3 MakePositionToTargetWithZoom(Vector3 targetPosition, float zoomFactor)
        {
            
            Vector3 position = Vector3.Lerp(transform.position, targetPosition, zoomFactor);

            return position;
        }

        public void FixedFollowDistance(Vector3 targetPosition, float distance)
        {
            // Stay exactly distance away
            Vector3 directionToTarget = Quaternion.LookRotation(targetPosition - transform.position).eulerAngles;
            transform.DORotate(directionToTarget, 0.2f);
            
            Vector3 targetPositionWithDistance = targetPosition - (transform.forward * distance);
            transform.DOMove(targetPositionWithDistance, 0.2f);
        }

        public void ZoomToTarget(Vector3 targetPosition, float zoomFactor, bool autoTween = true)
        {
            Vector3 target = MakePositionToTargetWithZoom(targetPosition, zoomFactor);
            if (autoTween)
            {
                transform.DOMove( target, 0.2f);
            }
            else
            {
                transform.position = target;
            }
            
            
            Vector3 directionToTarget = Quaternion.LookRotation(targetPosition - transform.position).eulerAngles;
            transform.DORotate(directionToTarget, 0.2f);
        }

        public void ZoomToTarget(Vector3 start, Vector3 targetPosition, float zoomFactor)
        {
            Vector3 target = Vector3.Lerp(start, targetPosition, zoomFactor);
            transform.position = target;
            Vector3 directionToTarget = Quaternion.LookRotation(transform.position, targetPosition).eulerAngles;
            transform.DORotate(directionToTarget, 0.2f);
        }

        public Vector3 MoveToSafeStartingPosition(Vector3 targetPosition)
        {
            // Move to a position on the lerp line but it's a safe distance away from the target position
            Vector3 directionLine = (targetPosition - transform.position);
            if (directionLine.magnitude > SafeStartingDistance)
            {
                return transform.position;
            }
            Vector3 directionIdentity = directionLine.normalized;
            Vector3 safePosition = targetPosition - directionIdentity * SafeStartingDistance;
            transform.DOMove(safePosition, 0.1f);
            return safePosition;
        }
        

        public void Reset()
        {
            transform.DOMove(originalPosition, 0.5f);
            transform.DORotate(originalRotation, 0.5f);
        }
    }
}