using Grids;

namespace World
{
    public static class GridExtension
    {
        /// <summary>
        /// Устанавливает SceneGridSettings для Grid
        /// </summary>
        public static void ApplySceneGridSettings<T>(this Grid<T> grid, SceneGridSettings settings)
        {
            grid.CellSizeHorizontal = settings.CellSizeHorizontal;
            grid.CellSizeVertical = settings.CellSizeVertical;
            
            grid.Resize(settings.Width,settings.Height);
        }
    }
}