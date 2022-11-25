mergeInto(LibraryManager.library, {
    OpenNewTab : function(url)
    {
        url = UTF8ToString(url);
        window.open(url, '_blank');
    },
    GetURLFromPage: function () {
        var returnStr = window.top.location.href;
        var bufferSize = lengthBytesUTF8(returnStr) + 1
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    });
