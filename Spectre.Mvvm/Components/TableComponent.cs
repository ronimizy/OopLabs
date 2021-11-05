using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Spectre.Mvvm.Components
{
    public class TableComponent : Component
    {
        private readonly string _title;
        private readonly IReadOnlyCollection<TableColumn> _headers;
        private readonly IReadOnlyCollection<IReadOnlyCollection<IRenderable>> _data;

        public TableComponent(
            string title,
            IReadOnlyCollection<TableColumn> headers,
            IReadOnlyCollection<IReadOnlyCollection<IRenderable>> data)
        {
            if (data.Any() && headers.Count != data.First().Count)
                throw new InvalidOperationException("Header column count does not match the data column count");

            _title = title;
            _headers = headers;
            _data = data;
        }

        public override void Draw()
        {
            Table table = new Table
            {
                Border = TableBorder.Rounded,
                Title = new TableTitle(_title),
                Expand = true,
            };

            table.AddColumns(_headers.ToArray());

            foreach (IReadOnlyCollection<IRenderable> row in _data)
            {
                table.AddRow(row);
            }

            AnsiConsole.Write(table);
        }
    }
}