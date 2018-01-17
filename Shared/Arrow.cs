namespace Zebble
{
    using System;
    using System.Threading.Tasks;
    public class Arrow : Canvas
    {
        public enum ArrowDirections { Top, Bottom, Right, Left }

        Canvas Canvas = new Canvas { CssClass = "inner" };
        
        public ArrowDirections Direction { get; private set; } = ArrowDirections.Bottom;

        public Arrow()
        {
            CssClass = "arrow point-down";
            ClipChildren = true;
        }

        public Task SetDirection(ArrowDirections direction)
        {
            var cssClass = CssClass.Remove(GetCssClassFor(Direction)) + GetCssClassFor(direction);
            Direction = direction;
            return SetCssClass(cssClass);
        }

        public override async Task OnInitializing()
        {
            //WhenShown(() => this.ZIndex(1));
            await Add(Canvas);
            await base.OnInitializing();
        }

        string GetCssClassFor(ArrowDirections direction)
        {
            switch (direction)
            {
                case ArrowDirections.Top:
                    return "point-up";

                case ArrowDirections.Bottom:
                    return "point-down";

                default:
                    return $"point-{direction.ToString().ToLower()}";
            }
        }
    }
}
