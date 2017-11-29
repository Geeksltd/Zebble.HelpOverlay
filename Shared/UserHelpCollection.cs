namespace Zebble
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class UserHelpCollection
    {
        public static int CurrentIndex = 0;

        public static Dictionary<string, List<UserHelp>> UserHelps { get; set; } = new Dictionary<string, List<UserHelp>>();
        public static string CurrentPageName => Nav.CurrentPage.GetType().Name;

        static UserHelpCollection() { Nav.CurrentPage.WhenShown(async () => await MoveNext()); }

        public static Task AddHelpToPage(UserHelp view)
        {
            if (!UserHelps.Any(uh => uh.Key == CurrentPageName))
                UserHelps.Add(CurrentPageName, new List<UserHelp> { view });
            else
            {
                if (!UserHelps[CurrentPageName].Any(uh => uh.UserHelpId == view.UserHelpId))
                    UserHelps[CurrentPageName].Add(view);
            }

            return Task.CompletedTask;
        }

        public static Task ClearByPage(string name)
        {
            if (UserHelps.Any(uh => uh.Key == name)) UserHelps[name] = new List<UserHelp>();
            CurrentIndex = 0;

            return Task.CompletedTask;
        }

        public static async Task MoveNext()
        {
            if (UserHelps.Any(uh => uh.Key == CurrentPageName))
            {
                var view = UserHelps[CurrentPageName].First();
                if (!view.IsRunning)
                {
                    if (CurrentIndex > 0)
                        view = UserHelps[CurrentPageName][CurrentIndex];

                    if (CurrentIndex + 1 >= UserHelps[CurrentPageName].Count)
                        view.Balloon.SetFinishButton();
                    else if (CurrentIndex > 0) CurrentIndex -= 1;

                    await view.Show();
                }
            }
        }

        public static async Task ShowNext()
        {
            if (UserHelps.Any(uh => uh.Key == CurrentPageName))
            {
                var priorView = UserHelps[CurrentPageName][CurrentIndex];
                CurrentIndex += 1;
                if (CurrentIndex < UserHelps[CurrentPageName].Count)
                {
                    var nextView = UserHelps[CurrentPageName][CurrentIndex];
                    if (CurrentIndex + 1 >= UserHelps[CurrentPageName].Count) nextView.Balloon.SetFinishButton();

                    if (!nextView.IsRunning)
                    {
                        await priorView.Hide();
                        await nextView.Show();
                    }
                }
                else await priorView.Hide();
            }
        }
    }
}
