using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RuntimeSceneGizmo
{
	[System.Serializable]
	public class ComponentClickedEvent : UnityEvent<GizmoComponent>
	{
	}

	public class SceneGizmoRenderer : MonoBehaviour, IPointerClickHandler, IDragHandler
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL || UNITY_FACEBOOK || UNITY_WSA || UNITY_WSA_10_0
		, IPointerEnterHandler, IPointerExitHandler
#else
		, IPointerDownHandler, IPointerUpHandler
#endif
	{
#pragma warning disable 0649
		[SerializeField]
		private RawImage imageHolder;
		private RectTransform imageHolderTR;

		[SerializeField]
		private SceneGizmoController controller;

		[SerializeField]
		[Tooltip( "Should gizmo's hovered components turn yellow" )]
		private bool highlightHoveredComponents = true;
		private PointerEventData hoveringPointer;

		[SerializeField]
		[Tooltip( "(Optional) Gizmo will match the reference Transform's rotation" )]
		private Transform m_referenceTransform;
		public Transform ReferenceTransform
		{
			get { return m_referenceTransform; }
			set
			{
				m_referenceTransform = value;
				controller.ReferenceTransform = value;
			}
		}

		[SerializeField]
		private ComponentClickedEvent m_onComponentClicked;
		public ComponentClickedEvent OnComponentClicked { get { return m_onComponentClicked; } }
#pragma warning restore 0649

		private void Awake()
		{
			imageHolderTR = (RectTransform) imageHolder.transform;
			controller = (SceneGizmoController) Instantiate( controller );

			imageHolder.texture = controller.TargetTexture;
		}

		private void Start()
		{
			if( m_referenceTransform != null && !m_referenceTransform.Equals( null ) )
				controller.ReferenceTransform = m_referenceTransform;
		}

		private void OnEnable()
		{
			if( controller != null && !controller.Equals( null ) )
				controller.gameObject.SetActive( true );
		}

		private void OnDisable()
		{
			if( controller != null && !controller.Equals( null ) )
				controller.gameObject.SetActive( false );
		}

		private void Update()
		{
			if( hoveringPointer != null )
				controller.OnPointerHover( GetNormalizedPointerPosition( hoveringPointer ) );
		}

		public void OnPointerClick( PointerEventData eventData )
		{
			if( eventData.dragging )
				return;

			GizmoComponent hitComponent = controller.Raycast( GetNormalizedPointerPosition( eventData ) );
			if( hitComponent != GizmoComponent.None )
				m_onComponentClicked.Invoke( hitComponent );
		}

		public void OnDrag( PointerEventData eventData )
		{
		}

		private Vector3 GetNormalizedPointerPosition( PointerEventData eventData )
		{
			Vector2 localPos;
			Vector2 size = imageHolderTR.rect.size;
			RectTransformUtility.ScreenPointToLocalPointInRectangle( imageHolderTR, eventData.position, null, out localPos );

			return new Vector3( 1f + localPos.x / size.x, 1f + localPos.y / size.y, 0f );
		}

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL || UNITY_FACEBOOK || UNITY_WSA || UNITY_WSA_10_0
		public void OnPointerEnter( PointerEventData eventData )
		{
			if( highlightHoveredComponents )
				hoveringPointer = eventData;
		}

		public void OnPointerExit( PointerEventData eventData )
		{
			if( hoveringPointer != null )
			{
				controller.OnPointerHover( new Vector3( -10f, -10f, 0f ) );
				hoveringPointer = null;
			}
		}
#else
		public void OnPointerDown( PointerEventData eventData )
		{
			if( highlightHoveredComponents )
				hoveringPointer = eventData;
		}

		public void OnPointerUp( PointerEventData eventData )
		{
			if( hoveringPointer != null )
			{
				controller.OnPointerHover( new Vector3( -10f, -10f, 0f ) );
				hoveringPointer = null;
			}
		}
#endif
	}
}