using Microsoft.AspNetCore.Components; 

namespace MainTainSenseBlazor.Shared 
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
