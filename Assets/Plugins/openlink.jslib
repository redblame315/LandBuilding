mergeInto(LibraryManager.library, {
    OpenNewTab : function(url)
    {
        url = UTF8ToString(url);
        window.open(url, '_blank');
    },
    });
