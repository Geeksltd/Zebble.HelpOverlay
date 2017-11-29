namespace Zebble
{
    using System.Threading.Tasks;
    public class Balloon : Stack
    {
        bool HasFinishText;

        public readonly Button CloseButton = new Button();
        public readonly Button NextHelpButton = new Button();
        public readonly TextView HelpText = new TextView();

        public async override Task OnInitializing()
        {
            await base.OnInitializing();

            await Add(CloseButton);
            await Add(HelpText);
            await Add(NextHelpButton);
        }

        public async override Task OnRendered()
        {
            await base.OnRendered();

            CloseButton.Text("x").TextAlignment(Alignment.Middle).Width(20).Height(20);
            NextHelpButton.Text(HasFinishText ? "Finish" : "Next >").TextAlignment(Alignment.Middle).Width(70).Height(20).X(ActualWidth - 72)
                .Border(color: Colors.Silver, all: 1, radius: 5);
        }

        public void SetFinishButton() => HasFinishText = true;
    }
}
