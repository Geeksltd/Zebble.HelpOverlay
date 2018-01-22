using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zebble
{
    public static class Extensions
    {
        public static Task<PopOver> ShowPopOver(this View owner, string help) => owner.ShowPopOver(new TextView(help));
        public static async Task<PopOver> ShowPopOver(this View owner, View content)
        {
            if (owner.Id == null) throw new Exception("The owner view should have identification");
            var result = new PopOver(owner, content);
            await View.Root.Add(result);
            await result.Show();
            return result;
        }
    }
}
