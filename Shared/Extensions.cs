namespace Zebble
{
    using System;
    using System.Linq;

    public static class Extensions
    {
        public static void UserHelp(this View view, string help, string gesture)
        {
            if (view.Id == null) throw new Exception("The owner view should have identification");

            var userHelp = new UserHelp
            {
                For = view.Id,
                Help = help,
                Gesture = gesture
            };

            view.Parent.Add(userHelp);
        }

        public static float GetActualYPosition(this View view)
        {
            var parents = view.WithAllParents().TakeWhile(x => x != Nav.CurrentPage).ToList();
            return parents.Sum(y => y.ActualY);
        }

        public static float GetActualXPosition(this View view)
        {
            var parents = view.WithAllParents().TakeWhile(x => x != Nav.CurrentPage).ToList();
            return parents.Sum(x => x.ActualX);
        }
    }
}
