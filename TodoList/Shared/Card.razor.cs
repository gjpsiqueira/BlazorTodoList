using System;
using Microsoft.AspNetCore.Components;

namespace TodoList.Shared
{
    public partial class Card
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
        [Parameter]
        public EventCallback<Guid> OnDeleteCallback { get; set; }
    }
}
