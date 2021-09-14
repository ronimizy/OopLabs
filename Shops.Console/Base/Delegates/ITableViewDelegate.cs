using Shops.Console.Base.Models;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Shops.Console.Base.Delegates
{
    public interface ITableViewDelegate
    {
        TableBorder GetBorder();

        int GetColumnCount();
        int GetRowCount();

        TableColumn GetHeaderCellFor(IndexPath indexPath);
        IRenderable GetCellFor(IndexPath indexPath);
    }
}