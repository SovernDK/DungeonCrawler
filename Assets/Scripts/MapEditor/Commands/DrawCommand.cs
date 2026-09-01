namespace Crawler.MapEditor
{
    public class DrawCommand : MapEditorCommand
    {
        public DrawCommand(string name) : base(name) { }

        public override void Execute(MapEditor mapEditor)
        {
            base.Execute(mapEditor);
            mapEditor.Context.grid.InsertTile((int) mapEditor.type, mapEditor.CurrentCoordinate, mapEditor.CurrentRotation);
        }
    }
}