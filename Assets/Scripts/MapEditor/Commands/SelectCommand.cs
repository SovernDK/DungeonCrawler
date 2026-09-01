namespace Crawler.MapEditor
{
    public class SelectCommand : MapEditorCommand
    {
        public SelectCommand(string name) : base(name)
        {
        }

        public override void Execute(MapEditor mapEditor)
        {
            base.Execute(mapEditor);

            TileController tileController = mapEditor.Context.grid.GetTile(mapEditor.CurrentCoordinate);
            if(tileController != null)
            {
                mapEditor.SelectedTile = tileController;
                mapEditor.tileInfoWindow.gameObject.SetActive(true);
                mapEditor.tileInfoWindow.Tile = tileController;
                mapEditor.tileInfoWindow.Draw();
            }
            else 
            {
                mapEditor.tileInfoWindow.gameObject.SetActive(false);
            }
        }
    }
}