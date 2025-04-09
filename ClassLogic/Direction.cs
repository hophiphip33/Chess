using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ClassLogic
{
    public class Direction
    {
        public static readonly Direction North = new Direction(-1, 0);
        public static readonly Direction South = new Direction(1, 0);
        public static readonly Direction East = new Direction(0, 1);
        public static readonly Direction West = new Direction(0, -1);
        public static readonly Direction NorthEast = North+East;
        public static readonly Direction NorthWest = North + West;
        public static readonly Direction SouthEast = South + East;
        public static readonly Direction SouthWest = South + West;
        public int RowDelta { get; }
        public int ColumnDelta { get; }
         public Direction(int rowDelta, int columnDelta) {  RowDelta = rowDelta; ColumnDelta = columnDelta; }
        public static Direction operator +(Direction dir1, Direction dir2)
        {
            return new Direction(dir1.RowDelta + dir2.RowDelta, dir1.ColumnDelta + dir2.ColumnDelta);
        }
        public static Direction operator *(int scalar, Direction dir) { 
        return new Direction(scalar * dir.RowDelta, scalar * dir.ColumnDelta);
        }
    }
        
    
}
