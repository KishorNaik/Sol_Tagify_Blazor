/// <reference path="lib/jquery-3.6.0.min.js" />
/// <reference path="lib/tagify.min.js" />
/// <reference path="lib/tagify.min.js" />

var myInput = undefined;

export function addTags(inputElementReference, allowDuplicates) {
    myInput = $(inputElementReference).tagify({ duplicates: allowDuplicates });

    myInput.on('change', function (e) {
        console.log(e.isTrigger);
        if (e.isTrigger === 2) {
            let tagValues = myInput.val();

            DotNet.invokeMethodAsync("Tagify", "OnSelectedTag", tagValues);
        }
    });
}