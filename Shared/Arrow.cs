namespace Zebble
{
    using System;
    using System.Threading.Tasks;
    public class Arrow : Canvas
    {
        Canvas Inner = new Canvas { CssClass = "inner" };

        public enum ArrowDirections { Top, Bottom, Right, Left }
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
            await Add(Inner);

            await base.OnInitializing();

            // TODO: the following line should be removed when the iOS rotation problem solved.
            await WhenShown(() => Inner.Rotation(45));
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