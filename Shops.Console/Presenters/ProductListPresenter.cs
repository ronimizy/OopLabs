using System;
using System.Collections.Generic;
using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.Presenters;
using Shops.Console.Base.Views;
using Shops.Entities;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Shops.Console.Presenters
{
    public class ProductListPresenter : NavigatedPresenter, ITableViewDelegate
    {
        private readonly IReadOnlyList<Product> _products;

        public ProductListPresenter(IReadOnlyList<Product> products)
        {
            _products = products;

            View = new TableView(this);
        }

        public override string Title => "Product List";

        public TableBorder GetBorder()
            => TableBorder.Rounded;

        public int GetColumnCount()
            => 2;

        public int GetRowCount()
            => _products.Count;

        public TableColumn GetHeaderCellFor(IndexPath indexPath)
        {
            string[] headers = { "Name", "Description" };
            return new TableColumn(headers[indexPath.Column]);
        }

        public IRenderable GetCellFor(IndexPath indexPath)
        {
            Product product = _products[indexPath.Row];

            return indexPath.Column switch
            {
                0 => new Text(product.Name),
                1 => new Text(product.Description),
                _ => throw new InvalidOperationException("Invalid column"),
            };
        }
    }
}