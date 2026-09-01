namespace Crawler.MapEditor
{
    public class EraseCommand : MapEditorCommand
    {
        public EraseCommand(string name) : base(name)
        {
        }

        public override void Execute(MapEditor mapEditor)
        {
            base.Execute(mapEditor);
            mapEditor.Context.grid.EraseTile(mapEditor.CurrentCoordinate);
        }
    }
}