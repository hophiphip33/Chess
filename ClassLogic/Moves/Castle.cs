using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLogic
{
    public class Castle : Move
    {
        public override MoveType Type { get; }
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        private readonly Direction kingMoveDir;

        private readonly Position rookFromPos;
        private readonly Position rookToPos;
        public Castle(MoveType type, Position kingPos)
        { 
            Type = type;
            FromPos = kingPos;
            if (type ==MoveType.CastleKS)
            {
                kingMoveDir = Direction.East;
                ToPos = new Position(kingPos.Row, 6);
                rookFromPos = new Position(kingPos.Row, 7);
                rookToPos = new Position(kingPos.Row, 5);

            }
            else if (type == MoveType.CastleQS)
            {
                kingMoveDir = Direction.West;
                ToPos = new Position(kingPos.Row, 2);
                rookFromPos = new Position(kingPos.Row, 0);
                rookToPos = new Position(kingPos.Row, 3);
            }
            
        }
        public override void Execute(Board board)
        {
            new NormalMove(FromPos, ToPos).Execute(board);//Di chuyển vua trước, sau đó xe, bằng cách tạo ra hai nước đi bình thường 
            new NormalMove(rookFromPos, rookToPos).Execute(board);
        }
        public override bool IsLegal(Board board)
        {
            Player player = board[FromPos].Color;
            if (board.IsInCheck(player))// vua bị chiếu
            {
                return false;
            }
            Board copy = board.Copy();
            Position kingPosInCopy = FromPos;
            for(int i = 0; i < 2; i++)
            {
                new NormalMove(kingPosInCopy, kingPosInCopy + kingMoveDir).Execute(copy);// các ô trống có bị chiếu k
                kingPosInCopy += kingMoveDir;
                if (copy.IsInCheck(player))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
