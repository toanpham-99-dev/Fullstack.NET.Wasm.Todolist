(function () {
    window.QuillFunctions = {
        createQuill: function (quillElement) {
            var options = {
                debug: 'info',
                modules: {
                    toolbar: '#toolbar'
                },
                placeholder: 'Compose an epic...',
                readOnly: false,
                theme: 'snow'
            };
            // set quill at the object we can call
            // methods on later
            new Quill(quillElement, options);
        },
        getQuillHTML: function (quillControl) {
            return quillControl.__quill.root.innerHTML;
        },
        getQuillContent: function (quillControl) {
            return JSON.stringify(quillControl.__quill.getContents());
        },
        loadQuillContent: function (quillControl, quillContent) {
            content = JSON.parse(quillContent);
            console.log('content');
            console.log(content);
            console.log('quillControl');
            console.log(quillControl);
            return quillControl.__quill.setContents(content, 'api');
        },
        disableQuillEditor: function (quillControl) {
            quillControl.__quill.enable(false);
        }
    };
})();
