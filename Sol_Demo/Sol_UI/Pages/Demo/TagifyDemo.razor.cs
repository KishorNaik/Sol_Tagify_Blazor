using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tagify;

namespace Sol_UI.Pages.Demo
{
    public partial class TagifyDemo
    {
        #region Private Property

        private TagifyComponent TagifyComponentRef { get; set; }

        private List<TagifyData> SelectedTags { get; set; }

        #endregion Private Property

        #region Event

        //using Event
        private void OnGetTags(List<TagifyData> tagifyDatas)
        {
            SelectedTags = tagifyDatas;
        }

        private void OnSubmit()
        {
            SelectedTags
                .ForEach((TagifyData tags) =>
                {
                    Debug.WriteLine(tags.value);
                });
        }

        #endregion Event
    }
}