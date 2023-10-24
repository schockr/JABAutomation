using JABAutomation.AutoIt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;

namespace JABAutomation
{
    public class JavaTable
    {
        private IJavaElement element;
        private TreeNode treeEl;
        private PropertyList properties;
        private Size size;
        private Point tableCoordinates;
        private IEnumerable<PropertyGroup> tableInfo;

        public JavaTable(IJavaElement el)
        {
            this.element = el;
            this.treeEl = element.Element;
            this.properties = treeEl.GetProperties();
            this.size = element.Size;
            this.tableCoordinates = new Point(element.Coordinates.LocationOnScreen.X, element.Coordinates.LocationOnScreen.Y);
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

        public Size Size
        {
            get { return this.size; }
        }

        public Point TableCoordinates
        {
            get { return this.tableCoordinates; }
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
                    string rowColDef = RowColDefByIndex(i, j);
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

        public Point GetTableRowCoordinates(string cellName, int targetColIndex = -1, int targetRowIndex = -1)
        {
            List<PropertyNode> tableCells = this.TableCells.ToList();

            foreach (PropertyGroup cellGroup in tableCells)
            {
                string rowcol = cellGroup.Name;
                if (TryParseRowCol(rowcol, out int rowNum, out int rowCount, out int colNum, out int colCount))
                {
                    if ((targetColIndex < 0 && targetRowIndex < 0) 
                        || colNum == targetColIndex 
                        || rowNum == targetRowIndex 
                        && IsCellNameMatch(cellGroup, cellName))
                    {
                        // calculate the row coordinates
                        int rowHeight = Size.Height / rowCount;

                        // offset by one due to indexing (row 1 starts at table coordinate 0)
                        int currentY = TableCoordinates.Y + (rowHeight * (rowNum - 1)); 

                        // Create Coordinate for top left of table row 
                        return new Point(TableCoordinates.X, currentY);
                    }
                }
                else
                {
                    throw new InvalidElementStateException("The row column group name format failed to parse: " + rowcol);
                }
            }

            return new Point(-1, -1);
        }

        private bool TryParseRowCol(string rowCol, out int rowNum, out int rowCount, out int colNum, out int colCount)
        {
            Match match = RegexMatchRowCol(rowCol);
            if (match.Success)
            {
                rowNum = int.Parse(match.Groups[1].Value);
                rowCount = int.Parse(match.Groups[2].Value);
                colNum = int.Parse(match.Groups[3].Value);
                colCount = int.Parse(match.Groups[4].Value);
                return true;
            }
            else
            {
                rowNum = rowCount = colNum = colCount = 0;
                return false;
            }
        }

        private bool IsCellNameMatch(PropertyGroup cellGroup, string cellName)
        {
            PropertyNode cellText = cellGroup.Children.FirstOrDefault(group => group.Name == "Name");
            if (cellText != null)
            {
                string cellTextStr = cellText.Value.ToString();
                return cellTextStr == cellName;
            }
            return false;
        }


        private Match RegexMatchRowCol(string rowcol)
        {
            string pattern = @"\[Row (\d+)/(\d+), Col (\d+)/(\d+)\]";

            return Regex.Match(rowcol, pattern);
        }

    }
}
