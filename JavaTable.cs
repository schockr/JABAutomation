using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace JABAutomation
{
    public class JavaTable
    {
        private TreeNode element;
        private PropertyList properties;
        private IEnumerable<PropertyGroup> tableInfo;

        public JavaTable(TreeNode element)
        {
            this.element = element;
            this.properties = element.GetProperties();

            TableInfo = GetAccessibleTableInfo();
        }

        public IEnumerable<PropertyGroup> TableInfo
        {
            get
            {
                return this.tableInfo;
            }
            set { this.tableInfo = value; }
        }
        public int TableRowCount
        {
            get
            {
                return (int)TableInfo
                    .SelectMany(group => group.Children)
                    .OfType<PropertyNode>()
                    .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, "Row count")).FirstOrDefault().Value;
            }
        }
        public int TableColumnCount
        {
            get
            {
                return (int)TableInfo
                    .SelectMany(group => group.Children)
                    .OfType<PropertyNode>()
                    .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, "Column count")).FirstOrDefault().Value;
            }
        }
        public string TableName
        {
            get
            {
                return TableInfo.FirstOrDefault()?.Name;
            }
        }
        public string Caption
        {
            get
            {
                return (string)TableInfo.SelectMany(group => group.Children)
                    .OfType<PropertyGroup>()
                    .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, "caption"))
                    .FirstOrDefault().Value;
            }
        }
        public string Summmary
        {
            get
            {
                return (string)TableInfo.SelectMany(group => group.Children)
                    .OfType<PropertyGroup>()
                    .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, "summary"))
                    .FirstOrDefault().Value;
            }
        }
        public IEnumerable<PropertyNode> ColumnHeaders
        {
            get
            {
                return TableInfo
                    .SelectMany(group => group.Children)
                    .OfType<PropertyGroup>()
                    .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, "column headers"))
                    .SelectMany(subGroup => subGroup.Children)
                    .OfType<PropertyGroup>()
                    .Where(subSubGroup => StringUtils.EqualsIgnoreCase(subSubGroup.Name, "cells"))
                    .SelectMany(subSubGroup => subSubGroup.Children);
            }
        }

        public IEnumerable<PropertyNode> RowHeaders
        {
            get
            {
                return TableInfo
                    .SelectMany(group => group.Children)
                    .OfType<PropertyGroup>()
                    .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, "row headers"))
                    .SelectMany(subGroup => subGroup.Children)
                    .OfType<PropertyGroup>()
                    .Where(subSubGroup => StringUtils.EqualsIgnoreCase(subSubGroup.Name, "cells"))
                    .SelectMany(subSubGroup => subSubGroup.Children);
            }
        }

        public List<PropertyNode> TableCells
        {
            get
            {
                return TableInfo
                    .SelectMany(group => group.Children)
                    .OfType<PropertyGroup>()
                    .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, "cells"))
                    .SelectMany(subGroup => subGroup.Children)
                    .ToList();
            }
        }

        public List<PropertyNode> TableSelectCells
        {
            get
            {
                return TableInfo
                    .SelectMany(group => group.Children)
                    .OfType<PropertyGroup>()
                    .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, "select cells"))
                    .SelectMany(subGroup => subGroup.Children)
                    .ToList();
            }
        }

        public List<PropertyNode> TableCellsChildren
        {
            get
            {
                return TableInfo
                    .SelectMany(group => group.Children)
                    .OfType<PropertyGroup>()
                    .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, "cells"))
                    .SelectMany(subGroup => subGroup.Children)
                    .OfType<PropertyGroup>()
                    .SelectMany(subsubgroup => subsubgroup.Children)
                    .ToList();
            }
        }

        public List<PropertyNode> GetSelectCellByRowColIndex(int rowIndex, int colIndex)
        {
            string rowColDef = RowColDefByIndex(rowIndex, colIndex);
            return GetCellByRowColIndex("select cells", rowColDef);
        }

        public List<PropertyNode> GetSelectCellByName(string name)
        {
            for (int i = 1; i <= TableRowCount; i++)
            {
                for (int j = 1; j <= TableColumnCount; j++)
                {
                    string rowColDef = RowColDefByIndex(i,j);
                    List<PropertyNode> cell = GetCellByRowColIndex("select cells", rowColDef);

                    foreach (PropertyNode child in cell)
                    {
                        if (child.Name == "Name" && child.Value.ToString() == name)
                            return cell;
                    }
                }
            }
            return null;
        }


        public List<PropertyNode> GetCellByRowColIndex(int rowIndex, int colIndex)
        {
            string rowColDef = RowColDefByIndex(rowIndex, colIndex);
            return GetCellByRowColIndex("cells", rowColDef);
        }

        private string RowColDefByIndex(int rowIndex, int colIndex)
        {
            return "[Row " + rowIndex + "/" + TableRowCount + ", Col " + colIndex + "/" + TableColumnCount + "]";
        }

        public AccessibleRectInfo GetSelectCellCoordinates(int rowIndex, int colIndex)
        {
            string rowColDef = RowColDefByIndex(rowIndex, colIndex);
            IEnumerable<PropertyNode> cell = GetCellNodesByRowColIndex("select cells", rowColDef);

            AccessibleRectInfo rectInfo = (AccessibleRectInfo)cell
                .Where(subSubgroup => StringUtils.EqualsIgnoreCase(subSubgroup.Name, "Bounds"))
                .Select(subSubGroup => subSubGroup.Value)
                .FirstOrDefault();

            return rectInfo;
        }

        private List<PropertyNode> GetCellByRowColIndex(string groupName, string rowColDef)
        {
            var tableCells = GetCellNodesByRowColIndex(groupName, rowColDef).ToList();

            return tableCells;
        }

        private IEnumerable<PropertyNode> GetCellNodesByRowColIndex(string groupName, string rowColDef)
        {
            //TODO when the cell is already selected,
            //the JVM doesn't seem to recognize it and returns null here
            //Need to diagnose & resolve adequately
            var tableCells = TableInfo
                .SelectMany(group => group.Children)
                .OfType<PropertyGroup>()
                .Where(subGroup => StringUtils.EqualsIgnoreCase(subGroup.Name, groupName))
                .SelectMany(subGroup => subGroup.Children)
                .OfType<PropertyGroup>()
                .Where(cell => cell.Name == rowColDef)
                .SelectMany(cell => cell.Children)
                .OfType<PropertyNode>();
            return tableCells;
        }

        private IEnumerable<PropertyGroup> GetAccessibleTableInfo()
        {
            var table = properties
                .OfType<PropertyGroup>()
                .Where(group => StringUtils.EqualsIgnoreCase(group.Name, "Table"));

            return table;
        }
    }
}
