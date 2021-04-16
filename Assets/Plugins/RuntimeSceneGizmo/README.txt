= Runtime Scene Gizmo =

Online documentation available at: https://github.com/yasirkula/UnityRuntimeSceneGizmo
E-mail: yasirkula@gmail.com

1. ABOUT
This asset helps you display a runtime scene gizmo as a UI element in your Unity projects. An event is raised when a component of the gizmo is clicked.

2. HOW TO
Simply add Plugins/RuntimeSceneGizmo/Prefabs/GizmoRenderer to your UI canvas and position it as you like. Gizmo will always have 1:1 aspect ratio and, by default, will be placed at the top-right corner of GizmoRenderer. You can change the RenderTarget child object's pivot for a different placement.

To invoke a function when a gizmo component is clicked, use the On Component Clicked event of the GizmoRenderer object. Functions registered to this event should ideally take a GizmoComponent parameter (which is defined in RuntimeSceneGizmo namespace).

Gizmo's rotation will match the main camera's rotation but it is possible to change the reference object via *GizmoRenderer*'s **Reference Transform** property.

See Plugins/RuntimeSceneGizmo/Demo/DemoScene for an example scene.