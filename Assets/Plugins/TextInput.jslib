mergeInto(LibraryManager.library, {
  GetWebTextInput: function () {
    var str = window.prompt("Enter your name");
    var bufferSize = lengthBytesUTF8(str) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(str, buffer, bufferSize);
    return buffer;
  },
  IsMobile : function() {
    return Module.SystemInfo.mobile;
  }

});