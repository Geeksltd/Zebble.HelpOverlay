namespace Zebble
{
    using System;
    using System.Xml.Linq;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class UserHelp : View
    {
        View Owner;
        float BalloonYPosition = 15, OwnerYPosition, OwnerXPosition;

        public readonly Overlay UserHelpOverlay = new Overlay();
        public readonly Balloon Balloon = new Balloon();
        public readonly Arrow Arrow = new Arrow();
        public readonly Canvas Container = new Canvas();

        public string Help { get; set; }
        public string For { get; set; }
        public string Gesture { get; set; }
        public string UserHelpId
        {
            get
            {
                if (!For.HasValue())
                    throw new Exception("The For property of UserHelp did not set, please set it before using the UserHelp object");
                return $"HelpContainer_{For}";
            }
        }
        public bool IsRunning { get; set; }

        static UserHelp()
        {
            Nav.Navigating.Handle(async nav =>
            {
                await UserHelpCollection.ClearByPage(nav.From.GetType().Name);
            });

            Nav.Navigated.Handle(() =>
            {
                Nav.CurrentPage.WhenShown(async () => await UserHelpCollection.MoveNext());
            });
        }

        public async override Task OnInitializing()
        {
            UserHelpOverlay.ZIndex(1).Width(100.Percent()).Height(100.Percent())
                .Opacity(0.35f).Background(color: Colors.Black);

            Balloon.ZIndex(3).Absolute().Border(radius: 5, color: Colors.Black)
                .Margin(all: 10).Background(color: Colors.White);

            Balloon.HelpText.Text(Help).Padding(all: 20);

            Arrow.Absolute().Width(50).Height(35).Rotation(45).Opacity(0)
                .Background(color: Colors.White).Y(10);

            Container.Absolute().ZIndex(1).Id(UserHelpId).Opacity(0).Visible(value: false)
               .Width(100.Percent()).Height(100.Percent());

            if (Device.Platform.IsAndroid() || Device.Platform.IsIOS())
                BalloonYPosition = 0;

            await base.OnInitializing();
        }

        public async override Task OnRendered()
        {
            await base.OnRendered();

            await Init();
        }

        async Task Init()
        {
            if (IsDismissed()) return;
            if (Nav.CurrentPage.FindDescendent<UserHelp>() == null) return;

            Owner = FindView(For, Nav.CurrentPage);

            if (Owner == null)
            {
                Device.Log.Error($"[UserHelp]: There is not any object with {For} identification on this page.");
                return;
            }

            HandleEvents();

            await Container.Add(Arrow);
            await Container.Add(UserHelpOverlay);
            await Container.Add(Balloon);

            await UserHelpCollection.AddHelpToPage(this);
        }

        public async Task Show()
        {
            IsRunning = true;
            await Nav.CurrentPage.Add(Container);
            await CalculateThePositions();
            Container.Visible();
            await Container.Animate(new Animation
            {
                Delay = 2.Seconds(),
                Easing = AnimationEasing.EaseIn,
                EasingFactor = EasingFactor.Cubic,
                Change = () => { Container.Style.Opacity = 1; },
                Duration = Animation.FadeDuration
            });

            Arrow.Opacity(1);
        }

        public async Task Hide()
        {
            Arrow.Opacity(0);
            await Container.Animate(new Animation
            {
                Delay = 2.Seconds(),
                Easing = AnimationEasing.Linear,
                Change = () => Container.Style.Opacity = 0,
                Duration = Animation.FadeDuration
            });

            IsRunning = false;
            await Nav.CurrentPage.Remove(Container);
        }

        async Task Dismiss()
        {
            RegisterHelp();
            Arrow.Opacity(0);
            await Container.Animate(new Animation
            {
                Delay = 2.Seconds(),
                Easing = AnimationEasing.Linear,
                Change = () => Container.Style.Opacity = 0,
                Duration = Animation.FadeDuration
            });

            UserHelpCollection.UserHelps[UserHelpCollection.CurrentPageName].Remove(this);
            await Nav.CurrentPage.Remove(Container);

            await UserHelpCollection.MoveNext();
        }

        async Task CalculateThePositions()
        {
            var parentScrollView = Owner.FindParent<ScrollView>();

            if (parentScrollView == null)
            {
                OwnerYPosition = Owner.ActualY;

                await CalculateArrowPostion();
                await CalculateBalloonPosition();
            }
            else
            {
                OwnerYPosition = Owner.GetActualYPosition();

                if (parentScrollView.Direction == RepeatDirection.Vertical)
                {
                    var helpActualYPosiotion = OwnerYPosition + Owner.ActualHeight + Balloon.ActualHeight;

                    if (helpActualYPosiotion >= Root.ActualHeight)
                    {
                        await parentScrollView.ScrollTo(yOffset: OwnerYPosition - (Owner.ActualHeight + Balloon.ActualHeight), animate: true);
                        Balloon.Y(parentScrollView.ActualY + Owner.ActualHeight);
                    }
                    else
                    {
                        await parentScrollView.ScrollTo(yOffset: 0, animate: true);
                        await CalculateBalloonPosition();
                    }

                    await CalculateArrowPostion();
                }
                else
                {
                    OwnerXPosition = Owner.GetActualXPosition();
                    var helpActualYPosiotion = OwnerXPosition + Owner.ActualWidth;

                    float scrollValue = 0;
                    if (helpActualYPosiotion >= Root.ActualWidth)
                        scrollValue = OwnerXPosition;

                    await parentScrollView.ScrollTo(xOffset: scrollValue, animate: true);
                    await CalculateBalloonPosition();
                    await CalculateArrowPostion(RepeatDirection.Horizontal, helpActualYPosiotion);
                }
            }

            await Task.CompletedTask;
        }

        Task CalculateArrowPostion(RepeatDirection direction = RepeatDirection.Vertical, float helpActualYPosiotion = 0)
        {
            if (Arrow.IsYBoundedToBalloon)
            {
                if (Arrow.Direction == Arrow.ArrowDirections.Bottom) Arrow.Y(Balloon.ActualBottom - (Arrow.ActualHeight + BalloonYPosition));
                else Arrow.Y(Balloon.ActualY + (Arrow.ActualHeight - BalloonYPosition));

                var arrowXPosition = Owner.ActualX + Owner.ActualWidth / 2;
                if (direction == RepeatDirection.Vertical)
                    Arrow.X(arrowXPosition);
                else
                {
                    if (helpActualYPosiotion >= Root.ActualWidth)
                        Arrow.X((Balloon.ActualWidth - Math.Abs(Owner.ActualWidth - Balloon.ActualWidth)) / 2);
                    else
                        Arrow.X(arrowXPosition + Arrow.ActualHeight / 2);
                }
            }
            else
                Arrow.Y(OwnerYPosition + Arrow.Y.CurrentValue).X(Owner.ActualX + Owner.ActualWidth / 2);

            return Task.CompletedTask;
        }

        Task CalculateBalloonPosition()
        {
            if (OwnerYPosition < Balloon.ActualHeight + Arrow.ActualHeight)
            {
                Arrow.Direction = Arrow.ArrowDirections.Top;
                Balloon.Y(OwnerYPosition + Owner.ActualHeight + (Arrow.ActualHeight / 2));
                if (Device.Platform.IsAndroid() || Device.Platform.IsIOS())
                    BalloonYPosition = 35;
                else BalloonYPosition = 47;
            }
            else
                Balloon.Y(OwnerYPosition - (Balloon.ActualHeight + Arrow.ActualHeight / 2));

            return Task.CompletedTask;
        }

        bool IsDismissed()
        {
            var xmlFile = Device.IO.File("/UserHelp.xml");
            if (xmlFile.Exists())
            {
                var xDoc = XElement.Load(xmlFile.FullName);
                foreach (var element in xDoc.Elements())
                {
                    if (element.Attribute("Name").Value == UserHelpId)
                        return true;
                }
            }

            return false;
        }

        void RegisterHelp()
        {
            var xmlFile = Device.IO.File("/UserHelp.xml");
            if (xmlFile.Exists())
            {
                var xDoc = XElement.Load(xmlFile.FullName);
                xDoc.Add(new XElement("HelpOverlay", new XAttribute("Name", UserHelpId), new XAttribute("Help", Help), new XAttribute("For", For)));
                xmlFile.WriteAllText(xDoc.ToString());
            }
            else
            {
                xmlFile.WriteAllText("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<data>" +
                    "\n<HelpOverlay Name=\"" + UserHelpId + "\" Help=\"" + Help + "\" For=\"" + For + "\" />\n</data>");
            }
        }

        void HandleEvents()
        {
            Container.Tapped.Handle(() => Dismiss());
            Balloon.CloseButton.Tapped.Handle(() => Dismiss());
            Balloon.NextHelpButton.Tapped.Handle(UserHelpCollection.ShowNext);
        }

        View FindView(string id, View parent)
        {
            var views = new List<View>();
            View view = null;
            foreach (var child in parent.AllChildren)
            {
                if (child.Id == id)
                    views.Add(child);
                else
                {
                    view = FindView(id, child);
                    if (view != null) views.Add(view);
                }
            }

            if (views.Count > 1)
                throw new Exception($"[UserHelp]: There are different object with \"{For}\" identification on this page.");
            else if (views.Count == 0) return null;
            return views.First();
        }
    }
}
