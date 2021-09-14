using System.Linq;
using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Spectre.Console;

namespace Shops.Console.Base.Views
{
    public class TableView : View
    {
        private readonly ITableViewDelegate _delegate;

        public TableView(ITableViewDelegate @delegate)
        {
            _delegate = @delegate;
        }

        public override void DrawBody()
        {
            Table table = new Table
            {
                Border = _delegate.GetBorder(),
            };

            int columnCount = _delegate.GetColumnCount();
            for (int i = 0; i < columnCount; i++)
            {
                table.AddColumn(_delegate.GetHeaderCellFor(new IndexPath(i, -1)));
            }

            for (int row = 0; row < _delegate.GetRowCount(); row++)
            {
                int row1 = row;
                table.AddRow(Enumerable.Range(0, columnCount)
                                 .Select(column => _delegate.GetCellFor(new IndexPath(column, row1))));
            }

            AnsiConsole.Render(table);
        }
    }
}