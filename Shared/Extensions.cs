namespace Zebble
{
    using System;
    using System.Threading.Tasks;

    public static class Extensions
    {
        public static Task<PopOver> PopOver(this View view, string help) => view.PopOver(new TextView(help));

        public static async Task<PopOver> PopOver(this View view, View content)
        {
            if (view.Id == null) throw new Exception("The view that you are trying to add a PopOver should have an Id.");

            var popOver = new PopOver(view, content);
            await View.Root.Add(popOver);
            await popOver.Show();
            return popOver;
        }
    }
}