using System.Collections;
using UnityEngine;

namespace RuntimeSceneGizmo
{
	public class CameraGizmoListener : MonoBehaviour
	{
#pragma warning disable 0649
		[SerializeField]
		private float cameraAdjustmentSpeed = 3f;

		[SerializeField]
		private float projectionTransitionSpeed = 2f;
#pragma warning restore 0649

		private Camera mainCamera;

		private IEnumerator cameraRotateCoroutine, projectionChangeCoroutine;

		private void Awake()
		{
			mainCamera = GetComponent<Camera>();
		}

		private void OnDisable()
		{
			cameraRotateCoroutine = projectionChangeCoroutine = null;
		}

		public void OnGizmoComponentClicked( GizmoComponent component )
		{
			if( component == GizmoComponent.Center )
				SwitchOrthographicMode();
			else if( component == GizmoComponent.XNegative )
				RotateCameraInDirection( Vector3.right );
			else if( component == GizmoComponent.XPositive )
				RotateCameraInDirection( -Vector3.right );
			else if( component == GizmoComponent.YNegative )
				RotateCameraInDirection( Vector3.up );
			else if( component == GizmoComponent.YPositive )
				RotateCameraInDirection( -Vector3.up );
			else if( component == GizmoComponent.ZNegative )
				RotateCameraInDirection( Vector3.forward );
			else
				RotateCameraInDirection( -Vector3.forward );
		}

		public void SwitchOrthographicMode()
		{
			if( projectionChangeCoroutine != null )
				return;

			projectionChangeCoroutine = SwitchProjection();
			StartCoroutine( projectionChangeCoroutine );
		}

		public void RotateCameraInDirection( Vector3 direction )
		{
			if( cameraRotateCoroutine != null )
				return;

			cameraRotateCoroutine = SetCameraRotation( direction );
			StartCoroutine( cameraRotateCoroutine );
		}

		// Credit: https://forum.unity.com/threads/smooth-transition-between-perspective-and-orthographic-modes.32765/#post-212814
		private IEnumerator SwitchProjection()
		{
			bool isOrthographic = mainCamera.orthographic;

			Matrix4x4 dest, src = mainCamera.projectionMatrix;
			if( isOrthographic )
				dest = Matrix4x4.Perspective( mainCamera.fieldOfView, mainCamera.aspect, mainCamera.nearClipPlane, mainCamera.farClipPlane );
			else
			{
				float orthographicSize = mainCamera.orthographicSize;
				float aspect = mainCamera.aspect;
				dest = Matrix4x4.Ortho( -orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, mainCamera.nearClipPlane, mainCamera.farClipPlane );
			}

			for( float t = 0f; t < 1f; t += Time.unscaledDeltaTime * projectionTransitionSpeed )
			{
				float lerpModifier = isOrthographic ? t * t : Mathf.Pow( t, 0.2f );
				Matrix4x4 matrix = new Matrix4x4();
				for( int i = 0; i < 16; i++ )
					matrix[i] = Mathf.LerpUnclamped( src[i], dest[i], lerpModifier );

				mainCamera.projectionMatrix = matrix;
				yield return null;
			}

			mainCamera.orthographic = !isOrthographic;
			mainCamera.ResetProjectionMatrix();

			projectionChangeCoroutine = null;
		}

		private IEnumerator SetCameraRotation( Vector3 targetForward )
		{
			Quaternion initialRotation = transform.localRotation;
			Quaternion targetRotation;
			if( Mathf.Abs( targetForward.y ) < 0.99f )
				targetRotation = Quaternion.LookRotation( targetForward );
			else
			{
				Vector3 cameraForward = transform.forward;
				if( cameraForward.x == 0f && cameraForward.z == 0f )
					cameraForward.y = 1f;
				else if( Mathf.Abs( cameraForward.x ) > Mathf.Abs( cameraForward.z ) )
				{
					cameraForward.x = Mathf.Sign( cameraForward.x );
					cameraForward.y = 0f;
					cameraForward.z = 0f;
				}
				else
				{
					cameraForward.x = 0f;
					cameraForward.y = 0f;
					cameraForward.z = Mathf.Sign( cameraForward.z );
				}

				if( targetForward.y > 0f )
					cameraForward = -cameraForward;

				targetRotation = Quaternion.LookRotation( targetForward, cameraForward );
			}

			for( float t = 0f; t < 1f; t += Time.unscaledDeltaTime * cameraAdjustmentSpeed )
			{
				transform.localRotation = Quaternion.LerpUnclamped( initialRotation, targetRotation, t );
				yield return null;
			}

			transform.localRotation = targetRotation;
			cameraRotateCoroutine = null;
		}
	}
}