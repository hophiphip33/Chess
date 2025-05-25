

namespace ClassLogic
{
    public class DoublePawn : Move
    {
        public override MoveType Type => MoveType.DoublePawn;
        public override Position FromPos { get; }
        public override Position ToPos { get; }
        private readonly Position skippedPos;// ô mà quân tốt đã "bỏ qua

        public DoublePawn(Position fromPos, Position toPos)
        {
            FromPos = fromPos;
            ToPos = toPos;
            skippedPos = new Position((fromPos.Row + toPos.Row) / 2, fromPos.Column);
        }

        public override bool Execute(Board board)
        {
            Player player = board[FromPos].Color;
            
            board.SetPawnSkipPosition(player, skippedPos);
            new NormalMove(FromPos, ToPos).Execute(board);

            return true;// trả về true vì quân tốt đã di chuyển 2 ô
        }
    }
}
