namespace Zebble
{
    using System;
    using System.Xml.Linq;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class PopOver : Canvas
    {
        View Owner;
        View Content;

        public readonly Stack Balloon = new Stack { CssClass = "balloon" };
        public readonly Arrow Arrow = new Arrow();
        

        internal PopOver(View owner, View content)
        {
            Owner = owner;
            Content = content;


            content.SetCssClass(content.CssClass + " content");
            CssClass = "pop-over-container";
        }

        public override async Task OnInitializing()
        {
            await base.OnInitializing();

            await Balloon.Add(new TextView { CssClass = "close" }
                .Text("x")
                .On(x => x.Tapped, Hide));
            await Balloon.Add(Content);
            await Add(Arrow);
            await Add(Balloon);
            
            await CalculateThePositions();
        }
        
        internal async Task Show()
        {
            await BringToFront();
            await this.Animate(new Animation
            {
                Delay = 2.Seconds(),
                Easing = AnimationEasing.EaseIn,
                EasingFactor = EasingFactor.Cubic,
                Change = () => { Style.Opacity = 1; },
                Duration = Animation.FadeDuration
            });
        }

        public async Task Hide()
        {
            await this.Animate(new Animation
            {
                Delay = 2.Seconds(),
                Easing = AnimationEasing.Linear,
                Change = () => Style.Opacity = 0,
                Duration = Animation.FadeDuration
            });

            await this.RemoveSelf();
        }

        async Task CalculateThePositions()
        {
            var ownerY = Owner.CalculateAbsoluteY();
            var ownerX = Owner.CalculateAbsoluteX();
            
            var balloonHeight = Balloon.ActualHeight > 0 ? Balloon.ActualHeight : Balloon.CurrentChildren.Sum(x => x.ActualHeight); 
            var arrowHeight = Arrow.ActualHeight > 0 ? Arrow.ActualHeight : Arrow.CurrentChildren.Sum(x => x.ActualHeight);
            
            if (ownerY > balloonHeight + arrowHeight)
            {
                Balloon.Y.BindTo(Balloon.Height, Arrow.Height, (b, a) => ownerY - b - a / 2);
                Arrow.Y.BindTo(Arrow.Height, a => ownerY - a / 2);
            }
            else
            {
                await Arrow.SetDirection(Arrow.ArrowDirections.Top);

                Balloon.Y.BindTo(Owner.Height, Arrow.Height, (o, a) => ownerY + o + a / 2);
                Arrow.Y.BindTo(Owner.Height, Arrow.Height, (o, a) => ownerY + o - a / 2);
            }

            Arrow.X.BindTo(Owner.Width, Arrow.Width, (o, a) => ownerX + o / 2 - a / 2);
        }
    }
}
