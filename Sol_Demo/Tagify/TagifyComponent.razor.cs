using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagify
{
    public partial class TagifyComponent
    {
        #region Private Variables

        private Task<IJSObjectReference> _module = null;

        #endregion Private Variables

        #region Inject

        [Inject]
        public IJSRuntime JavascriptRuntime { get; set; }

        #endregion Inject

        #region Tagify Loads (Step 1)

        #region Parameters

        [Parameter]
        public String PlaceHolder { get; set; }

        [Parameter]
        public bool AllowDuplicates { get; set; }

        #endregion Parameters

        #region Private Property

        private ElementReference InputElementReference { get; set; }

        #endregion Private Property

        #region Private Method

        private void LoadJsModules()
        {
            _module = JavascriptRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Tagify/inputTagify.js").AsTask();
        }

        private async Task LoadTagifyJs()
        {
            await (await _module).InvokeVoidAsync(identifier: "addTags", InputElementReference, AllowDuplicates);
        }

        #endregion Private Method

        #endregion Tagify Loads (Step 1)

        #region Tagify Gets (Step 2)

        #region Private Property

        private static Action<List<TagifyData>> GetTagifyAction { get; set; }

        #endregion Private Property

        #region Public Property

        [Parameter]
        public List<TagifyData> SelectedTags { get; set; }

        [Parameter]
        public EventCallback<List<TagifyData>> SelectedTagsChanged { get; set; }

        //Alternative Solution for above Property if we dont want two way binding
        [Parameter]
        public EventCallback<List<TagifyData>> OnSelectTagsEvent { get; set; }

        #endregion Public Property

        #region Static Method

        [JSInvokable]
        public static Task OnSelectedTag(string datajson)
        {
            var data = JsonConvert.DeserializeObject<List<TagifyData>>(datajson);

            GetTagifyAction?.Invoke(data);

            return Task.CompletedTask;
        }

        #endregion Static Method

        #endregion Tagify Gets (Step 2)

        #region Protected Method

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Step 1
                this.LoadJsModules();

                await LoadTagifyJs();

                // Step 2
                GetTagifyAction = async (List<TagifyData> tags) =>
                {
                    await base.InvokeAsync(async () =>
                    {
                        await SelectedTagsChanged.InvokeAsync(tags);

                        await OnSelectTagsEvent.InvokeAsync(tags); // alternative Solution if we dont want two way binding
                    });
                };

                base.StateHasChanged();
            }
        }

        #endregion Protected Method
    }
}