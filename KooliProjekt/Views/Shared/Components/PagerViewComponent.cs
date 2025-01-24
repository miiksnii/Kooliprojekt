﻿using Kooliprojekt.Data;
using Microsoft.AspNetCore.Mvc;

namespace Kooliprojekt.Views.Shared.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
