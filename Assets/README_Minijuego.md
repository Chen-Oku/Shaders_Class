# Minijuego: Reparar esculturas

Este mini proyecto añade un sistema simple para un minijuego donde el jugador arregla o reemplaza esculturas en una plaza frente a un museo.

Archivos añadidos (ubicación):

- `Assets/Scripts/Minigame/DragAndDrop.cs` - Comportamiento para arrastrar piezas con el ratón.
- `Assets/Scripts/Minigame/RepairPiece.cs` - Script que identifica una pieza reparable.
- `Assets/Scripts/Minigame/RepairSlot.cs` - Slots en la plaza que aceptan piezas (snap).
- `Assets/Scripts/Minigame/MinigameManager.cs` - Contiene lógica básica del minijuego y contador.
- `Assets/Editor/MinigameSceneGenerator.cs` - Herramienta del editor (Tools/Minigame) para crear una escena base: `Assets/Scenes/Minigame_Museo_Plaza.unity`.

Cómo usar

1. En Unity, abre el menú Tools → Minigame → Crear escena museo-plaza. Esto generará una escena con objetos placeholder en `Assets/Scenes`.
2. Abre la escena y ajusta modelos reales: reemplaza los cubos/primitivos por tus mallas (museo, esculturas, piezas).
3. Para cada objeto que sea una pieza arrastrable, añade `RepairPiece` y `DragAndDrop` y un `Rigidbody` (opcional: desactivar gravedad si no quieres que caiga).
4. Para cada posición objetivo añade un GameObject con `RepairSlot` y configura `expectedPieceId` para que coincida con `pieceId` de la pieza; asigna `snapTarget` si quieres un punto de snap distinto.
5. Añade un `MinigameManager` en la escena (si no existe) para ver el contador en consola o conecta un `UI.Text` al campo `statusText`.

Notas y siguientes pasos recomendados

- Mejorar la detección de toque para dispositivos móviles.
- Añadir animaciones cuando se coloca una pieza y efectos sonoros.
- Añadir undo, rollback y niveles de dificultad.
- Reemplazar primitivas por los assets de tu proyecto (modelos, materiales, iluminación HDRP/URP según tu proyecto).

Si quieres, puedo ajustar los scripts para admitir multi-touch, efectos visuales, o crear prefabs listos para usar con tus modelos; dime qué prefieres que haga a continuación.
