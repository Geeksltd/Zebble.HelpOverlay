namespace Zebble
{
    using System.Threading.Tasks;
    public class Arrow : Stack
    {
        public bool IsXBoundedToBalloon { get; set; } = true;
        public bool IsYBoundedToBalloon { get; set; } = true;
        public ArrowDirections Direction { get; set; } = ArrowDirections.Bottom;

        public enum ArrowDirections
        {
            Top = 0,
            Bottom = 1,
            Right = 2,
            Left = 3
        }

        public override Task OnInitializing()
        {
            WhenShown(() => this.ZIndex(1));
            return base.OnInitializing();
        }
    }
}
